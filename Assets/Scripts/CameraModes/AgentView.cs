using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// initial implementation from Christos Tsiliakis 
public class AgentView : MonoBehaviour {


	private GameObject currentPed = null;
	private GameObject mainCameraParent;

	void Start() {
		mainCameraParent = GameObject.Find ("MainCameraParent");
		Camera.main.nearClipPlane = 0.05f;
	}

	public GameObject getCurrentPed() {
		return currentPed;
	}

	void LateUpdate (){
		if (currentPed != null)
			followPedestrian (currentPed);
		else
			getOneOfLastPeds ();

		if (Input.GetMouseButtonDown (0))
			getOneOfLastPeds ();
	}
		
	private void followPedestrian (GameObject pedestrian) {
		Vector3 pedPos = pedestrian.transform.position;
		Vector3 newPos = new Vector3 (pedPos.x, pedPos.y + 1.66f, pedPos.z);
		mainCameraParent.transform.position = newPos;
	}


	private void getOneOfLastPeds () {
		List<int> indizes = new List<int> ();
		indizes.Add (3);
		indizes.Add (11);
		indizes.Add (16);
		indizes.Add (17);
		indizes.Add (10);
		List<Pedestrian> peds = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ().pedestrians;
		currentPed = peds[indizes [Random.Range(0, indizes.Count - 1)]].gameObject;
	}
	/*
	private void findRandomPedestrian () {
		List<Pedestrian> peds = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ().pedestrians;
		if (peds.Count > 0) {
			int randIndex = -1;
			bool isPedActive = false;
			int i = 0;
			bool cancelled = false;
			while (!isPedActive && !cancelled) {
				randIndex = Random.Range (peds.Count - 3, peds.Count);
				isPedActive = peds[randIndex].GetComponentInChildren<Renderer> ().enabled;
				if (i ++ > peds.Count)
					cancelled = true;
			}
			if (!cancelled)
				currentPed = peds [randIndex].gameObject;
		}	
	}
		
	private void findPedestrianFurthestFromDestination() {
		List<Pedestrian> peds = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ().pedestrians;
		float maxDist = 0;
		Vector3 destination = new Vector3 (16f, 1.5f, 5.3f);
		foreach (Pedestrian ped in peds) {
			float dist = ped.getDistTo (destination);
			if (dist > maxDist) {
				maxDist = dist;
				currentPed = ped.gameObject;
			}
		}
	}*/

}
