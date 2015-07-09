using UnityEngine;
using System.Collections;

public class FileChooser : MonoBehaviour {

	FileBrowser fb = new FileBrowser();
	FileLoaderXML fl = new FileLoaderXML();

	// Use this for initialization
	void Start () {
		fb.searchPattern = "*.xml";
	}

	void OnGUI(){	
		if (fb.draw()) {
			if (fb.outputFile == null){
				Debug.Log("Cancel hit");
				Application.Quit();
			} else {
				Debug.Log("Ouput File = \""+fb.outputFile.ToString()+"\"");
				/*the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"*/
				fl.loadXMLFile(fb.outputFile.FullName);
				Destroy (this);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
