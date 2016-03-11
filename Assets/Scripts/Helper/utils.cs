using UnityEngine;
using System.Collections;

public class utils {


	public static string getStreamingAssetsPath(string relativePath) {
		string realPath = "";
		if (Application.platform == RuntimePlatform.Android) { // https://gist.github.com/amowu/8121334
			string oriPath = System.IO.Path.Combine (Application.streamingAssetsPath, relativePath);
			WWW wwwreader = new WWW (oriPath);
			while (!wwwreader.isDone) {}
			realPath = Application.persistentDataPath + "/db";
			System.IO.File.WriteAllBytes (realPath, wwwreader.bytes);
		} 
		else
			realPath = Application.streamingAssetsPath + "/" + relativePath;
		return realPath;
	}

	public static string loadFileIntoEditor(string filepath) {
		if (!System.IO.File.Exists (filepath)) {
			string msg = "Error: file " + filepath + " not found";
			Debug.LogError (msg);
			DebugConsole.Log (msg);
			return "";
		}
		return System.IO.File.ReadAllText (filepath);
	}

	public static string loadFileAtRuntimeIntoBuild(string filepath) {
		if (filepath.Substring (filepath.Length - ".csv".Length, ".csv".Length) == ".csv") // TODO thats not very safe
			filepath = filepath.Substring (0, filepath.Length - ".csv".Length);
		var textAsset = Resources.Load (filepath) as TextAsset; // TODO find file.exists equivalent
		return textAsset.text;
	}

}
