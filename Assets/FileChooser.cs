using UnityEngine;
using System.Collections;

public class FileChooser : MonoBehaviour {

	FileBrowser fb = new FileBrowser();
	
	void OnGUI(){
		fb.searchPattern = "*.xml";

		FileLoaderXML fl = new FileLoaderXML();

		if(fb.draw()){
			if(fb.outputFile == null){
				Debug.Log("Cancel hit");
			}else{
				Debug.Log("Ouput File = \""+fb.outputFile.ToString()+"\"");
				/*the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"*/
				fl.loadXMLFile(fb.outputFile.ToString());
			}
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
