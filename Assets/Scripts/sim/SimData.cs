using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class SimData {

    private Dictionary<string, Floor> floorsMap = new Dictionary<string, Floor>();
    private Dictionary<string, WunderZone> wunderZoneMap = new Dictionary<string, WunderZone>();
    public float minElev = float.PositiveInfinity; 
    public float maxElev = float.NegativeInfinity;
    public float minX = float.PositiveInfinity;
    public float minY = float.PositiveInfinity;

    internal void addFloor(Floor floor) {
        floorsMap[floor.floorId] = floor;
        if (minElev > floor.elevation)
            minElev = floor.elevation;

        if (maxElev < floor.elevation)
            maxElev = floor.elevation;

    }

    internal Floor getFloor(string floorId) {
        return floorsMap[floorId];
    }

    internal void addWunderZoneToMap(string wzId, WunderZone wz) {
        if (wunderZoneMap.ContainsKey(wzId)) {
            throw new Exception("WunderZone IDs must be unique. There are at least two with the ID: " + wzId);
        }
        wunderZoneMap[wzId] = wz;
    }

    internal void printFloors() {
        //foreach (var floor in floors) {
        //    floor.printGeometryElements();
        //}
    }

    internal void setBounds()
    {
        foreach (Floor floor in floorsMap.Values)
        {
            if (minX > floor.boundingPoints[0])
                minX = floor.boundingPoints[0];

            if (minY > floor.boundingPoints[1])
                minY = floor.boundingPoints[1];
        }
    }
}
