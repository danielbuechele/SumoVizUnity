using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  {

	public static void create  (string name, List<Vector2> verticesList, float height) {

		Material topMaterial;
		Material sideMaterial;
		
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();

		if (height <= 4.0) {
			sideMaterial = gl.theme.getBoxMaterial();
			topMaterial =  gl.theme.getBoxMaterial();
		} else {
			topMaterial = gl.theme.getRoofMaterial();
			sideMaterial = gl.theme.getHouseMaterial();
			sideMaterial.SetTextureScale("_MainTex", gl.theme.getTextureScaleForHeight((float) height));
		}

		ExtrudeGeometry.create (name, verticesList, height, topMaterial, sideMaterial);
	}
}

