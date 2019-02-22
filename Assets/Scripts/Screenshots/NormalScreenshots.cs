using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using SFB;
using System;

public class NormalScreenshots : MonoBehaviour {

    public int superSizeFactor = 1;
    public int fps = 25;

    private PlaybackControl pc;
    private PedestrianMover pm;
    private int count = 0;
    private bool render = false;

    // set via inspector
    [SerializeField] Button record;
    [SerializeField] Sprite RecordStartSprite;
    [SerializeField] Sprite RecordingSprite;


    void Start() {
        pm = FindObjectOfType<PedestrianMover>();
        Time.captureFramerate = fps;
        Application.runInBackground = true;
        record.onClick.AddListener(delegate () { renderScreen(); });
    }

    private void renderScreen() {
        // if already rendering, do not ask for file
        if (!render) {
            String outFile = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "mp4"); //Path.GetFileName(path))
            if (outFile == "") // = cancel was clicked in open file dialog
                return;
            Screenrecorder.init(outFile);
            record.image.sprite = RecordingSprite;
        } else {
            record.image.sprite = RecordStartSprite;
        }
        render = !render;
    }

    void OnPostRender() {
        if (render && pm.isFirstRound()) {
            //Application.CaptureScreenshot ("Screenshots/screenshot" + (count ++) + ".png", superSizeFactor);

            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); // via http://answers.unity3d.com/answers/1190178/view.html
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToJPG();
            //File.WriteAllBytes("Screenshots/screenshot" + (count ++) + ".jpg", bytes);

            Screenrecorder.writeImg(bytes);
            UnityEngine.Object.Destroy(screenshot);

            count++;

//            Debug.Log("frame #" + count + " captured");
        } else {
            if (!Screenrecorder.isClosed) {
                Debug.Log("video exported with " + count + " frames");
                Screenrecorder.close();
                record.image.sprite = RecordStartSprite;
                render = false;
            }
        }
    }

}
