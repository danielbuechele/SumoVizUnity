using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PedestrianMover : MonoBehaviour {

    bool initialized = false;
    private GameObject peds;
    private ScenarioLoader sl;
    public bool playing = false;
    decimal currentTime;
    int roundCounter = 0;

    public PedestrianMover() { }

    private void Start() {
        sl = GameObject.Find("ScenarioLoader").GetComponent<ScenarioLoader>();
        currentTime = 0;
    }

    internal void init() {
        peds = sl.getSimData().getPedestrianGameObject();
        foreach (Transform ped in peds.transform) {
            ped.GetComponent<Pedestrian>().init();
        }
        initialized = true;
        roundCounter = 0;
        currentTime = 0;
    }

    public void changePlaying() {
        if (playing) {
            playing = false;
//            gameObject.GetComponent<Image>().set 
        }
        else playing = true;
    }

    public void Reset() {
        playing = false;
        initialized = false;
    }

    public void Update() {
        if (playing && initialized) {
            currentTime = (currentTime + (decimal)Time.deltaTime);

            if (currentTime >= sl.getSimData().getMaxTime()) { // new round
                currentTime = 0;

                foreach (Transform ped in peds.transform) {
                    ped.GetComponent<Pedestrian>().reset();
                }
                roundCounter++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            playing = !playing;
        }

        if (initialized) {
            foreach (Transform ped in peds.transform) {
                ped.GetComponent<Pedestrian>().move(currentTime);
            }
        }
    }
}
