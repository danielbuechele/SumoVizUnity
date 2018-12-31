using UnityEngine;
using System.Collections.Generic;


public class GeometryElement {

    internal Floor floor;
    internal List<Vector2> points;
    internal string id;
    private string layerId;

    internal void setFloor(Floor floor) {
        this.floor = floor;
    }

    internal void setPoints(List<Vector2> points) {
        this.points = points;
    }

    internal void setId(string id) {
        this.id = id;
    }

    internal string getId() {
        return id;
    }

    internal void setLayerId(string layerId) {
        this.layerId = layerId;
    }

    // for plane objects like origins, destinations, waiting zones
    public virtual void createObject(GameObject floor) {}

    // for  walls, obstacles
    public virtual void createObject(GameObject floor, GeometryLoader gl) {}

    // for elevators, escalators and stairs
    public virtual void createObject(GameObject floor, SimData simData, GeometryLoader gl) {}
}
