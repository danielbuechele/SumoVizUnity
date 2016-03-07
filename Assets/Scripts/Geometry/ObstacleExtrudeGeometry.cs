using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleExtrudeGeometry : ExtrudeGeometry  {

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

		ExtrudeGeometry.create (name, verticesList, height, topMaterial, sideMaterial);
	}
}

