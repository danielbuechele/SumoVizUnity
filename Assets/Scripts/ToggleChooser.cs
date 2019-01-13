using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChooser : MonoBehaviour {
    [SerializeField] Toggle togglePrefab;
    [SerializeField] GameObject toggleFrame
        ;
    private SimData simData;
    private List<int> hiddenFloorLevels;


    public void setToggles(SimData simData) {
        this.simData = simData;
        hiddenFloorLevels = new List<int>();

        //Clear the old options of the Dropdown menu
        int yOffset = 0;
        foreach (String floorID in simData.getFloorIDs()) {
            Vector3 pos = new Vector3(togglePrefab.transform.position.x, togglePrefab.transform.position.y - yOffset, togglePrefab.transform.position.z);
            Toggle newToggle = Instantiate(togglePrefab);
            newToggle.GetComponentInChildren<Text>().text = floorID;
            newToggle.transform.parent = toggleFrame.transform;
            newToggle.onValueChanged.AddListener(delegate {
                this.onUpdate(newToggle);
            });
        }

    }

    public void Reset() {
        simData = null;
        hiddenFloorLevels = null;
    }

    void onUpdate(Toggle toggle) {
        String floorName = toggle.GetComponentInChildren<Text>().text;
        String strinToSearch = floorName + "_ground";

        Floor floor = simData.getFloor(floorName);

        GameObject floorGO = GameObject.Find(strinToSearch);
        if (floorGO.GetComponent<Renderer>().enabled == true) {
            hiddenFloorLevels.Add(floor.level);
            floorGO.GetComponent<Renderer>().enabled = false;
            foreach (Renderer r in floorGO.GetComponentsInChildren<Renderer>()) {
                r.enabled = false;
            }
        } else {
            hiddenFloorLevels.Remove(floor.level);
            floorGO.GetComponent<Renderer>().enabled = true;
            foreach (Renderer r in floorGO.GetComponentsInChildren<Renderer>())
                r.enabled = true;
        }

    }
    // Update is called once per frame
    void Update () {
        if (simData == null)
            return;
        try {
            foreach (Pedestrian ped in simData.getPedestrianGameObject().GetComponentsInChildren<Pedestrian>()) {
                if (hiddenFloorLevels.Contains(ped.getCurrentFloorID())) {
                    foreach (Renderer r in ped.GetComponentsInChildren<Renderer>()) {
                        r.enabled = false;
                    }
                } else {
                    if (!ped.reachedTarget()) {
                        foreach (Renderer r in ped.GetComponentsInChildren<Renderer>()) {
                            r.enabled = true;
                        }
                    }
                }
            }
        } catch (NullReferenceException) {

        }
    }

    internal GameObject getCanvas() {
        return toggleFrame;
    }
}
