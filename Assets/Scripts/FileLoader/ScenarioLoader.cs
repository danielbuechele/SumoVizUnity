using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class ScenarioLoader {

	public ScenarioLoader(string filepath) {
		loadScenario (filepath);
	}
		
	public void loadScenario(string filepath) {
		if (!System.IO.File.Exists(filepath)) {
			Debug.LogError("Error: File " + filepath + " not found.");
			return;
		}

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(System.IO.File.ReadAllText(filepath));

		// Load geometry
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

		XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");
		foreach(XmlElement floor in spatial.SelectNodes("floor")) { // TODO: load different floors..
			float height = TryParseWithDefault.ToSingle(floor.GetAttribute("height"), 1.0f);

			foreach (XmlElement geomObj in floor.SelectNodes("object")) {
				switch (geomObj.GetAttribute("type")) {

				case "openWall":
					WallExtrudeGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj), height, -0.2f);
					break;

				case "wall":
					ObstacleExtrudeGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj), height);
					break;

				case "origin":
				case "destination":
				case "scaledArea":
				case "waitingZone":
				case "portal":
				case "beamExit":
				case "eofWall":
				case "queuingArea":
					AreaGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj));
					break;

				default:
					Debug.Log("Warning: XML geometry parser: Don't know how to parse Object of type '" + geomObj.GetAttribute("type") + "'.");
					break;
				}
			}
		}
	}

	public String getTrajectoryFilePath() {
		return "";	
	}

	// Parse an XmlElement full of <point> XmlElements into a coordinate list 
	static List<Vector2> parsePoints(XmlElement polyPoints) {
		List<Vector2> list = new List<Vector2>();

		foreach(XmlElement point in polyPoints.SelectNodes("point")) {
			float x;
			float y;
			if (float.TryParse(point.GetAttribute("x"), out x) && float.TryParse(point.GetAttribute("y"), out y)) {
				list.Add(new Vector2(x, y));
			}
		}

		return list;
	}
}
