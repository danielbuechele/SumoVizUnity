using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class FileChooser : MonoBehaviour {

	FileBrowser fb = new FileBrowser();

	void Start () {
		fb.extensionsAllowed = new[] {".crowdit", ".xml"};
	}

	void OnGUI(){	
		if (fb.draw()) {
			if (fb.outputFile == null){
				Debug.Log("Choosing file was cancelled");
				Application.Quit();
			} else {
				Debug.Log("Loading file " + fb.outputFile.ToString());

				var path = fb.outputFile.FullName;

				new GameObject("World");
				new GameObject ("Pedestrians");

				RuntimeInitializer ri = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
				ri.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
				ri.geometryLoader.setTheme (new DefaultThemingMode ());

				ScenarioLoader sl = new ScenarioLoader (path);
				sl.loadScenario ();

				ri.boundingPoints = sl.getBoundingPoints ();

				string csvAt = sl.getRelativeTrajFilePath ();
				if (csvAt != "") {
					ri.relativeTrajFilePath = path.Substring (0, path.Length - path.Split (Path.DirectorySeparatorChar).Last ().Length) + csvAt;
					ri.doStart ();
				} else {
					Debug.Log ("No trajectories file found");
				}

				Destroy (this);
			}
		}
	}
	
	void Update () {}
}
