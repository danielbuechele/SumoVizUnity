using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// idea from Christos Tsiliakis 
public class AgentView : MonoBehaviour {

	//pedestrian that is currently followed
	private GameObject currentPed = null;


	// LateUpdate is called after all Update functions have been called. 
	void LateUpdate (){

		//List<Pedestrian> pedestrians = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ().pedestrians;
		//Debug.Log (pedestrians.Count);


		if (currentPed != null)
			followPedestrian (currentPed);
		//else
			//findRandomPedestrian ();

		//if (Cardboard.SDK.Triggered)
		//	findRandomPedestrian ();
	}

	/* Move the camera relative to the position of the pedestrian that is followed
	 */
	private void followPedestrian (GameObject pedestrian){
		Vector3 pedestrianPosition = pedestrian.transform.position;
		Vector3 newPosition = new Vector3 (pedestrianPosition.x, transform.position.y, pedestrianPosition.z); // not changing Y value
		transform.position = newPosition;
	}

	/* Find a random pedestrian from a list of GameObjected tagged with "pedestrian"
	 * Set 'currentPedestrian' to that random pedestrian only if hes is still active (moving)
	 */
/*
	private void findRandomPedestrian (){
		GameObject[] pedestrians = GameObject.FindGameObjectsWithTag ("pedestrian"); 

		if (pedestrians.Length == 0) {
			Debug.LogError ("No game objects are tagged with pedestrian");
		} else {
			System.Random random = new System.Random ();
			int position = random.Next (1, pedestrians.Length);
			bool isPedActive = pedestrians [position].GetComponentInChildren<Pedestrian> ().isActive ();

			while (!isPedActive) {
				position = random.Next (1, pedestrians.Length);
				isPedActive = pedestrians [position].GetComponentInChildren<Pedestrian> ().isActive ();
				//Debug.Log ("Set new pedestrian to follow : " + currentPedestrian + ". Is active : " + isPedActive);
			}
			currentPed = pedestrians [position];
			//Debug.Log ("Set new pedestrian to follow : " + currentPedestrian + ". Is active : " + isPedActive);
		}			
	}
*/
}
