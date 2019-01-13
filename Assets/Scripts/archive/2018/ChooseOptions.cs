using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseOptions : MonoBehaviour {
    Dropdown m_Dropdown;
    private List<int> hiddenFloorLevels;
    private SimData simData;

    public ChooseOptions() { }

    void Start() {
        m_Dropdown = GetComponent<Dropdown>();
        m_Dropdown.ClearOptions();
        m_Dropdown.onValueChanged.AddListener(delegate {
            this.DropdownValueChanged(m_Dropdown);
        });
    }

    public void setOptions(SimData simData) {
        this.simData = simData;
        hiddenFloorLevels = new List<int>();
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();

        //Add the options created in the List above
        m_Dropdown.AddOptions(simData.getFloorIDs());
    }

    public void Reset() {
        simData = null;
        hiddenFloorLevels = null;
    }


    //Ouput the new value of the Dropdown into Text
    public void DropdownValueChanged(Dropdown change) {
        Dropdown m_Dropdown = GameObject.Find("FloorChooser").GetComponent<Dropdown>();
        String floorName = m_Dropdown.options[m_Dropdown.value].text;
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

    public void Update() {
        //if (simData == null)
        //    return;
        //try {
        //    foreach (Pedestrian ped in simData.getPedestrianGameObject().GetComponentsInChildren<Pedestrian>()) {
        //        if (hiddenFloorLevels.Contains(ped.getCurrentFloorID())) {
        //            foreach (Renderer r in ped.GetComponentsInChildren<Renderer>()) {
        //                r.enabled = false;
        //            }
        //        } else {
        //            if (!ped.reachedTarget()) {
        //                foreach (Renderer r in ped.GetComponentsInChildren<Renderer>()) {
        //                    r.enabled = true;
        //                }
        //            }
        //        }
        //    }
        //} catch (NullReferenceException) {

        //}

    }
}
