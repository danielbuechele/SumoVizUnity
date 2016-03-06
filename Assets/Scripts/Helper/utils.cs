using UnityEngine;
using System.Collections;

public class utils {


	public static string loadFileIntoEditor(string filepath) {
		string filedata = "";
		if (!System.IO.File.Exists (filepath)) {
			string msg = "Error: file " + filepath + " not found";
			Debug.LogError (msg);
			DebugConsole.Log (msg);
		}
		else
			filedata = System.IO.File.ReadAllText (filepath);
		return filedata;
	}

	public static string loadFileAtRuntimeIntoBuild(string filepath) {
		string filedata = "";

		//TODO

		return filedata;	
	}

}
