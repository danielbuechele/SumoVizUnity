using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


public class CameraTour : MonoBehaviour {

	//private GameObject cameraObj;
	private PlaybackControl pc;
	private float currentTime = 0;

	private List<Waypoint> waypoints = new List<Waypoint> ();
	float s_ges = 0;

	private bool firstUpdateDone = false;

	private List<Section> sections = new List<Section>();
	private int currentSectionIndex = 0;

	private float t_ges;
	private float t_waitSum; // sum of all waiting times in waypoints

	private float accelEndMarkerPerc = 0.2f; // 1/5
	private float decelStartMarkerPerc = 0.8f; // 4/5

	private string waypointsFilepath;


	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		//cameraObj = GameObject.Find ("Sphere");

		waypointsFilepath = Application.dataPath + "/Data/waypoints/waypointsEvaktischBig2.csv";
		importWaypoints ();

		if (waypoints[0].doWait()) // extra check this, because i starts at 1 in following for-loop
			addWaitSection (waypoints[0]);

		for (int i = 1; i < waypoints.Count; i ++) {
			Vector3 startWaypoint = waypoints[i - 1].getPoint();
			float velocReducerStart = waypoints[i - 1].getVelocReducer();
			Vector3 endWaypoint = waypoints[i].getPoint();
			float velocReducerEnd = waypoints[i].getVelocReducer();

			float dist = Vector3.Distance (startWaypoint, endWaypoint);
			//Debug.Log ("dist: " + dist);

			float s_accel = dist * accelEndMarkerPerc;
			float s_decel = dist - (dist * decelStartMarkerPerc);
			float s_const = dist - s_accel - s_decel;
			//Debug.Log (string.Concat("s_accel: ", s_accel, " s_const: ", s_const, " s_decel: ", s_decel));

			Vector3 accelEndPoint = Vector3.Lerp (startWaypoint, endWaypoint, accelEndMarkerPerc);
			Vector3 decelStartPoint = Vector3.Lerp (startWaypoint, endWaypoint, decelStartMarkerPerc);

			Section accelSect = new Section(sections.Count, Section.Type.ACCELERATION, startWaypoint, accelEndPoint, velocReducerStart, s_ges, s_accel);
			//Debug.Log ("ACCEL-SECT: " + accelSect);
			sections.Add(accelSect);
			Section constSect = new Section(sections.Count, Section.Type.CONSTANT, accelEndPoint, decelStartPoint, 1, s_ges + s_accel, s_const);
			//Debug.Log ("CONST-SECT: " + constSect);
			sections.Add(constSect);
			Section decelSect = new Section(sections.Count, Section.Type.DECELERATION, decelStartPoint, endWaypoint, velocReducerEnd, s_ges + s_accel + s_const, s_decel);
			//Debug.Log ("DECEL-SECT: " + decelSect);
			sections.Add(decelSect);

			if (waypoints[i].doWait())
				addWaitSection (waypoints[i]);

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
			string line = reader.ReadLine(); // skip the first line = header
			while((line = reader.ReadLine()) != null) {
				if (line.Length > 0) {
					if (line.Substring (0, 1) != "#") {
						string[] values = line.Split(',');
						float x;
						float y;
						float z;
						float velocReducer;
						float waitingTime;
						bool doFocus;
						float.TryParse(values[0], out x);
						float.TryParse(values[1], out y);
						float.TryParse(values[2], out z);
						float.TryParse(values[3], out velocReducer);
						float.TryParse(values[4], out waitingTime);
						bool.TryParse (values [5], out doFocus);

						Waypoint wp = new Waypoint (waypoints.Count, new Vector3 (x, y, z), velocReducer, waitingTime, doFocus);

						if (doFocus) {
							float.TryParse(values[6], out x);
							float.TryParse(values[7], out y);
							float.TryParse(values[8], out z);
							wp.setFocusPoint (new Vector3 (x, y, z));
						}

						waypoints.Add (wp);
					}
				}
			}
		}
	}

	private void addWaitSection(Waypoint wp) {
		Section waitSect = new Section(sections.Count, Section.Type.WAIT, wp.getPoint(), wp.getPoint(), 0, 0, 0);
		waitSect.setTinSection(wp.getWaitingTime());
		sections.Add (waitSect);
		t_waitSum += wp.getWaitingTime();
	}

	/*
	private void addWaypoint(float x, float y, float z, float timeVal){
		waypoints.Add (new Vector3 (x, y, z));
		times.Add (timeVal); // below 0 means waiting time
	}*/

	private void onFirstUpdate(){// because pc.total_time is not know when Start() is executed
		firstUpdateDone = true;

		t_ges = (float) pc.total_time - t_waitSum;
		//Debug.Log (pc.total_time);

		if (t_ges <= 0)
			throw new Exception ("The sum of waiting times in camera tour waypoints is bigger than the total simulation time.");

		float v_max = 0;
		foreach (var section in sections)
			v_max += section.getFormulaContrib();		
		v_max /= t_ges;
		//Debug.Log ("v_max: " + v_max);

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

	void Update () {
		if (!firstUpdateDone)
			onFirstUpdate ();

		if (currentTime > (float) pc.current_time) // next loop starts
			currentSectionIndex = 0;

		currentTime = (float) pc.current_time;

		Section sec = sections [currentSectionIndex];
		while (!sec.thatsMe(currentTime))
			sec = sections [++ currentSectionIndex];

		Vector3 newPos = sec.getCoordAtT (currentTime);
		//Debug.Log (currentTime + ": " + newPos);

		//cameraObj.transform.position = newPos;
		Camera.main.transform.position = newPos;

		Transform focusPoint = GameObject.Find ("FocusPoint").transform;
		Camera.main.transform.LookAt(focusPoint);
	}

}