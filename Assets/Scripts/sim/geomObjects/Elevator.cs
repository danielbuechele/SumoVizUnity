using System;
using UnityEngine;
using System.Collections.Generic;

public class Elevator : WunderZone {

    internal List<Floor> floors = new List<Floor>();


    public override void createObject(GameObject parent)
    {

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

        foreach (Floor floorstops in floors)
        {
            if (minElev > floorstops.elevation)
                minElev = floorstops.elevation;

            if (maxElev < floorstops.elevation)
                maxElev = floorstops.elevation;
        }
        height = Math.Abs(maxElev - minElev);
        ObstacleExtrudeGeometry.create(this.id + "-elevator", this.points, height, minElev, parent);

    }

    internal void addFloors(List<Floor> floors)
    {
        this.floors = floors;
    }

    internal void addFloor(Floor floor)
    {
        floors.Add(floor);
    }
}
