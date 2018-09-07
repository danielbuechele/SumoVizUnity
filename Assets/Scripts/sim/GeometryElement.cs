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

    public virtual void createObject() {}

}
