using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaGeometry : Geometry  {

	public static void create (string name, List<Vector2> verticesList) {


		GameObject obstacle = new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
		MeshFilter mesh_filter = obstacle.GetComponent<MeshFilter> ();

		obstacle.transform.position = new Vector3 (0, 0.01f, 0);
		//obstacle.GetComponent<Renderer>().material.color = new Color (1, 0, 0, .3f);
		//obstacle.GetComponent<Renderer>().material.shader = Shader.Find ("Transparent/Diffuse");
		var renderer = obstacle.GetComponent<Renderer>();
		renderer.sharedMaterial = new Material (Shader.Find ("Transparent/Diffuse"));
		renderer.sharedMaterial.color = new Color (1, 0, 0, .3f);

		Vector2[] vertices2D = verticesList.ToArray();
		
		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indicesArray = tr.Triangulate();
		List<int> indices = new List<int>();
		for (int i = 0;i<indicesArray.Length;i++) {
			indices.Add (indicesArray[i]);
		}

		// Create the Vector3 vertices
		List<Vector3> vertices = new List<Vector3>();

		for (int i=0; i<vertices2D.Length; i++) {
			vertices.Add (new Vector3(vertices2D[i].x, 0, vertices2D[i].y));
		}

		// Create the mesh
		Mesh mesh = new Mesh();

		mesh.vertices = vertices.ToArray();
		mesh.uv = verticesList.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		//flip if needed
		if (mesh.normals [0].y == -1) {
			indices.Reverse ();
			mesh.triangles = indices.ToArray ();
			mesh.RecalculateNormals();
		}

		mesh_filter.mesh = mesh;

		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		gl.setWorldAsParent (obstacle);
	}
}
