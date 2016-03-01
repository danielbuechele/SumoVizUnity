using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	[Header("load trajectories at editing time:")] // false means to load them at runtime
	[SerializeField]
	public bool trajAtEditingTime = true;
	[HideInInspector]
	public GeometryLoader geometryLoader;
	[HideInInspector]
	public List<string> trajectoryLines; 

	void Start () {
		//FileLoaderXML runtimeFileLoader = new FileLoaderXML();
		//runtimeFileLoader.loadTrajectories (trajectoryLines);
	}

}
