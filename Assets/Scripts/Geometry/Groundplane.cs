using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Groundplane : MonoBehaviour {
	/*
	public Vector3 point1;
	public bool point1active;
	public Vector3 point2;
	public bool point2active;
	VectorLine myLine;
	public int lineCrossed;

	public List<decimal> crossings = new List<decimal>();

	VectorLine myLine_tmp;
	public float crossingSpeed = 0.0f;
	public float avgFlow = 0.0f;

	// Use this for initialization
	void Start () {

	}

	void OnMouseDown(){
	
		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();

		if (pc.drawLine) {

			RaycastHit hit;
			Ray ray = GameObject.Find("Flycam").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if (GetComponent<Collider>().Raycast (ray, out hit, Mathf.Infinity)) {

				if (!point1active) {
					point1 = hit.point;
					point1active = true;
				} else if (!point2active)  {
					point2 = hit.point;
					VectorLine.SetCamera (GameObject.Find ("Flycam").GetComponent<Camera>());
					myLine = VectorLine.SetLine3D (Color.red, new Vector3[] {point1, point2});
					myLine.lineWidth = 3.0f;
					point2active = true;
					lineCrossed = 0;
					pc.lineDrawn();
				} else {
					removeLine();
				}

			}
		}
	}

	public void lineCross(float speed) {
		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		crossings.Insert (0,pc.current_time);

		crossingSpeed = (crossingSpeed*lineCrossed + speed)/(lineCrossed+1);
		lineCrossed++;

	}

	public void removeLine() {
		point1active = false;
		point2active = false;
		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		VectorLine.Destroy(ref myLine);
		pc.drawLine = false;
		lineCrossed = 0;
		crossingSpeed = 0.0f;

		crossings = new List<decimal> ();

		InfoText it = GameObject.Find ("InfoText").GetComponent<InfoText> ();
		if (it.diagram) it.removeDiagram();

	}
	
	// Update is called once per frame
	void Update () {

		if (point1active && !point2active) {
			RaycastHit hit;
			Ray ray = GameObject.Find("Flycam").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			if (GetComponent<Collider>().Raycast (ray, out hit, Mathf.Infinity)) {
				VectorLine.Destroy(ref myLine_tmp);
				VectorLine.SetCamera (GameObject.Find ("Flycam").GetComponent<Camera>());
				myLine_tmp = VectorLine.SetLine3D (Color.red, new Vector3[] {point1, hit.point});
				myLine_tmp.lineWidth = 3.0f;

				InfoText it = GameObject.Find ("InfoText").GetComponent<InfoText> ();
				if (it.diagram) it.removeDiagram();
			}
		} else {
			VectorLine.Destroy(ref myLine_tmp);
			if (point1active && point2active) {
				PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
				for (int i = 0; i< crossings.Count; i++) {
					if (crossings[i]<pc.current_time-1) crossings.RemoveAt(i);
				}
			}
		}
	}
	*/
}
