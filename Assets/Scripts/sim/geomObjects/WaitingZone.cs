using UnityEngine;
using UnityEditor;

public class WaitingZone : WunderZone {

    public override void createObject() {
        this.createSurfaceObject(new Color(0.39f, 0.24f, 0, .3f));
    }
    
}
