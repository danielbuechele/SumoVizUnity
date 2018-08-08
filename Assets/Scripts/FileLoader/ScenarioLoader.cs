using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class ScenarioLoader {

	private XmlDocument xmlDoc = new XmlDocument();
	//private List<string> specials = new List<string> ();

	public ScenarioLoader(string filepath) {
		xmlDoc.LoadXml (utils.loadFileIntoEditor (filepath));
		 
		/*
		// for big project
		string wallToOpenWallCasesFilecontent = utils.loadFileIntoEditor (Application.dataPath + "/Resources/Data/_ignore/wallToOpenWallCases.txt");
		string[] lines = wallToOpenWallCasesFilecontent.Split("\n"[0]);
		foreach (string line in lines)
			if (line.Length > 0)
				if (line.Substring (0, 1) != "#")
					specials.Add (line.Split ('#')[0].Trim ());*/
	}

	public string getRelativeTrajFilePath() {
		string relativeTrajFilePath = "";
		XmlNode output = xmlDoc.SelectSingleNode("//output");
		if (output != null) {
			foreach (XmlElement floor in output.SelectNodes("floor")) { // TODO more floors
				relativeTrajFilePath = floor.GetAttribute ("csvAt");
			}
		}
		return relativeTrajFilePath;
	}
		
	public void loadScenario() {
		XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");
		foreach(XmlElement floor in spatial.SelectNodes("floor")) { // TODO: more floors
			float height = 1f; // used 2.4 in disco scene
			foreach (XmlElement collection in floor.SelectNodes("collection")) { // that's the new element in the XML format, added by DrGeli
				foreach (XmlElement geomObj in collection.SelectNodes("object")) {
					string name = collection.GetAttribute ("id") + "-" + geomObj.GetAttribute ("name");
					switch (geomObj.GetAttribute("type")) {
						case "openWall":
							WallExtrudeGeometry.create(name, parsePoints(geomObj), height, -0.2f);
							break;
						case "wall":
							/*if (specials.Contains (name)) {
								List<Vector2> vertices = parsePoints (geomObj);
								Vector2 first = vertices [0];	
								Vector2 last = vertices[vertices.Count - 1];
								Vector2 secondLast = vertices[vertices.Count - 2];
								if (first == last) {
									Vector2 newLast = Vector2.Lerp (secondLast, last, 0.99f);
									vertices.RemoveAt (vertices.Count - 1);
									vertices.Add (newLast);
								} else {
									Vector2 newLast = Vector2.Lerp (last, first, 0.99f);
									vertices.Add (newLast);
								}
								WallExtrudeGeometry.create (name, vertices, height, -0.2f);
							}
							else*/
								ObstacleExtrudeGeometry.create(name, parsePoints(geomObj), height);
							break;
						case "origin":
							AreaGeometry.createOriginTarget (name, parsePoints (geomObj), new Color (0.61f, 0.04f, 0, .3f));
							break;
						case "destination":
							AreaGeometry.createOriginTarget (name, parsePoints (geomObj), new Color (0.18f, 0.71f, 0, .3f));
							break;
						case "scaledArea":
							AreaGeometry.createOriginTarget(name, parsePoints(geomObj), new Color (0.43f, 0.98f, 0.39f, .3f));
							break;
						case "waitingZone":
							AreaGeometry.createOriginTarget(name, parsePoints(geomObj), new Color (0.39f, 0.24f, 0, .3f));
							break;
						case "portal":
						case "beamExit":
						case "eofWall":
						case "queuingArea":
							AreaGeometry.createOriginTarget(name, parsePoints(geomObj));
							break;
						default:
							Debug.Log("Warning: XML geometry parser: Don't know how to parse Object of type '" + geomObj.GetAttribute("type") + "'.");
							break;
					}
				}
			}
		}

		/*
		Debug.Log ("minX: " + minX);
		Debug.Log ("maxX: " + maxX);
		Debug.Log ("minY: " + minY);
		Debug.Log ("maxY: " + maxY);
		*/
	}


	static float minX = float.MaxValue; 
	static float maxX = 0;
	static float minY = float.MaxValue;
	static float maxY = 0;

	// Parse an XmlElement full of <point> XmlElements into a coordinate list 
	static List<Vector2> parsePoints(XmlElement polyPoints) {
		List<Vector2> list = new List<Vector2>();
		foreach(XmlElement point in polyPoints.SelectNodes("point")) {
			float x = 0;
			float y = 0;
			if (float.TryParse(point.GetAttribute("x"), out x) && float.TryParse(point.GetAttribute("y"), out y)) {
				list.Add(new Vector2(x, y));
			}

			if (x < minX)
				minX = x;
			if (x > maxX)
				maxX = x;
			if (y < minY)
				minY = y;
			if(y > maxY)
				maxY = y;
		}
		return list;
	}

	public List<float> getBoundingPoints() {
		return new List<float> (){minX, minY, maxX, maxY};
	}

	/*
	static string listToString(List<Vector2> list) {
		string str = "";
		foreach (Vector2 v in list) {
			str += "x:" + v.x + "/y:" + v.y + "_";
		}
		return str;
	}
	*/
}
