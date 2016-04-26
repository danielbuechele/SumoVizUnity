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
	public string relativeTrajFilePath; // TODO more floors

	void Start () { // = Play button was pressed in unity
		if (relativeTrajFilePath != "no_path_to_trajectory_file") {
			TrajectoryLoader tl = new TrajectoryLoader (relativeTrajFilePath);
			tl.loadTrajectories ();
		} else {
			Debug.LogError ("no path to trajectory file -> set one in RuntimeInitializer relative to the StreamingAssets directory");
		}
	}
}
