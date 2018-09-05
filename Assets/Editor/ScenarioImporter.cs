using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public class ScenarioImporter {

	[MenuItem("Assets/Import crowd:it project")]
	
	static void importAccurateOutput() {
		if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ())
			return;

		bool continueOk = true;

		if (EditorSceneManager.GetActiveScene().name == "Base") 
			continueOk = !EditorUtility.DisplayDialog("duplicate scene", "It is recommend that you first duplicate the Scene (select it in the Scenes folder and use Edit > Duplicate), rename it optionally and doubleclick the new scene.", "ok, let me duplicate", "continue");

		string scenariosPath = Application.dataPath + "/StreamingAssets/Scenarios";

		if (continueOk) {
			var crowditFilePath = EditorUtility.OpenFilePanel ("", scenariosPath, "crowdit;*.crowdit"); //Path.GetFileName(path))

			if (crowditFilePath == "") // = cancel was clicked in open file dialog
				return;

			new GameObject("World");
			new GameObject ("Pedestrians");

			RuntimeInitializer ri = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
			ri.geometryLoader = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
			ri.geometryLoader.setTheme (new MarketplaceThemingMode ()); // EvaktischThemingMode ());

            string resFolderPath = Path.Combine(Path.GetDirectoryName(crowditFilePath), Path.GetFileNameWithoutExtension(crowditFilePath)) + "_res";

            ScenarioLoader sl = new ScenarioLoader ();
            sl.loadScenario (crowditFilePath, resFolderPath);

			ri.boundingPoints = sl.getBoundingPoints ();
            ri.setCrowditFileAndResFolder(crowditFilePath, resFolderPath);

            //string projectFolderName = path.Substring (scenariosPath.Length, path.Length - scenariosPath.Length - Path.GetFileName (path).Length); // TODO make this more solid
            //ri.relativeTrajFilePath = sl.getRelativeTrajFilePath () != "" ? "Assets/StreamingAssets/Scenarios" + projectFolderName + sl.getRelativeTrajFilePath () : "no_path_to_trajectory_file";
            //ri.absoluteTrajFilePath = Path.GetDirectoryName (path) + "/" + sl.getRelativeTrajFilePath ();
        }
    }

	[MenuItem("Assets/dev")]

	static void dev() {
		/* this is the name the app gets on an android smartphone */
		//PlayerSettings.productName = "app_name";
		//Debug.Log (PlayerSettings.productName);

		/* to undo the texture-stretching on a cube that stretching the cube caused */
		//ReCalcCubeTexture script = GameObject.Find("Cube").GetComponent<ReCalcCubeTexture>();
		//script.reCalcCubeTexture ();

		/*
		string dir = "Assets/Resources/Data/_ignore/.../";
		FileInfo fi = new FileInfo (dir + "floor-0.csv");
		StreamReader reader = fi.OpenText ();
		StreamWriter writer = new StreamWriter(dir + "floor-0_reduced.csv", false);
		using (reader) {
			string line;
			int i = 0;
			while((line = reader.ReadLine()) != null && i++ < 100000) {
				writer.WriteLine (line); 
			}
			reader.Close ();
			writer.Close();
		}
		Debug.Log ("DONE");
		*/

        /*
		var absoluteFfmpegExeLoc = ""; // C:\\Program Files\\ffmpeg\\bin\\ffmpeg.exe";

		bool continueOk = true;

		if (absoluteFfmpegExeLoc == "") {
			if (EditorUtility.DisplayDialog("No ffmpeg.exe found", "Please specify the location of an ffmpeg.exe.", "Ok", "Cancel"))
				absoluteFfmpegExeLoc = EditorUtility.OpenFilePanel ("", "", "exe");
		}
			
		if (continueOk && absoluteFfmpegExeLoc != "") {
			var relativeScreenshotFileGenericLoc = "Screenshots\\screenshot%d.png";
			var relativeOutFileLoc = "out.mp4";
			var ffmpegCommand = "-i " + relativeScreenshotFileGenericLoc + " -vf scale=trunc(iw/2)*2:trunc(ih/2)*2 -r 25 -c:v libx264 -pix_fmt yuv420p -crf 18 " + relativeOutFileLoc; // keep same resolution code from http://stackoverflow.com/a/20848224

			System.Diagnostics.Process.Start (absoluteFfmpegExeLoc, ffmpegCommand);
		}
        */
	}
		

}
