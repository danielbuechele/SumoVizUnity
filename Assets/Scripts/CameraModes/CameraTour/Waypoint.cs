using UnityEngine;
using System.Collections;

public class Waypoint {

	private Vector3 point;
	private float velocReducer;
	private float waitingTime;
	private Vector3 focusPoint;

	public Waypoint(int id, Vector3 point, float velocReducer, float waitingTime, Vector3 focusPoint) {
		this.point = point;
		this.velocReducer = velocReducer;
		this.waitingTime = waitingTime;

		if (waitingTime > 0 && velocReducer != 0)
			throw new UnityException ("Error in waypoint " + id + ": setting a waiting time requires the deceleration percentage to be 0, that wasn't the case");
	
		this.focusPoint = focusPoint;
	}

	public Vector3 getPoint() {
		return point;
	}

	public bool doWait() {
		return waitingTime > 0;
	}

	public float getWaitingTime() {
		return waitingTime;
	}

	public float getVelocReducer() {
		return velocReducer;
	}

	public Vector3 getFocusPoint() {
		return focusPoint;
	}

}
