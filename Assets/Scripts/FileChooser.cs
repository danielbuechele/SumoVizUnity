using UnityEngine;
using System.Collections;

public class FileChooser : MonoBehaviour {

	FileBrowser fb = new FileBrowser();

	void Start () {
		//fb.extensionsAllowed = new[] {".crowdit", ".xml"};
	}

	void OnGUI(){	
		if (fb.draw()) {
			if (fb.outputFile == null){
				Debug.Log("Choosing file was cancelled");
				Application.Quit();
			} else {
				Debug.Log("Output File = \"" + fb.outputFile.ToString() + "\"");

				//fl.loadXMLFile(fb.outputFile.FullName);

				Destroy (this);
			}
		}
	}
	
	void Update () {}
}
