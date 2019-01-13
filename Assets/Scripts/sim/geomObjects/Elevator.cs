using System;
using UnityEngine;
using System.Collections.Generic;

public class Elevator : WunderZone {

    internal List<Floor> floors = new List<Floor>();

    public override void createObject(GameObject parent, SimData simData, GeometryLoader gl) {

        /*       float height = floor.height;
                if (morphosisEntry.HasAttribute("connectsTo"))
                {
                    string connectsTo = morphosisEntry.GetAttribute("connectsTo");
                    Floor connectsToFloor = floor.simData.getFloor(connectsTo);
                    height = connectsToFloor.elevation;

                }

                StairExtrudeGeometry.create(this.id + "-Stair", this.points, height, floor.elevation, dirVect, noOfTreads);*/
        float height = 0;
        float minElev = float.PositiveInfinity;
        float maxElev = float.NegativeInfinity;

        foreach (Floor floorstops in floors) {
            if (minElev > floorstops.elevation)
                minElev = floorstops.elevation;

            if (maxElev < floorstops.elevation)
                maxElev = floorstops.elevation;
        }
        height = Math.Abs(maxElev - minElev);
        GameObject elevator = ObstacleExtrudeGeometry.create(this.id + "-elevator", this.points, height, minElev, parent,
            gl.getTheme().getElevatorMaterial(), gl.getTheme().getElevatorMaterial(), gl.getObstaclePrefab());

        foreach (Renderer renderer in elevator.GetComponentsInChildren<Renderer>()) {
            renderer.sharedMaterial = new Material(Shader.Find("UI/Lit/Transparent"));
            // greeenish
            renderer.sharedMaterial.color = new Color(0.43f, 0.98f, 0.39f, .3f);
        }
    }

    internal void addFloors(List<Floor> floors) {
        this.floors = floors;
    }

    internal void addFloor(Floor floor) {
        floors.Add(floor);
    }
}
