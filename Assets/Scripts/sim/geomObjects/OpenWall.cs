using UnityEngine;
using UnityEditor;

public class OpenWall : Wall {

    public override void createObject() {
        WallExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1, -0.2f);
    }

}
