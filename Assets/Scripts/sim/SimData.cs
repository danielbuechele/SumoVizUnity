using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;


public class SimData {

    private Dictionary<string, Floor> floorsMap;
    private Dictionary<string, WunderZone> wunderZoneMap;
    public float minElev;
    public float maxElev;
    public float minX;
    public float minY;
    decimal maxTime;
    GameObject peds;


    public SimData(){
        floorsMap = new Dictionary<string, Floor>();
        wunderZoneMap = new Dictionary<string, WunderZone>();
        minElev = float.PositiveInfinity;
        maxElev = float.NegativeInfinity;
        minX = float.PositiveInfinity;
        minY = float.PositiveInfinity;
        maxTime = 0;
    }

    internal void addFloor(Floor floor) {
        floorsMap[floor.floorId] = floor;
        if (minElev > floor.elevation)
            minElev = floor.elevation;

        if (maxElev < floor.elevation)
            maxElev = floor.elevation;

    }

    internal List<Floor> getFloors() {
        return floorsMap.Values.ToList<Floor>();
    }

    internal List<String> getFloorIDs() {
        return new List<String>(floorsMap.Keys);
    }

    internal Dictionary<string, Floor> getFloorMap() {
        return floorsMap;
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

    internal void setBounds() {
        foreach (Floor floor in floorsMap.Values) {
            if (minX > floor.boundingPoints[0])
                minX = floor.boundingPoints[0];

            if (minY > floor.boundingPoints[1])
                minY = floor.boundingPoints[1];
        }
    }

    internal decimal getMaxTime() {
        return maxTime;
    }

    internal void setMaxTime(decimal maxTime) {
        this.maxTime = maxTime;
    }

    internal void setPedestrianGameObject(GameObject peds) {
        this.peds = peds;
    }

    internal GameObject getPedestrianGameObject() {
        return peds;
    }
}
