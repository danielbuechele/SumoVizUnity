using UnityEngine;

public class Stair : WunderZone {

    public override void createObject() {

        Vector3 dirVect = new Vector3(
            float.Parse(morphosisEntry.GetAttribute("dirX")),
            float.Parse(morphosisEntry.GetAttribute("dirY")),
            float.Parse(morphosisEntry.GetAttribute("dirZ")));
        int noOfTreads = int.Parse(morphosisEntry.GetAttribute("noOfTreads"));
        string connectsTo = morphosisEntry.GetAttribute("connectsTo");

        // TODO

        StairExtrudeGeometry.create(this.id + "-Stair", this.points, floor.height, floor.elevation, dirVect, noOfTreads);
    }

}
