using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;
using System.IO;
using Vectrosity;

public class PlaybackControl : MonoBehaviour {

	public bool playing = true;
	public decimal current_time;
	public decimal slider_value;
	public decimal total_time;
	public int tiles = 0;
	public bool drawLine;
	public TileColoringMode tileColoringMode = TileColoringMode.TileColoringNone;
	public bool trajectoriesShown;
	public float threshold;
	public bool fundamental = false;

	private List<Label> yLabels = new List<Label> ();
	private List<Label> xLabels = new List<Label> ();
	private List<VectorLine> fundamentalDiagramLines = new List<VectorLine> ();
	private VectorPoints fundamentalDiagramPoints;
	bool lineIsDrawn;


	public struct Label
	{
		public Rect rect;
		public string label;
		
		public Label (Rect r, string s)
		{
			label = s;
			rect = r;
		}
	}


	// Use this for initialization
	void Start () {
		current_time = 0;
		total_time = 0;
		playing = true;
		threshold = 2.0f;
	}

	void OnGUI () {

		playing = GUI.Toggle(new Rect(30, 25, 100, 30), playing, " PLAY");
		current_time = (decimal) GUI.HorizontalSlider (new Rect (100, 30, 400, 30), (float) current_time, 0.0f, (float) total_time);

		string btnText = "show trajectories";
		if (trajectoriesShown) btnText = "hide trajectories";

		if (GUI.Button (new Rect (510,20,120,30), btnText)) {
			PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
			if (trajectoriesShown) {
				foreach (GameObject p in pl.pedestirans) {
					p.GetComponent<Pedestrian>().hideTrajectory();
				}
				trajectoriesShown = false;
			} else {
				foreach (GameObject p in pl.pedestirans) {
					p.GetComponent<Pedestrian>().showTrajectory();
				}
				trajectoriesShown = true;
			}
		}



		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		Groundplane gp = gl.groundplane;

		btnText = "add line";
		if (drawLine) GUI.color = Color.red;
		if (lineIsDrawn) btnText = "remove line";

		if (GUI.Button (new Rect (640, 20, 80, 30), btnText)) {
			if (lineIsDrawn) {
				gp.removeLine();
				drawLine = false;
				lineIsDrawn = false;
			} else {
				drawLine = !drawLine;
				if (!drawLine) {
					gp.removeLine();
				}
			}
		}
		GUI.color = Color.white;

		if (tiles == 0) btnText = "colors by speed";
		if (tiles == 1) btnText = "colors by density";
		if (tiles == 2) btnText = "hide colors";

		if (GUI.Button (new Rect (730, 20, 120, 30), btnText)) {
			tiles = (tiles+1)%3;

			InfoText it = GameObject.Find ("InfoText").GetComponent<InfoText> ();
			if (it.diagram) it.removeDiagram();

			if (tiles == 0) tileColoringMode = TileColoringMode.TileColoringNone;
			if (tiles == 1) tileColoringMode = TileColoringMode.TileColoringSpeed;
			if (tiles == 2) tileColoringMode = TileColoringMode.TileColoringDensity;
		}

		if (tileColoringMode == TileColoringMode.TileColoringDensity) {
			threshold = GUI.HorizontalSlider (new Rect (730, 55, 120, 30), threshold, 0.0f, 6.0f);
			GUI.Label(new Rect (730, 70, 120, 30),"Threshold: "+System.Math.Round(threshold,2)+"/m²");
		}



	}

	public void lineDrawn() {
		drawLine = false;
		lineIsDrawn = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (playing) {
			try {
				current_time = (current_time + (decimal) Time.deltaTime) % total_time;
			} catch (DivideByZeroException) {
				current_time = 0;
			}
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			playing = !playing;
		}

	}
}
