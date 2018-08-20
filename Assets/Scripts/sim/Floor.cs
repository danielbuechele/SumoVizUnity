using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Floor {

    private List<Wall> walls = new List<Wall>();
    private List<WunderZone> wunderZones = new List<WunderZone>();
    private string floorId;

    public Floor(string floorId) {
        this.floorId = floorId;
    }

    public void addWunderZone(WunderZone wz) {
        wunderZones.Add(wz);
    }
}
