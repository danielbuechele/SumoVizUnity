using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  { // walls

	public static void create (string name, List<Vector2> verticesList, float height) {

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
			height = 2.2f + Random.value * 1.2f;
		//}

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

		ExtrudeGeometry.create (name, verticesList, height, topMaterial, sideMaterial);
	}
}
