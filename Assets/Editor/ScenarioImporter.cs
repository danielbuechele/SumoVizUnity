using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;
using System.Collections;

public class ScenarioImporter {

	[MenuItem("Assets/Import accu:rate output")]
	
	static void importAccurateOutput() {
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
		var continueOk = true;

		if (EditorSceneManager.GetActiveScene().name == "Base") 
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the new scene.", "ok, let me duplicate", "continue");

		if (continueOk) {
			var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/Data", "xml"); //Path.GetFileName(path))

			if (path == "") // = cancel was clicked in open file dialog
				return;

			new GameObject("World");
			new GameObject ("Pedestrians");

			RuntimeInitializer ri = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
			ri.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			ri.geometryLoader.setTheme (new EvaktischThemingMode ());

			ScenarioLoader sl = new ScenarioLoader (path);
			sl.loadScenario ();
			ri.trajectoryFilePath = sl.getTrajectoryFilePath ();
		}
	}

	[MenuItem("Assets/change product name")]

	static void changeProductName() {
		PlayerSettings.productName = "app_name"; //TODO
		Debug.Log (PlayerSettings.productName);
	}

	[MenuItem("Assets/dev")]

	static void dev() {
		//...
	}

}
