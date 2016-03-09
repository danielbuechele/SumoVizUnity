using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// initial implementation from Christos Tsiliakis 
public class AgentView : MonoBehaviour {

	private GameObject currentPed = null;


	void LateUpdate (){
		if (currentPed != null)
			followPedestrian (currentPed);
		else
			findRandomPedestrian ();

		//if(Cardboard.SDK.Triggered || Input.GetKeyDown (KeyCode.Space))
		//	findRandomPedestrian ();

		//TODO find trigger for gear vr oculus
	}
		
	private void followPedestrian (GameObject pedestrian) {
		Vector3 pedPos = pedestrian.transform.position;
		Vector3 newPos = new Vector3 (pedPos.x, pedPos.y + 2f, pedPos.z);
		Camera.main.transform.position = newPos;
	}

	private void findRandomPedestrian () {
		List<Pedestrian> peds = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ().pedestrians;
		if (peds.Count > 0) {
			int randIndex = -1;
			bool isPedActive = false;
			int i = 0;
			bool cancelled = false;
			while (!isPedActive && !cancelled) {
				randIndex = Random.Range (0, peds.Count);
				isPedActive = peds[randIndex].GetComponentInChildren<Renderer> ().enabled;
				if (i ++ > peds.Count)
					cancelled = true;
			}
			if(!cancelled)
				currentPed = peds[randIndex].gameObject;
		}	
	}
}
