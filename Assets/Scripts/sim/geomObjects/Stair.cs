using UnityEngine;

public class Stair : WunderZone {

    public override void createObject() {

        Vector3 dirVect = new Vector3(
            float.Parse(morphosisEntry.GetAttribute("dirX")),
            float.Parse(morphosisEntry.GetAttribute("dirY")),
            float.Parse(morphosisEntry.GetAttribute("dirZ")));
        int noOfTreads = int.Parse(morphosisEntry.GetAttribute("noOfTreads"));

        float height = floor.height;
        if (morphosisEntry.HasAttribute("connectsTo")) {
            string connectsTo = morphosisEntry.GetAttribute("connectsTo");
            Floor connectsToFloor = floor.simData.getFloor(connectsTo);

            // TODO
        }

        StairExtrudeGeometry.create(this.id + "-Stair", this.points, height, floor.elevation, dirVect, noOfTreads);
    }

}
