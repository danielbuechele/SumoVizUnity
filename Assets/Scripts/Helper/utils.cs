using UnityEngine;
using System.Collections;

public class utils {

	public static void crash(string message) {
		UnityEditor.EditorApplication.isPlaying = false; // this works only sometimes for some reason
		throw new UnityException (message);
	}

}
