using UnityEngine;

public class ClosedWall : Wall {

    public override void createObject(GameObject parent, GeometryLoader gl) {
        ObstacleExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1, floor.elevation, parent, gl.getTheme().getWallMaterial(), gl.getTheme().getWallMaterial(), gl.obstaclePrefab);
    }

}
