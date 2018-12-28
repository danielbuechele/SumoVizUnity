using UnityEngine;

public class OpenWall : Wall {

    public override void createObject(GameObject parent) {
        WallExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1, -0.2f, floor.elevation, parent);
    }

}
