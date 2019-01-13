using UnityEngine;

public class Stair : WunderZone {

    public override void createObject(GameObject parent, SimData simData, GeometryLoader gl) {

        Vector3 dirVect = new Vector3(
            float.Parse(morphosisEntry.GetAttribute("dirX")),
            float.Parse(morphosisEntry.GetAttribute("dirZ")),
            float.Parse(morphosisEntry.GetAttribute("dirY")));
        int noOfTreads = int.Parse(morphosisEntry.GetAttribute("noOfTreads"));

        float height = 0;
        if (morphosisEntry.HasAttribute("connectsTo")) {
            string connectsTo = morphosisEntry.GetAttribute("connectsTo");
            Floor connectsToFloor = simData.getFloor(connectsTo);
            height = Mathf.Abs(connectsToFloor.elevation - floor.elevation);
        }

        StairExtrudeGeometry.createStair(this.id + "-Stair", this.points, height, floor.elevation, dirVect, noOfTreads, gl.getTreadPrefab()).transform.SetParent(parent.transform);
    }

}
