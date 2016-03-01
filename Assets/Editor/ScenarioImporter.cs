using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Collections;

public class ScenarioImporter : MonoBehaviour {

	[MenuItem("Assets/Import accu:rate output")]
	
	static void importAccurateOutput() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();

		var continueOk = true;

		if (EditorSceneManager.GetActiveScene().name == "Base") 
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the new scene.", "ok, let me duplicate", "continue");

		if (continueOk) {
			var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/Data", "xml"); //Path.GetFileName(path))

			new GameObject("World");
			new GameObject ("Pedestrians");

			RuntimeInitializer runtimeInitializer = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
			runtimeInitializer.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			runtimeInitializer.geometryLoader.setTheme (new NatureThemingMode ());

			FileLoaderXML fl = new FileLoaderXML ();
			fl.loadXMLFile (path);
			//fl.buildGeometry();

			//if(runtimeInitializer.trajAtEditingTime)
				//runtimeInitializer.trajectoryLines = fileLoader.loadTrajectoryLines();
		}
	}

	[MenuItem("Assets/change product name")]

	static void changeProductName() {
		PlayerSettings.productName = "app_name"; //TODO
		Debug.Log (PlayerSettings.productName);
	}

}
