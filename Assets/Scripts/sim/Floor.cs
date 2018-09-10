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

        List<Vector2> plane = new List<Vector2>();
        float minX = boundingPoints[0],
              minY = boundingPoints[1],
              maxX = boundingPoints[2],
              maxY = boundingPoints[3];
        plane.Add(new Vector2(minX, minY));
        plane.Add(new Vector2(minX, maxY));
        plane.Add(new Vector2(maxX, maxY));
        plane.Add(new Vector2(maxX, minY));

        AreaGeometry.createOriginTarget(floorId + "_ground", plane, new Color(1.0f, 1.0f, 1.0f, 0.3f), elevation - 0.01f);

        /*
        float planeHeight = 0.2f;
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "FloorPlane_" + floorId;
        Material floorPlaneMaterial = Resources.Load("Materials/FloorPlaneMaterial.mat", typeof(Material)) as Material;
        cube.GetComponent<Renderer>().sharedMaterial = floorPlaneMaterial;
        //floorPlaneMaterial.shader = Shader.Find("Standard");
        cube.transform.position = new Vector3((minX + maxX) / 2, elevation - planeHeight / 2 - 0.01f, (minY + maxY) / 2);
        cube.transform.localScale = new Vector3(minX + maxX, planeHeight, minY + maxY);
        GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
        gl.setWorldAsParent(cube);
        */

        foreach (WunderZone wz in wunderZones) {
            wz.createObject();
        }
        foreach (Wall wall in walls) {
            wall.createObject();
        }
    }
}
