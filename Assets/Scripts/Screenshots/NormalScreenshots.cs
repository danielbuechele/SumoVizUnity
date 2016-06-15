using UnityEngine;
using System.Collections;
using System.IO;

public class NormalScreenshots : MonoBehaviour {

	public int superSizeFactor = 1;
	public int fps = 25;

	private PlaybackControl pc;
	private int count = 0;



	void Start () {
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
		Time.captureFramerate = fps;


		Screenrecorder.init ();
	}
	
	void OnPostRender () {
		if (pc.inFirstRound ()) {
			//Application.CaptureScreenshot ("Screenshots/screenshot" + (count ++) + ".png", superSizeFactor);

			Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); // via http://answers.unity3d.com/answers/1190178/view.html
			screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			screenshot.Apply();

			byte[] bytes = screenshot.EncodeToPNG();

			//Screenrecorder.writeImg (bytes);

			Object.Destroy(screenshot);

			//File.WriteAllBytes(Application.dataPath + "/test.png", bytes);
			//File.WriteAllBytes("Screenshots/screenshot" + (count ++) + ".png", bytes);

			UnityEngine.Debug.Log ("screenshot #" + count + " taken");
		}
	}

}
