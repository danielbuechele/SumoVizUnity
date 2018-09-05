using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	/*[Header("load trajectories at editing time:")] // false means to load them at runtime
	[SerializeField]
	public bool trajAtEditingTime = true;*/
	[HideInInspector]
	public GeometryLoader geometryLoader;
	public string trajFilePath; // TODO more floors

	public List<float> boundingPoints;

	/*void Start() {
		if (GameObject.Find ("FileChooser") == null) {
			doStart ();
		}
	}*/
		
	void Start () { // = Play button was pressed in unity
		if (trajFilePath != "no_path_to_trajectory_file") {
			TrajectoryLoader tl = new TrajectoryLoader ();
			tl.loadTrajectories (trajFilePath);
		} else {
			Debug.LogError ("no path to trajectory file -> set one in RuntimeInitializer relative to the StreamingAssets directory");
		}
	}

	/*public void doStart() {
		TrajectoryLoader tl = new TrajectoryLoader (absoluteTrajFilePath);
		tl.loadTrajectories ();
	}*/
}
