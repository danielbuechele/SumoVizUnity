using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Vectrosity;

public struct Entry
{
	public string name;
	public string unit;
	public int decimals;
	public float value;
	public bool graphable;
	public float maxValue;

	public Entry(string n, string u, float val, int d, bool g, float m)
	{
		name = n;
		unit = u;
		decimals = d;
		value = val;
		graphable = g;
		maxValue = m;
	}
}

public struct CurrentValue {
	public decimal time;
	public float value;
	public CurrentValue(decimal t, float v) {
		time = t;
		value = v;
	}
}

public class InfoText : MonoBehaviour {

	private List<Entry> infos;

	private float maxDensity = float.MinValue;
	private float minDenstiy = float.MaxValue;
	private float avgDenstiy = float.MinValue;
	private int crossings;
	
	private int activeEntry = -1;
	private VectorLine dataPoints;
	private List<VectorLine> diagramLines = new List<VectorLine> ();
	private VectorLine timeIndicator;
	private Rect diagramPosition;
	public bool diagram;

	PlaybackControl pc;

	private List<Label> yLabels = new List<Label> ();
	private List<Label> xLabels = new List<Label> ();

	private List<CurrentValue> currentSpeed = new List<CurrentValue>();
	private List<CurrentValue> currentDensity = new List<CurrentValue>();


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


	private float maxSpeed = float.MinValue;
	private float minSpeed = float.MaxValue;


	// Use this for initialization
	void Start () {
		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
	}
	
	// Update is called once per frame
	void Update () {

		InvokeRepeating("UpdateSecond", 0, 1.0f);

	}

	void UpdateSecond() {
		if (activeEntry>-1) {
			FundamentalDiagram fd = GameObject.Find ("FundamentalDiagram").GetComponent<FundamentalDiagram> ();
			fd.removeFundamentalDiagram();

			PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
			float tx = ((float)(pc.current_time/pc.total_time))*diagramPosition.width+diagramPosition.x;
			timeIndicator.points2 = new Vector2[] {
				new Vector2(tx,diagramPosition.y-2),
				new Vector2(tx,diagramPosition.y+8+diagramPosition.height)
			};
			timeIndicator.depth = 100;
			timeIndicator.Draw();

			Vector2[] dp = dataPoints.points2;
			try {
				dp[((int)pc.current_time)-1] = new Vector2(diagramPosition.x+diagramPosition.width*((float)(pc.current_time/pc.total_time)),diagramPosition.y+Mathf.Min (1.0f,(infos[activeEntry].value/infos[activeEntry].maxValue))*diagramPosition.height);
				dataPoints.points2 = dp;
				dataPoints.Draw();
			} catch (System.IndexOutOfRangeException) {
			}
		}
	}

	void OnGUI () {

		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		PedestrianLoader pl = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ();
		GeometryLoader gl = GameObject.Find ("GeometryLoader").GetComponent<GeometryLoader> ();
		Groundplane gp = gl.groundplane;

		infos = new List<Entry> ();

		string text = "";
		string minutes = Mathf.Floor((float)pc.current_time / 60).ToString("00");;
		string seconds = ((float)pc.current_time%60).ToString("00");;
		text += "current time: "+minutes+":"+seconds+"\n";

		if (pl.population != null) {

			infos.Add( new Entry("population","",pl.population[(int) pc.current_time],0,true,pl.pedestirans.Count));
		}

		if (gp.point1active && gp.point2active) {
			infos.Add( new Entry("line length","m",Vector3.Distance(gp.point1, gp.point2),2,false,0));
			infos.Add( new Entry("line crossings","",gp.lineCrossed,0,true,pl.pedestirans.Count));
			infos.Add( new Entry("line flow","/s",gp.crossings.Count,0,true,10.0f));
			infos.Add( new Entry("avg. crossing speed","m/s",gp.crossingSpeed,2,true,3.0f));
			infos.Add( new Entry("current flow","/ms",gp.crossings.Count/Vector3.Distance(gp.point1, gp.point2),2,true,3.0f));
		}

		if (pc.tileColoringMode == TileColoringMode.TileColoringSpeed) {
			infos.Add( new Entry("current speed","m/s",currentValue(currentSpeed),2,true,3.0f));
			infos.Add( new Entry("min. speed","m/s",minSpeed,2,false,3.0f));
			infos.Add( new Entry("max. speed","m/s",maxSpeed,2,false,3.0f));
		} else {
			maxSpeed = float.MinValue;
			minSpeed = float.MaxValue;
		}

		if (pc.tileColoringMode == TileColoringMode.TileColoringDensity) {
			infos.Add( new Entry("current density","/m²",currentValue(currentDensity),2,true,3.0f));
			infos.Add( new Entry("min. density","/m²",minDenstiy,2,false,5.0f));
			infos.Add( new Entry("max. density","/m²",maxDensity,2,false,5.0f));
		} else {
			maxDensity = float.MinValue;
			minDenstiy = float.MaxValue;
			crossings = 0;
		}

		for (int i =0;i<infos.Count;i++) {
			Entry e = infos[i];
			text += infos[i].name+": "+System.Math.Round(e.value,e.decimals)+e.unit+"\n";
			if (e.graphable) {
				if (GUI.Toggle(new Rect(Screen.width*(transform.position.x)-20, Screen.height*(1-transform.position.y)-(15*(infos.Count-i)+17), 100, 15), i==activeEntry, "") && i != activeEntry) {
					removeDiagram ();
					activeEntry = i;
					Rect position = new Rect(30,30, 400, 300);
					position.x = Screen.width - position.x - position.width;
					drawDiagram(position);
				}
			}
		}
		guiText.text = text;

		if (diagram) {

			GUI.Label(new Rect(diagramPosition.x, Screen.height-diagramPosition.y-diagramPosition.height-30, diagramPosition.width, 30), infos[activeEntry].name);
			GUI.Label(new Rect(diagramPosition.x-15, Screen.height-diagramPosition.y, 30, 30), "[s]");
			GUI.Label(new Rect(diagramPosition.x-35, Screen.height-diagramPosition.y-diagramPosition.height-10, 30, 30), System.Math.Round(infos[activeEntry].maxValue,infos[activeEntry].decimals)+"");
			if (infos[activeEntry].unit!="") {
				GUI.Label(new Rect(diagramPosition.x-35, Screen.height-diagramPosition.y-diagramPosition.height-30, 30, 30), "["+infos[activeEntry].unit+"]");
			}
		}

	}

	private void drawDiagram(Rect position) {

		diagram = true;
		FundamentalDiagram fd = GameObject.Find ("FundamentalDiagram").GetComponent<FundamentalDiagram> ();

		if (fd.fundamental) fd.removeFundamentalDiagram();

		diagramPosition = position;
		diagramLines = new List<VectorLine> ();
		PedestrianLoader pl = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ();
		float maxSpeed = float.MinValue;
		float maxDensity = float.MinValue;
		List<Vector2> points = new List<Vector2> ();
		foreach (GameObject p in pl.pedestirans) {
			Pedestrian ped = p.GetComponent<Pedestrian>();
			Renderer r = ped.GetComponentInChildren<Renderer>() as Renderer;
			if (r.enabled) {
				float speed = ped.getSpeed();
				float density = ped.getDensity();
				maxSpeed = Mathf.Max(speed,maxSpeed);
				maxDensity = Mathf.Max (density,maxDensity);
				points.Add(new Vector2(speed,density));

			}
		}
		

		for (int i = 0;i<points.Count;i++) {
			points[i] = new Vector2(points[i].x*position.width/maxSpeed+position.x,points[i].y*position.height/maxDensity+position.y);
		}

		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		
		//data points
		VectorPoints.SetCamera (GameObject.Find ("Flycam").camera);

		Vector2[] dp = new Vector2[(int)pc.total_time];
		for (int i = 0;i<dp.Length;i++) {dp[i] = new Vector2(position.x+position.width*((float)(i/pc.total_time)),position.y);}
		dataPoints = VectorLine.SetLine (Color.white, dp);
		dataPoints.depth = 99;
		dataPoints.Draw ();
		diagramLines.Add (dataPoints);
		
		//frame
		VectorLine line = VectorLine.SetLine (new Color(1f,1f,1f,0.5f), new Vector2[] {
			new Vector2(position.x-6,position.y-2),
			new Vector2(position.x-6,position.y+9+position.height),
			new Vector2(position.x+5+position.width,position.y+9+position.height),
			new Vector2(position.x+5+position.width,position.y-2),
			new Vector2(position.x-6,position.y-2)
		});
		line.depth = 2;
		line.Draw ();
		diagramLines.Add (line);
		

		int maxTime = (int)pc.total_time;

		//lines
		xLabels = new List<Label> ();
		float scaleFactor = 10f;
		int numberOfLines = (int)(maxTime*(1/scaleFactor));
		if (numberOfLines>0) {
			for (int i = 1; i<=numberOfLines; i++) {
				float hx = position.x+ i*(position.width/(maxTime*(1/scaleFactor)));
				line = VectorLine.SetLine (new Color(1f,1f,1f,0.5f), new Vector2[] {
					new Vector2(hx,position.y-2),
					new Vector2(hx,position.y+8+position.height)
				});
				line.depth = 1;
				line.Draw ();
				diagramLines.Add (line);
				xLabels.Add (new Label(new Rect (Screen.width-position.x-position.width+hx-44,Screen.height-position.y,28,28),""+i*scaleFactor));
			}
		}

		float tx = ((float)(pc.current_time/pc.total_time))*position.width+position.x;
		timeIndicator = VectorLine.SetLine (Color.red, new Vector2[] {
			new Vector2(tx,position.y-2),
			new Vector2(tx,position.y+8+position.height)
		});
		
		//background
		line = VectorLine.SetLine (new Color(0,0,0,0.7f), new Vector2[] {new Vector2(position.x-6,position.y+(position.height+6)/2),new Vector2(position.x+position.width+6,position.y+(position.height+6)/2)});
		line.lineWidth = position.height+12;
		line.Draw ();
		
		diagramLines.Add (line);
	}

	public void removeDiagram() {
		diagram = false;
		activeEntry = -1;
		if (diagramLines.Count>0) {
			for (int i = 0;i<diagramLines.Count;i++ ) {
				VectorLine line = (VectorLine)diagramLines[i];
				VectorLine.Destroy(ref line);
			}
		}
		VectorLine.Destroy(ref timeIndicator);
	}

	public void updateSpeed(float s) {
		currentSpeed.Insert (0,new CurrentValue(pc.current_time, s));
		maxSpeed = Mathf.Max(maxSpeed,s);
		minSpeed = Mathf.Min(minSpeed,s);
	}

	public void updateDensity(float d) {

		currentDensity.Insert (0,new CurrentValue(pc.current_time, d));
		crossings++;
		maxDensity = Mathf.Max(maxDensity,d);
		minDenstiy = Mathf.Min(minDenstiy,d);
	}

	private float currentValue(List<CurrentValue> l) {
		float v = 0;
		for (int i = l.Count-1;i>=0;i--) {
			if (l[i].time<pc.current_time-1) l.RemoveAt(i);
			else v += l[i].value;
		}
		return v/l.Count;
	}

}
