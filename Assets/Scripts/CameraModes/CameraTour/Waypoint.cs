using UnityEngine;
using System.Collections;

public class Waypoint {

	//private int id;
	private Vector3 point;
	private float velocReducer;
	private float waitingTime;
	private bool doFocusBool;
	private Vector3 focusPoint;


	public Waypoint(int id, Vector3 point, float velocReducer, float waitingTime, bool doFocus) {
		//this.id = id;
		this.point = point;
		this.velocReducer = velocReducer;
		this.waitingTime = waitingTime;

		if (waitingTime > 0 && velocReducer != 0)
			utils.crash ("Error in waypoint " + id + ": setting a waiting time requires the deceleration percentage to be 0, that wasn't the case");

		this.doFocusBool = doFocus;
	}

	public void setFocusPoint(Vector3 focusPoint) {
		this.focusPoint = focusPoint;
	}

	public Vector3 getPoint() {
		return point;
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

	public Vector3 getFocusPoint() {
		return focusPoint;
	}

}
