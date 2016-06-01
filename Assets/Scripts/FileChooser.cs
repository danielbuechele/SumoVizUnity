using UnityEngine;
using System.Collections;

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
				ri.geometryLoader.setTheme (new MarketplaceThemingMode ());

				ScenarioLoader sl = new ScenarioLoader (path);
				sl.loadScenario ();

				Destroy (this);
			}
		}
	}
	
	void Update () {}
}
