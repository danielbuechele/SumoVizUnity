using UnityEngine;
using System.Collections.Generic;

public class RuntimeInitializer : MonoBehaviour {

	[HideInInspector]
    public string crowditFilePath;
    public string resFolderPath;

    // TODO only needed from legacy code
    internal List<float> boundingPoints;
    public GeometryLoader geometryLoader;

    void Start()
    { 

    }

    public void setCrowditFileAndResFolder(string crowditFilePath, string resFolderPath) {
        this.crowditFilePath = crowditFilePath;
        this.resFolderPath = resFolderPath;
    }

    public string getResFolderPath()
    {
        return resFolderPath;
    }
}

    /*
    [Header("load trajectories at editing time:")] // false means to load them at runtime
	[SerializeField]
	public bool trajAtEditingTime = true;

    void Start() {
		if (GameObject.Find ("FileChooser") == null) {
			doStart ();
		}
	}
    
    public void doStart() {
		TrajectoryLoader tl = new TrajectoryLoader (absoluteTrajFilePath);
		tl.loadTrajectories ();
	}
    
    if (trajFilePath != "no_path_to_trajectory_file") {
	    TrajectoryLoader tl = new TrajectoryLoader ();
		tl.loadTrajectories (trajFilePath);
	} else {
		Debug.LogError ("no path to trajectory file -> set one in RuntimeInitializer relative to the StreamingAssets directory");
	}
   */
