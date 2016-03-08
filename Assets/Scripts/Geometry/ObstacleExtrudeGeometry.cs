using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  { // walls

	public static void create (string name, List<Vector2> verticesList, float height) {

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		Material topMaterial = gl.theme.getWallMaterial();
		Material sideMaterial = gl.theme.getWallMaterial();

		/*
		if (height <= 4.0) {
			topMaterial = gl.theme.getBoxMaterial();
			sideMaterial = gl.theme.getBoxMaterial();
		} else { // make it a house then
			topMaterial = gl.theme.getRoofMaterial();
			sideMaterial = gl.theme.getHouseMaterial();
			sideMaterial.SetTextureScale("_MainTex", gl.theme.getTextureScaleForHeight((float) height));
		}
		*/

		if(verticesList[verticesList.Count - 1] == verticesList[0])
			verticesList.RemoveAt (verticesList.Count - 1);

		// http://debian.fmi.uni-sofia.bg/~sergei/cgsr/docs/clockwise.htm (found by @Lesya91)
		int n = verticesList.Count;
		int i, j, k;
		int count = 0;
		float val;
		if (n > 3) {
			for (i = 0; i < n; i ++) {
				j = (i + 1) % n;
				k = (i + 2) % n;
				val = (verticesList [j].x - verticesList [i].x) * (verticesList [k].y - verticesList [j].y);
				val -= (verticesList [j].y - verticesList [i].y) * (verticesList [k].x - verticesList [j].x);
				if (val < 0)
					count --;
				if (val > 0)
					count ++;
			}
			if (count > 0) // CCW
				verticesList.Reverse ();
		}

		/* det needs squared matrices!
		// https://en.wikipedia.org/wiki/Curve_orientation#Orientation_of_a_simple_polygon
		// http://blog.ivank.net/lightweight-matrix-class-in-c-strassen-algorithm-lu-decomposition.html
		string matrixString = "";

		foreach (Vector2 v in verticesList)
			matrixString += "1 " + v.x + " " + v.y + "\r\n";

		Matrix m = Matrix.Parse (matrixString);

		Debug.Log (m.ToString ());
		Debug.Log (m.Det ());
		Debug.Log ("");
		*/

		/*
		// math to detect cw or ccw from http://stackoverflow.com/a/1165943
		float edgeSum = 0;
		for(int i = 0; i < verticesList.Count - 1; i ++) {
			Vector2 current = verticesList [i];
			Vector2 next = verticesList [i + 1];
			float edge = (next.x - current.x) * (next.y + current.y);
			edgeSum += edge;	
			Debug.Log (edge);
		}

		Vector2 last = verticesList [0];
		Vector2 first = verticesList [verticesList.Count - 1];
		//edgeSum += (first.x - last.x) * (first.y + last.y);

		//Debug.Log (edgeSum);

		if (edgeSum < 0) { // counterclockwise
			Debug.Log (name + ": CCW"); 
		} else {
			Debug.Log (name + ": CW");
		}
		*/

		ExtrudeGeometry.create (name, verticesList, height, topMaterial, sideMaterial);
	}
}
