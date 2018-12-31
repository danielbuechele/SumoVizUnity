using UnityEngine;

public class OpenWall : Wall {

    public override void createObject(GameObject parent, GeometryLoader gl) {
        WallExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1, -0.2f, floor.elevation, parent, gl);
    }

}
