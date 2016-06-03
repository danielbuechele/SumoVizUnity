using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AreaGeometry : Geometry  {


	public static void createOriginTarget (string name, List<Vector2> verticesList) {
		createOriginTarget (name, verticesList, new Color (1, 0, 0, .3f));
	}

	public static void createOriginTarget (string name, List<Vector2> verticesList, Color color) {
		if (verticesList.First() == verticesList.Last()) { //otherwise circular boundary definitions won't be rendered
			verticesList.Remove (verticesList.Last());
		}

		GameObject obstacle = new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
		obstacle.transform.position = new Vector3 (0, 0.02f, 0);
		//obstacle.GetComponent<Renderer>().material.color = new Color (1, 0, 0, .3f);
		//obstacle.GetComponent<Renderer>().material.shader = Shader.Find ("Transparent/Diffuse");
		var renderer = obstacle.GetComponent<Renderer>();
		renderer.sharedMaterial = new Material (Shader.Find ("Transparent/Diffuse"));
		renderer.sharedMaterial.color = color;
		create (obstacle, verticesList);
	}

	public static void createPlane(string name, List<Vector2> verticesList, Material material) {
		GameObject obstacle = new GameObject (name, typeof(MeshFilter), typeof(MeshRenderer));
		obstacle.transform.position = new Vector3 (0, 0.01f, 0);
		var renderer = obstacle.GetComponent<Renderer>();
		renderer.sharedMaterial = material;
		create (obstacle, verticesList);
	}

	public static void create(GameObject obstacle, List<Vector2> verticesList) {
		Vector2[] vertices2D = verticesList.ToArray();

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices2D);
		int[] indicesArray = tr.Triangulate();
		List<int> indices = new List<int>();
		for (int i = 0; i < indicesArray.Length; i ++) {
			indices.Add (indicesArray[i]);
		}

		// Create the Vector3 vertices
		List<Vector3> vertices = new List<Vector3>();
		for (int i = 0; i < vertices2D.Length; i ++) {
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

		MeshFilter mesh_filter = obstacle.GetComponent<MeshFilter> ();
		mesh_filter.mesh = mesh;

		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		gl.setWorldAsParent (obstacle);
	}
}
