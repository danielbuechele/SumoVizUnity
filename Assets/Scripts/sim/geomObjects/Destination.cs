using UnityEngine;

public class Destination : WunderZone {

    public override void createObject(GameObject parent) {
        this.createSurfaceObject(new Color(0.18f, 0.71f, 0, .3f), parent);
    }

}
