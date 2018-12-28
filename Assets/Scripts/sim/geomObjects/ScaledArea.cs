using UnityEngine;

public class ScaledArea : WunderZone {

    public override void createObject(GameObject parent) {
        this.createSurfaceObject(new Color(0.43f, 0.98f, 0.39f, .3f), parent);
    }
    
}
