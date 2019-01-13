using UnityEngine;
using System;

public class Escalator : WunderZone {

    public override void createObject(GameObject parent, SimData simData, GeometryLoader gl)
    {

        Vector3 dirVect = new Vector3(
            float.Parse(morphosisEntry.GetAttribute("dirX")),
            float.Parse(morphosisEntry.GetAttribute("dirZ")),
            float.Parse(morphosisEntry.GetAttribute("dirY")));
        int noOfTreads = int.Parse(morphosisEntry.GetAttribute("noOfTreads"));
        int noOfHorizontalTreads = int.Parse(morphosisEntry.GetAttribute("noOfHorizontalTreads"));
        string againstDirString = morphosisEntry.GetAttribute("transport");
        bool againstDir = false;
        if (againstDirString.Equals("againstDir"))
        {
            againstDir = true;
        }

        float height = 0;
        float elevation = floor.elevation;
        if (morphosisEntry.HasAttribute("connectsTo"))
        {
            string connectsTo = morphosisEntry.GetAttribute("connectsTo");
            Floor connectsToFloor = simData.getFloor(connectsTo);
            height = Mathf.Abs(connectsToFloor.elevation - floor.elevation);
            if (againstDir)
            {
                elevation = connectsToFloor.elevation;
 //               height = floor.elevation - connectsToFloor.elevation;
            } 
        }
        StairExtrudeGeometry.createEscalator(this.id + "-Stair", this.points, height, elevation, dirVect, 
            noOfTreads, noOfHorizontalTreads, againstDir, gl.getTreadPrefab(), gl.getTheme().getEscalatorTreadMaterial()).transform.SetParent(parent.transform);
    }
}
