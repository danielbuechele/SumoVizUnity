using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class FileLoaderXML : MonoBehaviour {

	// Use this for initialization
	void Start () {

		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

		string path = "";
		if (Application.platform == RuntimePlatform.OSXEditor) {
			path = "Data/";
		} else if (Application.platform == RuntimePlatform.OSXPlayer) {
			path = "../../";
		} else if (Application.platform == RuntimePlatform.WindowsEditor) {
			path = "Data/";
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path = "../";
		}
		
		// loadPedestrianFile(path + "b090_combined.txt");
		loadGeometryFile (path + "geometry.txt");
		loadXMLFile(path + "out_flughafen-modell-gruppen.xml");

	}
	
	// Update is called once per frame
	void Update () {}

	// Load an XML file containing both, geometry and pedestrian positions
	void loadXMLFile(string filename) {
		if (!System.IO.File.Exists(Application.dataPath + "/" + filename)) {
		    return;
		}

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(System.IO.File.ReadAllText(Application.dataPath + "/" + filename));

		// Load geometry
		//foreach(XmlElement node in xmlDoc.SelectNodes("spatial/object")) { // TODO: load different walls.. xpath?

		//}

		// Load pedestrians
		XmlNode output = xmlDoc.SelectSingleNode("//output");
	
		if (output == null) {
			Debug.Log("Debug: No output / pedestrian position data found in file.");
			return;
		}

		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
		foreach(XmlElement node in output.SelectNodes("floor")) { // TODO: load different floors..
			string[] lines = node.InnerText.Split(new[]{";\r\n"}, StringSplitOptions.None);
			foreach (string line in lines) {
				string[] v = line.Split(',');
				if (v.Length>=3) {
					decimal time;
					int id;
					float x;
					float y;
					decimal.TryParse(v[0], out time);
					int.TryParse(v[1], out id);
					float.TryParse(v[2], out x);
					float.TryParse(v[3], out y);
					pl.addPedestrianPosition(new PedestrianPosition(id,time,x,y));
				}
			}
		}
		pl.createPedestrians ();
		
	}

	void loadPedestrianFile(string filename) {
		var sr = new StreamReader(Application.dataPath + "/" + filename);
		var fileContents = sr.ReadToEnd();
		sr.Close();

		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

		string[] lines = fileContents.Split("\n"[0]);
		foreach (string line in lines) {
			string[] v = line.Split(' ');
			if (v.Length>=3) {
				decimal time;
				int id;
				float x;
				float y;
				decimal.TryParse(v[0], out time);
				int.TryParse(v[1], out id);
				float.TryParse(v[2], out x);
				float.TryParse(v[3], out y);
				pl.addPedestrianPosition(new PedestrianPosition(id,time,x,y));
			}
		}

		
		pl.createPedestrians ();
	}

	void loadGeometryFile(string filename) {
		var sr = new StreamReader(Application.dataPath + "/" + filename);
		var fileContents = sr.ReadToEnd();
		sr.Close();

		string[] lines = fileContents.Split("\n"[0]);
		foreach (string line in lines) {
			string[] v = line.Split(' ');
			if (v.Length>=3) {

				float height = 0.0f;
				int indexCorrection = 0;
				if (!float.TryParse(v[v.Length-1],out height)) {
					indexCorrection = 1;
				}

				if ((v.Length+indexCorrection)%2==0) {
					string name = v[v.Length-3+indexCorrection];
					List<Vector2> list = new List<Vector2>();
					for (int i = 1; i<v.Length-4+indexCorrection; i=i+2) {
						float x;
						float y;
						if (float.TryParse(v[i], out x) && float.TryParse(v[i+1], out y)) {
							list.Add(new Vector2(x, y));
						}
					}

					string type = v[v.Length-2+indexCorrection];
					if (type == "wall") WallExtrudeGeometry.create(name, list, height, -0.2f);
					else if (type == "obstacle") ObstacleExtrudeGeometry.create(name, list, height);
					else if (type == "tree") TreeGeometry.create(name, list[0]);
					else AreaGeometry.create(name, list);
				}
			}
		}
	}
}


