using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using SFB;


// inspired from: https://answers.unity.com/questions/585314/record-camera-and-play-again.html

public class CameraPositionRecorder : MonoBehaviour {

    [System.Serializable]
    //simple class for values we're going to record
    public class GoVals {
        public Vector3 position;
        public Quaternion rotation;
        public float frame;

        //constructor
        public GoVals(Vector3 position, Quaternion rotation, float frame) {
            this.position = position;
            this.rotation = rotation;
            this.frame = frame;
        }

        //constructor
        public GoVals(Vector3 position, Quaternion rotation) {
            this.position = position;
            this.rotation = rotation;
            this.frame = -1;
        }

    }

    //a list of recorded values
    List<GoVals> vals = new List<GoVals>();

    //...are we replaying?
    bool replaying = false;

    [SerializeField] Button addCameraPosition;
    [SerializeField] Button replay;
    [SerializeField] Button loadCameraPosition;
    [SerializeField] Button saveCameraPosition;
    [SerializeField] Button resetPositions;
    private PedestrianMover pm;
    private GoVals currentPoint;
    private int currentIndex;
    private StreamWriter writer;

    //cache of our transform
    Transform tf;

    void Start() {
        //cache it...
        tf = this.transform;
        addCameraPosition.onClick.AddListener(delegate () {
            this.addPosition();
        });
        replay.onClick.AddListener(delegate () {
            this.replayCamera();
        });
        loadCameraPosition.onClick.AddListener(delegate () {
            this.loadCameraPositions();
        });
        saveCameraPosition.onClick.AddListener(delegate () {
            this.saveCameraPositions();
        });

        resetPositions.onClick.AddListener(delegate () {
            this.Reset();
        });
        pm = FindObjectOfType<PedestrianMover>();
    }

    private void saveCameraPositions() {
        String savedPositions = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "txt"); //Path.GetFileName(path))
        if (savedPositions == "") // = cancel was clicked in open file dialog
            return;
        writer = new StreamWriter(savedPositions);
        writer.AutoFlush = true;
        foreach  (GoVals val in  vals) {
            writer.WriteLine(val.position.x + ";" + val.position.y + ";" + val.position.z + ";" + val.rotation.x + ";" + val.rotation.y + ";" + val.rotation.z + ";" + val.rotation.w + ";" + val.frame.ToString());

        }
        writer.Close();
    }

    private void loadCameraPositions() {
        String[] savedPositions = StandaloneFileBrowser.OpenFilePanel("", "", "txt;*.txt", false); 
        if (savedPositions == null) // = cancel was clicked in open file dialog
            return;
        String savedPositionFile = savedPositions[0];
         StreamReader file =
          new StreamReader(savedPositionFile);
        string line;
        while ((line = file.ReadLine()) != null) {
            string[] values = line.Split(';');
            if (values.Length >= 4) {
                Vector3 position;
                Quaternion rotation;
                float currentTime;
                int id;
                float x, y, z,r, t;
                float.TryParse(values[0], out x);
                float.TryParse(values[1], out y);
                float.TryParse(values[2], out z);
                position = new Vector3(x, y, z);

                float.TryParse(values[3], out x);
                float.TryParse(values[4], out y);
                float.TryParse(values[5], out z);
                float.TryParse(values[6], out r);
                rotation = new Quaternion(x, y, z, r);

                float.TryParse(values[7], out currentTime);

                vals.Add(new GoVals(position, rotation, currentTime));
                if (currentPoint == null) {
                    currentPoint = vals[0];
                    currentIndex = 0;
                }
            }
        }
    }

    private void addPosition() {
        vals.Add(new GoVals(tf.position, tf.rotation, pm.getCurrentTime()));
        if (currentPoint == null) {
            currentPoint = vals[0];
            currentIndex = 0;
        }
    }

    void Update() {
        ReplayPoints();
    }

    void ReplayPoints() {
        if (!replaying) return;

        //if no further camera points are stored, stay at this position and stop replaying
        if (pm.getCurrentTime() >= vals[vals.Count - 1].frame) {
//            replayCamera();
            currentIndex = 0;
            currentPoint = vals[currentIndex];
            return;
        } else if (pm.getCurrentTime() >= currentPoint.frame) {
            //set our transform values
            tf.position = currentPoint.position;
            tf.rotation = currentPoint.rotation;
            currentIndex = currentIndex + 1;
            currentPoint = vals[currentIndex];
        } else if (currentIndex > 0) {
            float timeBetweenPts = vals[currentIndex].frame - vals[currentIndex - 1].frame;
            float ratio = (pm.getCurrentTime() - vals[currentIndex - 1].frame) / timeBetweenPts;
            tf.position = Vector3.Lerp(vals[currentIndex - 1].position, vals[currentIndex].position, ratio);
            tf.rotation = Quaternion.Lerp(vals[currentIndex - 1].rotation, vals[currentIndex].rotation, ratio);
        }
    }


    void replayCamera() {
        replaying = !replaying;

        if (replaying) {
            replay.GetComponentInChildren<Text>().text = "Stop Replay";
            addCameraPosition.enabled = false;
            saveCameraPosition.enabled = false;
            loadCameraPosition.enabled = false;
            resetPositions.enabled = false;
        } else {
            replay.GetComponentInChildren<Text>().text = "Replay";
            addCameraPosition.enabled = true;
            saveCameraPosition.enabled = true;
            loadCameraPosition.enabled = true;
            resetPositions.enabled = true;
        }
    }

    public void Reset() {
        replaying = false;
        vals = new List<GoVals>();
        currentPoint = null;
    }
}

