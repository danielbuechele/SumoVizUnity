using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class ScenarioLoader {

	private XmlDocument xmlDoc = new XmlDocument();
	private string directoryName;


	public ScenarioLoader(string filepath) {
		if (!System.IO.File.Exists (filepath)) {
			Debug.LogError ("Error: scenario file " + filepath + " not found.");
		} else {
			xmlDoc.LoadXml (System.IO.File.ReadAllText (filepath));
			directoryName = Path.GetDirectoryName (filepath);
		}
	}

	public string getTrajectoryFilePath() {
		XmlNode output = xmlDoc.SelectSingleNode("//output");
		string trajectoryFilePath = "";
		foreach(XmlElement floor in output.SelectNodes("floor")) { // TODO more floors
			trajectoryFilePath = directoryName + "/" + floor.GetAttribute("csvAt");
		}
		return trajectoryFilePath;
	}
		
	public void loadScenario() {
		XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");
		foreach(XmlElement floor in spatial.SelectNodes("floor")) { // TODO: more floors
			float height = 1f; // TryParseWithDefault.ToSingle(floor.GetAttribute("height"), 1.0f);
			foreach (XmlElement collection in floor.SelectNodes("collection")) { // that's the new element in the XML format, added by DrGeli
				foreach (XmlElement geomObj in collection.SelectNodes("object")) {
					switch (geomObj.GetAttribute("type")) {
						case "openWall":
						WallExtrudeGeometry.create(geomObj.GetAttribute("name_") + listToString(parsePoints(geomObj)), parsePoints(geomObj), height, -0.2f);
							break;
						case "wall":
						ObstacleExtrudeGeometry.create(geomObj.GetAttribute("name_") + listToString(parsePoints(geomObj)), parsePoints(geomObj), height);
							break;
						/*
						case "origin":
						case "destination":
						case "scaledArea":
						case "waitingZone":
						case "portal":
						case "beamExit":
						case "eofWall":
						case "queuingArea":
							AreaGeometry.createOriginTarget(geomObj.GetAttribute("name"), parsePoints(geomObj));
							break;
						*/
						default:
							Debug.Log("Warning: XML geometry parser: Don't know how to parse Object of type '" + geomObj.GetAttribute("type") + "'.");
							break;
					}
				}
			}
		}
			
		// groundplane below scenario

		/*
		Debug.Log ("minX: " + minX);
		Debug.Log ("maxX: " + maxX);
		Debug.Log ("minY: " + minY);
		Debug.Log ("maxY: " + maxY);
		*/

		// add wall thickness
		//minX -= 0.2f;
		maxX += 0.2f;
		minY -= 0.2f;
		maxY += 0.2f;
		List<Vector2> list = new List<Vector2>();
		list.Add (new Vector2 (minX, minY));
		list.Add (new Vector2 (minX, maxY));
		list.Add (new Vector2 (maxX, maxY));
		list.Add (new Vector2 (maxX, minY));
		//AreaGeometry.createPlane ("ground", list, (Material) Resources.Load ("evaktisch/Boden", typeof(Material)));

		// two extra evaktisch areas

		list = new List<Vector2>();
		list.Add (new Vector2 (8.25f, 2.65f - 0.2f));
		list.Add (new Vector2 (8.25f, 3.56f + 0.2f));
		list.Add (new Vector2 (11.9f, 3.56f + 0.2f));
		list.Add (new Vector2 (11.9f, 2.65f- 0.2f));
		//AreaGeometry.createPlane ("ground", list, (Material) Resources.Load ("evaktisch/Boden", typeof(Material)));

		list = new List<Vector2>();
		list.Add (new Vector2 (8.25f, 5.33f + 0.2f));
		list.Add (new Vector2 (8.25f, 6.18f));
		list.Add (new Vector2 (11.9f, 6.18f));
		list.Add (new Vector2 (11.9f, 5.33f + 0.2f));
		//AreaGeometry.createPlane ("ground", list, (Material) Resources.Load ("evaktisch/Boden", typeof(Material)));

	}

	static float minX = float.MaxValue; 
	static float maxX = 0;
	static float minY = float.MaxValue;
	static float maxY = 0;

	// Parse an XmlElement full of <point> XmlElements into a coordinate list 
	static List<Vector2> parsePoints(XmlElement polyPoints) {
		List<Vector2> list = new List<Vector2>();
		foreach(XmlElement point in polyPoints.SelectNodes("point")) {
			float x;
			float y;
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

	static string listToString(List<Vector2> list) {
		string str = "";
		foreach (Vector2 v in list) {
			str += "x:" + v.x + "/y:" + v.y + "_";
		}
		return str;
	}

}
