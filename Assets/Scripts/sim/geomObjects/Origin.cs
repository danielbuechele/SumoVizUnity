using UnityEngine;

public class Origin : WunderZone {

    public override void createObject(GameObject parent) {
        this.createSurfaceObject(new Color(0.61f, 0.04f, 0, .3f), parent);
    }

}
