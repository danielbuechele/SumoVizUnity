using UnityEngine;

public class ClosedWall : Wall {

    public override void createObject(GameObject parent) {
        ObstacleExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1, floor.elevation, parent);
    }

}
