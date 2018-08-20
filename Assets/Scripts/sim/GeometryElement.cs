using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class GeometryElement {

    private List<Vector2> points;
    private string id;

    internal void setPoints(List<Vector2> points) {
        this.points = points;
    }

    internal void setId(string id) {
        this.id = id;
    }

    internal string getId() {
        return id;
    }
}
