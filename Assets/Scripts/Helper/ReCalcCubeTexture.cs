/* 
	author: Dsphar
	http://answers.unity3d.com/answers/855632/view.html -> https://github.com/Dsphar/Cube_Texture_Auto_Repeat_Unity/blob/master/ReCalcCubeTexture.cs
	Attach this script to your Cube.
	After you change the scale of the Cube, either
	Click the "Update Texture" button [if in edit mode], or...
	Call reCalcCubeTexture() [if in runtime]
*/

#if UNITY_EDITOR //prevents contents from compiling into the final build
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;


public class ReCalcCubeTexture : MonoBehaviour {

	Vector3 currentScale = new Vector3();


	void Start() {
		currentScale = transform.localScale;   
	}
		
	public void reCalcCubeTexture() {

		//if the scale has changed, do something...
		if (currentScale != transform.localScale) {
			currentScale = transform.localScale;

			//if scale is (1, 1, 1) there is no need for a custom MeshFilter (for tiling the texture), revert to the standard cube MeshFilter
			if (currentScale == Vector3.one) {
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

				DestroyImmediate(GetComponent<MeshFilter>());
				gameObject.AddComponent<MeshFilter>();
				GetComponent<MeshFilter>().sharedMesh = cube.GetComponent<MeshFilter>().sharedMesh;

				DestroyImmediate(cube);
				return;
			}

			//if still here, update the UV map on the mesh so the texture will repeat at the correct scale
			float length = currentScale.x;
			float width = currentScale.z;
			float height = currentScale.y;

			Mesh mesh;

			#if UNITY_EDITOR
			MeshFilter meshFilter = GetComponent<MeshFilter>();
			Mesh meshCopy = (Mesh) Mesh.Instantiate(meshFilter.sharedMesh);
			mesh = meshFilter.mesh = meshCopy;
			#else
			mesh = GetComponent<MeshFilter>().mesh;
			#endif

			Vector2[] mesh_UVs = mesh.uv;

			//update UV map
			//Front
			mesh_UVs[2] = new Vector2(0, height);
			mesh_UVs[3] = new Vector2(length, height);
			mesh_UVs[0] = new Vector2(0, 0);
			mesh_UVs[1] = new Vector2(length, 0);

			//Back
			mesh_UVs[6] = new Vector2(0, height);
			mesh_UVs[7] = new Vector2(length, height);
			mesh_UVs[10] = new Vector2(0, 0);
			mesh_UVs[11] = new Vector2(length, 0);

			//Left
			mesh_UVs[19] = new Vector2(0, height);
			mesh_UVs[17] = new Vector2(width, height);
			mesh_UVs[16] = new Vector2(0, 0);
			mesh_UVs[18] = new Vector2(width, 0);

			//Right
			mesh_UVs[23] = new Vector2(0, height);
			mesh_UVs[21] = new Vector2(width, height);
			mesh_UVs[20] = new Vector2(0, 0);
			mesh_UVs[22] = new Vector2(width, 0);
		
			//Top
			mesh_UVs[4] = new Vector2(0, width);
			mesh_UVs[5] = new Vector2(length, width);
			mesh_UVs[8] = new Vector2(0, 0);
			mesh_UVs[9] = new Vector2(length, 0);

			//Bottom
			mesh_UVs[15] = new Vector2(0, width);
			mesh_UVs[13] = new Vector2(length, width);
			mesh_UVs[12] = new Vector2(0, 0);
			mesh_UVs[14] = new Vector2(length, 0);

			//apply new UV map
			mesh.uv = mesh_UVs;
			mesh.name = "Cube Instance";
			if (GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
				GetComponent<Renderer>().sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;

		}
	}
}
