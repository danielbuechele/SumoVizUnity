using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class GeometryElement {

    private List<Vector2> points;

    public void setPoints(List<Vector2> points) {
        this.points = points;
    }
}
