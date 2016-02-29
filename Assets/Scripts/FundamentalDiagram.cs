/*using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;
using System.IO;
using Vectrosity;

public class FundamentalDiagram : MonoBehaviour {


	public bool fundamental = false;
	private List<Label> yLabels = new List<Label> ();
	private List<Label> xLabels = new List<Label> ();
	private List<VectorLine> fundamentalDiagramLines = new List<VectorLine> ();
	private VectorPoints fundamentalDiagramPoints;
	private PlaybackControl pc;


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
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
	}

	void OnGUI () {

		GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
		centeredStyle.alignment = TextAnchor.UpperCenter;
		Rect position = new Rect(30,30, 400, 300);
		position.x = Screen.width - position.x - position.width;
		if (fundamental) {
			GUI.Label (new Rect(position.x-50, Screen.height-position.y-position.height-15, 50, 30), "[m/s]");
			GUI.Label (new Rect(position.x-10, Screen.height-position.y, 40, 30), "[1/m²]");
			GUI.Label (new Rect(position.x, Screen.height-position.y-position.height-30, position.width, 30), "fundamental diagram",centeredStyle);
			if (yLabels.Count>0) foreach (Label yl in yLabels) GUI.Label (yl.rect, yl.label);
			if (xLabels.Count>0) foreach (Label xl in xLabels) GUI.Label (xl.rect, xl.label);
			GUI.color = Color.red;
		}
		else {
			GUI.color = Color.white;
		}
		if (GUI.Button (new Rect (860, 20, 140, 30), "fundamental diagram")) {
			fundamental = !fundamental;
			if (fundamental) {
				GUI.color = Color.white;
				drawFundamentalDiagram(position);
			} else {
				removeFundamentalDiagram();
			}
		}

	}
	
	private void drawFundamentalDiagram(Rect position) {

		pc.playing = false;

		InfoText it = GameObject.Find ("InfoText").GetComponent<InfoText> ();
		if (it.diagram) it.removeDiagram();

		fundamentalDiagramLines = new List<VectorLine> ();
		PedestrianLoader pl = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ();
		float maxSpeed = float.MinValue;
		float maxDensity = float.MinValue;
		List<Vector2> points = new List<Vector2> ();
		foreach (GameObject p in pl.pedestrians) {
			Pedestrian ped = p.GetComponent<Pedestrian>();
			if (p.hideFlags != HideFlags.HideInHierarchy) {
				float speed = ped.getSpeed();
				float density = ped.getDensity();
				maxSpeed = Mathf.Max(speed,maxSpeed);
				maxDensity = Mathf.Max (density,maxDensity);
				points.Add(new Vector2(density,speed));
			}
		}

		if (points.Count==0) {
			fundamental = false;
			return;
		}

		VectorPoints.SetCamera (GameObject.Find ("Flycam").GetComponent<Camera>());

		//trendline
		
		int steps = 5;
		float stepper = maxDensity / steps;


		float[] avgSpeed = new float [steps];
		int[] avgNumber = new int [steps];
		for (int i = 0;i<points.Count;i++) {
			int j = (int) (points[i].y/stepper);
			if (j<avgNumber.Length) {
				avgNumber[j]++;
				avgSpeed[j] = avgSpeed[j] + points[i].x;
			}
		}
		List<Vector2> l = new List<Vector2> ();
		for (int i = 0;i<steps;i++) {
			avgSpeed[i] = avgSpeed[i]/avgNumber[i];
			Vector2 a = new Vector2 ((i*stepper+(i+1)*stepper)/2,avgSpeed[i]);
			if (avgSpeed[i]>0) l.Add ( new Vector2(a.x*position.width/maxDensity+position.x,a.y*position.height/maxSpeed+position.y));
		}
		VectorLine line;
		if (l.Count>1) {
			line = new VectorLine ("spline", new Vector2[l.Count], null, 1, LineType.Continuous);
			line.SetColor (Color.red);
			line.MakeSpline (l.ToArray ());
			line.depth = 99;
			line.Draw ();
			fundamentalDiagramLines.Add (line);
		}


		//data points
		for (int i = 0;i<points.Count;i++) {
			points[i] = new Vector2(points[i].x*position.width/maxDensity+position.x,points[i].y*position.height/maxSpeed+position.y);
		}

		fundamentalDiagramPoints = new Vectrosity.VectorPoints ("Data", points.ToArray (),Color.white,null,3);
		fundamentalDiagramPoints.depth = 99;
		fundamentalDiagramPoints.Draw ();



		//frame
		line = VectorLine.SetLine (new Color(1f,1f,1f,0.5f), new Vector2[] {
			new Vector2(position.x-6,position.y-2),
			new Vector2(position.x-6,position.y+9+position.height),
			new Vector2(position.x+5+position.width,position.y+9+position.height),
			new Vector2(position.x+5+position.width,position.y-2),
			new Vector2(position.x-6,position.y-2)
		});
		line.depth = 2;
		line.Draw ();
		fundamentalDiagramLines.Add (line);

		//lines
		yLabels = new List<Label> ();
		float scaleFactor = 0.5f;
		int numberOfLines = (int)(maxSpeed*(1/scaleFactor));
		if (numberOfLines>0) {
			for (int i = 1; i<=numberOfLines; i++) {
				float hy = position.y+ i*(position.height/(maxSpeed*(1/scaleFactor)));
				line = VectorLine.SetLine (new Color(1f,1f,1f,0.5f), new Vector2[] {
					new Vector2(position.x-5,hy),
					new Vector2(position.x+5+position.width,hy)
				});
				line.depth = 1;
				line.Draw ();
				fundamentalDiagramLines.Add (line);
				yLabels.Add (new Label(new Rect (position.x-28,Screen.height-hy-12,20,25),""+i*scaleFactor));
			}
		}

		//lines
		xLabels = new List<Label> ();
		scaleFactor = 0.5f;
		numberOfLines = (int)(maxDensity*(1/scaleFactor));
		if (numberOfLines>0) {
			for (int i = 1; i<=numberOfLines; i++) {
				float hx = position.x+ i*(position.width/(maxDensity*(1/scaleFactor)));
				line = VectorLine.SetLine (new Color(1f,1f,1f,0.5f), new Vector2[] {
					new Vector2(hx,position.y-2),
					new Vector2(hx,position.y+8+position.height)
				});
				line.depth = 1;
				line.Draw ();
				fundamentalDiagramLines.Add (line);
				xLabels.Add (new Label(new Rect (Screen.width-position.x-position.width+hx-44,Screen.height-position.y,28,28),""+i*scaleFactor));
			}
		}

		//background
		line = VectorLine.SetLine (new Color(0,0,0,0.7f), new Vector2[] {new Vector2(position.x-6,position.y+(position.height+6)/2),new Vector2(position.x+position.width+6,position.y+(position.height+6)/2)});
		line.lineWidth = position.height+12;
		line.Draw ();
		
		fundamentalDiagramLines.Add (line);


	}

	public void removeFundamentalDiagram() {
		fundamental = false;
		if (fundamentalDiagramLines.Count>0) {
			for (int i = 0;i<fundamentalDiagramLines.Count;i++ ) {
				VectorLine line = (VectorLine)fundamentalDiagramLines[i];
				VectorLine.Destroy(ref line);
			}
		}

		VectorPoints.Destroy (ref fundamentalDiagramPoints);
	}
	
	// Update is called once per frame
	void Update () {
		if (fundamental && pc.playing) removeFundamentalDiagram ();
	}
}
*/
