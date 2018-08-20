using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SimData {

    private List<Floor> floors = new List<Floor>();
    private Dictionary<string, WunderZone> wunderZoneMap = new Dictionary<string, WunderZone>();

    public void addFloor(Floor floor) {
        this.floors.Add(floor);
    }

    public void addWunderZoneToMap(string wzId, WunderZone wz) {
        if (wunderZoneMap.ContainsKey(wzId)) {
            throw new Exception("WunderZone IDs must be unique. There are at least two with the ID: " + wzId);
        }
        wunderZoneMap[wzId] = wz;
    }
}
