using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class CameraTour : MonoBehaviour {

	private PlaybackControl pc;
	private float currentTime = 0;

	private List<Waypoint> waypoints = new List<Waypoint> ();
	private List<FocusPointCommand> focusPointCommands = new List<FocusPointCommand>(); // same amount as waypoints

	float s_ges = 0;

	private bool firstUpdateDone = false;

	private List<Section> sections = new List<Section>();
	private int currentSectionIndex = 0;

	private float t_ges;
	private float t_waitSum; // sum of all waiting times in waypoints

	private float accelEndMarkerPerc = 0.2f; // 1/5
	private float decelStartMarkerPerc = 0.8f; // 4/5

	private string waypointsFilepath;
	private Transform focusPoint;
	private Transform cam;
	//private GameObject cameraObj;

	private int currentSectionStartWaypointIndex = 0;
	private FocusPointCommand current_fpc;


	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		focusPoint = GameObject.Find ("FocusPoint").transform;
		cam = Camera.main.transform; // = cameraObj.transform;
		//cameraObj = GameObject.Find ("Sphere");

		waypointsFilepath = Application.dataPath + "/Data/waypoints/waypointsEvaktischBig2.csv";
		importWaypoints ();

		if (waypoints[0].doWait()) // extra check this, because i starts at 1 in following for-loop
			addWaitSection (waypoints[0]);

		for (int i = 1; i < waypoints.Count; i ++) {
			Waypoint startWaypoint = waypoints[i - 1];
			Vector3 startWaypointCoords = startWaypoint.getPoint();
			float velocReducerStart = startWaypoint.getVelocReducer();

			Waypoint endWaypoint = waypoints[i];
			Vector3 endWaypointCoords = endWaypoint.getPoint();
			float velocReducerEnd = endWaypoint.getVelocReducer();

			float dist = Vector3.Distance (startWaypointCoords, endWaypointCoords);
			//Debug.Log ("dist: " + dist);

			float s_accel = dist * accelEndMarkerPerc;
			float s_decel = dist - (dist * decelStartMarkerPerc);
			float s_const = dist - s_accel - s_decel;
			//Debug.Log (string.Concat("s_accel: ", s_accel, " s_const: ", s_const, " s_decel: ", s_decel));

			Vector3 accelEndPoint = Vector3.Lerp (startWaypointCoords, endWaypointCoords, accelEndMarkerPerc);
			Vector3 decelStartPoint = Vector3.Lerp (startWaypointCoords, endWaypointCoords, decelStartMarkerPerc);

			Section accelSect = new Section(Section.Type.ACCELERATION, startWaypointCoords, accelEndPoint, velocReducerStart, s_accel);
			//float s_upToHere = s_ges; // Debug.Log ("ACCEL-SECT: " + accelSect);
			sections.Add(accelSect);
			Section constSect = new Section(Section.Type.CONSTANT, accelEndPoint, decelStartPoint, 1, s_const);
			//float s_upToHere = s_ges + s_accel; // Debug.Log ("CONST-SECT: " + constSect);
			sections.Add(constSect);
			Section decelSect = new Section(Section.Type.DECELERATION, decelStartPoint, endWaypointCoords, velocReducerEnd, s_decel);
			//float s_upToHere = s_ges + s_accel + s_const; // Debug.Log ("DECEL-SECT: " + decelSect);
			sections.Add(decelSect);

			if (endWaypoint.doWait())
				addWaitSection(endWaypoint);

			s_ges += dist;
		}
	}
		
	private void importWaypoints() {
		string filedata = "";
		if (!System.IO.File.Exists (waypointsFilepath))
			Debug.LogError ("Error: waypoints file " + waypointsFilepath + " not found.");
		else
			filedata = System.IO.File.ReadAllText (waypointsFilepath);

		using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(filedata)))) {
			string line = reader.ReadLine (); // skip the first line = header
			while((line = reader.ReadLine ()) != null) {
				if (line.Length > 0) {
					if (line.Substring (0, 1) != "#") {
						string[] values = line.Split(',');
						float x;
						float y;
						float z;
						float velocReducer;
						float waitingTime;
						bool changeFocus;
						float.TryParse(values[0], out x);
						float.TryParse(values[1], out y);
						float.TryParse(values[2], out z);
						float.TryParse(values[3], out velocReducer);
						float.TryParse(values[4], out waitingTime);
						bool.TryParse (values [5], out changeFocus);

						Waypoint wp = new Waypoint (waypoints.Count, new Vector3 (x, y, z), velocReducer, waitingTime);
						FocusPointCommand fpc;

						if (changeFocus) {
							float.TryParse (values [6], out x);
							float.TryParse (values [7], out y);
							float.TryParse (values [8], out z);
							fpc = new FocusPointCommand (new Vector3 (x, y, z));
						} else
							fpc = new FocusPointCommand ();

						focusPointCommands.Add (fpc);
						waypoints.Add (wp);
					}
				}
			}
		}
		current_fpc = focusPointCommands[0];
	}

	private void addWaitSection(Waypoint wp) {
		Section waitSect = new Section(Section.Type.WAIT, wp.getPoint(), wp.getPoint(), 0, 0);
		waitSect.setTinSection(wp.getWaitingTime());
		sections.Add (waitSect);
		t_waitSum += wp.getWaitingTime();
	}

	private void onFirstUpdate(){ // because pc.total_time is not know when Start() is executed
		firstUpdateDone = true;

		t_ges = (float) pc.total_time - t_waitSum; // Debug.Log (pc.total_time);

		if (t_ges <= 0)
			utils.crash("the sum of waiting times in camera tour waypoints is bigger than the total simulation time");

		float v_max = 0;
		foreach (var section in sections)
			v_max += section.getFormulaContrib();		
		v_max /= t_ges; // Debug.Log ("v_max: " + v_max);

		float t_sum = 0;
		foreach (var section in sections) {
			section.calcTinSection(v_max); //TODO consolidate these three method calls into one?
			section.setTupToHere(t_sum);
			section.calcAccel();
			t_sum += section.getTinSection();
		}
		/*// TEST
		Debug.Log ("sum of t_inSection's: " + t_sum + " <- must be " + t_ges);
		foreach (var section in sections) {
			Debug.Log(section.check());
		}*/
	}

	private void newLoop() {
		currentSectionIndex = 0;
		currentSectionStartWaypointIndex = 0;
		foreach(Section sec in sections)
			sec.reset (); //TODO there should be a better way
	}

	void Update () {
		if (!firstUpdateDone)
			onFirstUpdate ();

		if (currentTime > (float) pc.current_time) // next loop starts
			newLoop();

		currentTime = (float) pc.current_time;

		Section sec = sections [currentSectionIndex];
		while (!sec.thatsMe (currentTime))
			sec = sections [++ currentSectionIndex];

		if(sec.getIndexIncrementSignal ()) {
			current_fpc = focusPointCommands [currentSectionStartWaypointIndex ++];
			if (current_fpc.doChangeFocus ())
				focusPoint.position = current_fpc.getFocusPoint ();
		}
			
		if(sec.isDecel () && current_fpc.doChangeFocus ()) {	
			float perc = sec.getProcProgressWithinSection (currentTime);
			FocusPointCommand next_fpc = focusPointCommands[currentSectionStartWaypointIndex];
			focusPoint.position = Vector3.Lerp (current_fpc.getFocusPoint (), next_fpc.getFocusPoint (), perc);
		}

		cam.position = sec.getCoordAtT (currentTime); // Debug.Log (currentTime + ": " + newPos);
		cam.LookAt (focusPoint);
	}
}
