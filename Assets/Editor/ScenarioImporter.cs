using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;

public class ScenarioImporter : MonoBehaviour {

	[MenuItem("Assets/Import accu:rate output")]
	
	static void importAccurateOutput() {
		EditorApplication.SaveCurrentSceneIfUserWantsTo();

		string currentSceneName = Path.GetFileNameWithoutExtension(EditorApplication.currentScene);
		var continueOk = true;
		if (currentSceneName == "Base") 
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the new scene.", "ok, let me duplicate", "continue");

		if (continueOk) {
			RuntimeInitializer runtimeInitializer = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();

			runtimeInitializer.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			runtimeInitializer.geometryLoader.setTheme (new NatureThemingMode ());

			var path = EditorUtility.OpenFilePanel ("", Application.dataPath + "/Data", "xml");

			/*
			FileLoaderXML fl = new FileLoaderXML ();
			fl.loadFileByPath(Path.GetFileName(path));
			fl.buildGeometry();
			runtimeInitializer.trajectoryLines = fileLoader.loadTrajectoryLines();
			*/
		}
	}

}
