using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeGeometry : Geometry  {

	public static void create (string name, Vector2 vertices) {
		GameObject tree = (GameObject) Instantiate(Resources.Load("BigTree"));
		tree.transform.position = new Vector3(vertices.x,0,vertices.y);
		tree.transform.localEulerAngles = new Vector3(0,(Random.Range(0, 360)),0);
		float scale = Random.Range (0.9f,1.2f);
		tree.transform.localScale = new Vector3(scale,scale,scale);
	}

}

