using UnityEngine;
using System.Collections.Generic;
using System;

public class Floor {

    private List<Wall> walls = new List<Wall>();
    private List<WunderZone> wunderZones = new List<WunderZone>();
    private string floorId;
    private int level;
    internal float elevation;
    private float height;
    internal List<float> boundingPoints;

    internal Floor(string floorId) {
        this.floorId = floorId;
    }

    internal void addWunderZone(WunderZone wz) {
        wunderZones.Add(wz);
    }

    internal void addWall(Wall wall) {
        walls.Add(wall);
    }

    internal void printGeometryElements() {
        Debug.Log("Floor " + floorId);
        foreach(var wz in wunderZones) {
            Debug.Log("    WunderZone: " + wz.getId());
        }
        foreach (var wall in walls) {
            Debug.Log("    Wall: " + wall.getId());
        }
    }

    internal void setMetaData(int level, float height, float elevation, List<float> boundingPoints) {
        this.level = level;
        this.height = height;
        this.elevation = elevation;
        this.boundingPoints = boundingPoints;
    }

    internal void createObjects() {
        foreach (WunderZone wz in wunderZones) {
            wz.createObject();
        }
        foreach (Wall wall in walls) {
            wall.createObject();
        }
    }
}
