using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class Floor {

    private List<Wall> walls = new List<Wall>();
    private List<WunderZone> wunderZones = new List<WunderZone>();
    private string floorId;
    private int level;
    private double elevation;
    private double height;

    internal Floor(string floorId, int level) {
        this.floorId = floorId;
        this.level = level;
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

    internal void setFloorElevationAndHeight(double height, double elevation) {
        this.elevation = elevation;
        this.height = height;
    }
}
