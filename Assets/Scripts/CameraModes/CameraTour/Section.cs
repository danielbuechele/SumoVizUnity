using UnityEngine;
using System;
using System.Collections;


public class Section {

	public enum Type {
		ACCELERATION, CONSTANT, DECELERATION, WAIT
	};

	private Type type;

	private Vector3 sectionStartCoord;
	private Vector3 sectionEndCoord;

	private float velocReducer;

	// s = distance
	private float s_inSection;
	private float s_end;

	// t = time
	private float t_upToHere;
	private float t_inSection;
	private float t_end;

	private float v_max;
	private float accel = 0; // acceleration/deceleration


	public Section (Type type, Vector3 sectionStartCoord, Vector3 sectionEndCoord, float velocReducer, float s_inSection) {
		this.type = type;
		this.sectionStartCoord = sectionStartCoord;
		this.sectionEndCoord = sectionEndCoord;
		this.velocReducer = velocReducer;
		if (velocReducer > 1)
			Debug.LogError ("Velocities higher than v_max in waypoints will look weird, stay between 0 and 1");
		this.s_inSection = s_inSection;
	}

	/*
	public override string ToString() {
		return string.Concat("id: ", id, "  s_upToHere: ", s_upToHere, "  s_inSection: ", s_inSection, "  velocReducer: ", velocReducer, "  startCoord: ", sectionStartCoord, "  endCoord: ", sectionEndCoord);
	}

	public string check() { // for dev
		return t_upToHere + " + " + t_inSection + " = " + t_end + "   /   accel: " + accel;
	}
	*/

	public float getFormulaContrib() {
		switch (type) {
		case Type.ACCELERATION:
		case Type.DECELERATION:
			return s_inSection / (0.5f * velocReducer + 0.5f);
		case Type.CONSTANT:
			return s_inSection;
		default: // Type.WAIT
			return 0f;		
		}
	}

	public void calcTinSection(float v_max) {
		this.v_max = v_max;

		switch (type) {
		case Type.ACCELERATION:
		case Type.DECELERATION:
			t_inSection = s_inSection / (v_max * (0.5f + 0.5f * velocReducer));
			break;
		case Type.CONSTANT:
			t_inSection = s_inSection / v_max;
			break;
		}
	}

	public void setTinSection(float t_inSection) { // used for WAIT sections to set waiting time
		this.t_inSection = t_inSection;
	}

	public float getTinSection(){
		return t_inSection;
	}

	public void setTupToHere(float t_upToHere) {
		this.t_upToHere = t_upToHere;
		t_end = t_upToHere + t_inSection;
	}

	public void calcAccel() {
		switch (type) {
		case Type.ACCELERATION:
			accel = (2f * (s_inSection - velocReducer * v_max * t_inSection)) / (float) Math.Pow(t_inSection, 2);
			break;
		case Type.DECELERATION:
			accel = (2f * (s_inSection - v_max * t_inSection)) / (float) Math.Pow(t_inSection, 2); // can by definition never start from a v different to v_max -> no velocReducer
			break;
		}
	}

	public bool thatsMe(float time) {
		if (time >= t_upToHere && time < t_end)
			return true;
		return false;		
	}

	private bool gaveSignal = false;

	public bool getIndexIncrementSignal() {
		if (type == Type.ACCELERATION && !gaveSignal) {
			gaveSignal = true;
			return true;
		}
		return false;			
	}

	public bool isDecel() {
		return type == Type.DECELERATION;
	}

	public void reset() {
		gaveSignal = false;
	}

	public float getProcProgressWithinSection(float absoluteTime) {
		float relativeTime = absoluteTime - t_upToHere; // within section
		float s_add = 0;
		switch (type) {
			case Type.ACCELERATION:
				s_add = velocReducer * v_max * relativeTime + 0.5f * accel * (float) Math.Pow (relativeTime, 2);
				break;
			case Type.DECELERATION:
				s_add = v_max * relativeTime + 0.5f * accel * (float) Math.Pow (relativeTime, 2);
				break;
			case Type.CONSTANT:
				s_add = v_max * relativeTime;
				break;
		  /*case Type.WAIT: //is default case anyway
				s_add = 0;
				break;*/
		}
		float s_relative = s_add / s_inSection;
		return s_relative;
	}

	public Vector3 getCoordAtT(float absoluteTime) {
		return Vector3.Lerp (sectionStartCoord, sectionEndCoord, type == Type.WAIT ? 0f : getProcProgressWithinSection (absoluteTime));
	}

}