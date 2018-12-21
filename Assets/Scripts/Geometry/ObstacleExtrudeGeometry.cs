using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  { // walls

	public static void create (string name, List<Vector2> verticesList, float height, float elevation) {

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		Material topMaterial;
		Material sideMaterial;

		/*if (name.Contains ("_house")) {
			topMaterial = gl.theme.getRoofMaterial ();
			sideMaterial = gl.theme.getHouseMaterial ();
			sideMaterial.SetTextureScale ("_MainTex", gl.theme.getTextureScaleForHeight (4f));
			height = 6f;
		} else {*/
			topMaterial = gl.theme.getWallMaterial();
			sideMaterial = topMaterial;
//			height = 1f;//2.2f + Random.value * 1.2f;
		//}

		if(verticesList[verticesList.Count - 1] == verticesList[0])
			verticesList.RemoveAt (verticesList.Count - 1);

        /* doesn't work for objects with 3 vertices ~ BD 27.8.2018
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
		}*/

        // via https://stackoverflow.com/a/1165943
        double edgeValSum = 0;
        for (int i = 0; i < verticesList.Count - 1; i++) {
            edgeValSum += getEdgeValue(verticesList, i, i + 1);
        }
        // must be connected back to the first point
        edgeValSum += getEdgeValue(verticesList, verticesList.Count - 1, 0);
        if (edgeValSum < 0) {
            verticesList.Reverse();
        }

        ExtrudeGeometry.create (name, verticesList, height, elevation, topMaterial, sideMaterial);
	}

    private static double getEdgeValue(List<Vector2> verticesList, int thisIdx, int nextIdx) {
        Vector2 thisPoint = verticesList[thisIdx];
        Vector2 nextPoint = verticesList[nextIdx];
        return (nextPoint.x - thisPoint.x) * (nextPoint.y + thisPoint.y);
    }
}
