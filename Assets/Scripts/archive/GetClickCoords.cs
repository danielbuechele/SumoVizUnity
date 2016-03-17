using UnityEngine;
using System.Collections;


// http://docs.unity3d.com/ScriptReference/Collider.Raycast.html
// attach to terrain and in Game view it gives click coordinates on terrain

public class GetClickCoords : MonoBehaviour {

	public Collider coll;
	void Start() {
		coll = GetComponent<Collider>();
	}

	void Update() {
		//var mousePosInScene = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition).origin;
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (coll.Raycast (ray, out hit, 100.0F))
				Debug.Log (ray.GetPoint (100.0F).ToString ());
		}
	}

}
