using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	/*[Header("load trajectories at editing time:")] // false means to load them at runtime
	[SerializeField]
	public bool trajAtEditingTime = true;*/
	[HideInInspector]
	public GeometryLoader geometryLoader;
	public string trajectoryFilePath; // TODO more floors

	void Start () { // = Play button was pressed in unity
		TrajectoryLoader tl = new TrajectoryLoader (trajectoryFilePath);
		tl.loadTrajectories ();
	}

}
