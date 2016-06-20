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
		Application.runInBackground = true;

		Screenrecorder.init ();
	}
	
	void OnPostRender () {
	//void Update() {
		if (pc.inFirstRound ()) {
			//Application.CaptureScreenshot ("Screenshots/screenshot" + (count ++) + ".png", superSizeFactor);

			Texture2D screenshot = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false); // via http://answers.unity3d.com/answers/1190178/view.html
			screenshot.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
			screenshot.Apply ();

			byte[] bytes = screenshot.EncodeToJPG ();
			//File.WriteAllBytes("Screenshots/screenshot" + (count ++) + ".jpg", bytes);

			Screenrecorder.writeImg (bytes);
			Object.Destroy (screenshot);

			Debug.Log ("frame #" + (count ++) + " captured");
		} else {
			if (Screenrecorder.isActive) {
				Debug.Log ("video exported with " + (count - 1) + " frames");
				Screenrecorder.close ();
			}
		}
	}

}
