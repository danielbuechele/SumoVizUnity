using UnityEngine;
using System.Collections;

public class utils {


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
		var textAsset = Resources.Load (filepath) as TextAsset; // TODO find file.exists equivalent
		return textAsset.text;
	}

}
