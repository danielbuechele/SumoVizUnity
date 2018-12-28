using UnityEngine;

public class WaitingZone : WunderZone {

    public override void createObject(GameObject parent) {
        this.createSurfaceObject(new Color(0.39f, 0.24f, 0, .3f), parent);
    }
    
}
