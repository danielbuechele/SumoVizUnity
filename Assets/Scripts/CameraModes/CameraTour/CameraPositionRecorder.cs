using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


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
    private PedestrianMover pm;
    private GoVals currentPoint;
    private int currentIndex;

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
        pm = FindObjectOfType<PedestrianMover>();
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
            replaying = false;
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

        if (replaying)
            replay.GetComponentInChildren<Text>().text = "Stop Replay";
        else
            replay.GetComponentInChildren<Text>().text = "Replay";
    }
}

