using UnityEngine;
using System.Collections;

public class Waypoint {

	private int id;
	private Vector3 coords;
	private float velocReducer;
	private float waitingTime;
	private bool doFocusBool;
	private Vector3 focusCoords;


	public Waypoint(int id, Vector3 coords, float velocReducer, float waitingTime, bool doFocus) {
		this.id = id;
		this.coords = coords;
		this.velocReducer = velocReducer;
		this.waitingTime = waitingTime;

		if(waitingTime > 0 && velocReducer != 0)
			Debug.LogError ("Error in waypoint " + id + ": setting a waiting time requires the deceleration percentage to be 0, that wasn't the case");

		this.doFocusBool = doFocus;
	}

	public void setFocusCoords(Vector3 focusCoords) {
		this.focusCoords = focusCoords;
	}

	public Vector3 getCoords() {
		return coords;
	}

	public bool doWait() {
		return waitingTime > 0;
	}

	public bool doFocus() {
		return doFocusBool;
	}

	public float getWaitingTime() {
		return waitingTime;
	}

	public float getVelocReducer() {
		return velocReducer;
	}

}
