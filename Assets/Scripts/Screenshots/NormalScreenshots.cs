using UnityEngine;
using System.Collections;

public class NormalScreenshots : MonoBehaviour {

	public int superSizeFactor = 1;
	public int fps = 25;

	private PlaybackControl pc;
	private int count = 0;


	void Start () {
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
		Time.captureFramerate = fps;
	}
	
	void Update () {
		if (pc.inFirstRound ()) {
			Application.CaptureScreenshot ("screenshot" + (count ++) + ".png", superSizeFactor);
			Debug.Log ("screenshot #" + count + " taken");
		}
	}

}
