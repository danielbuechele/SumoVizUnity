
public class ClosedWall : Wall {

    public override void createObject() {
        ObstacleExtrudeGeometry.create(this.id + "-ExtrudeObject", points, 1);
    }

}
