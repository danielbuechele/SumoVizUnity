using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml;

public class FileLoaderXML {

	// Load an XML file containing both, geometry and pedestrian positions
	// 'filename' must contain the absolute path (I think.)
	public void loadXMLFile(string filename) {
		if (!System.IO.File.Exists(filename)) {
			Debug.LogError("Error: File " + filename + " not found.");
		    return;
		}

		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(System.IO.File.ReadAllText(filename));

		// Load geometry
		GeometryLoader gl = GameObject.Find("GeometryLoader").GetComponent<GeometryLoader>();
		gl.setTheme (new NatureThemingMode ());

		XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");
		foreach(XmlElement floor in spatial.SelectNodes("floor")) { // TODO: load different floors..
			float height = TryParseWithDefault.ToSingle(floor.GetAttribute("height"), 1.0f);

			foreach (XmlElement geomObj in floor.SelectNodes("object")) {
				switch (geomObj.GetAttribute("type")) {

				case "openWall":
					//WallExtrudeGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj), height, -0.2f);
					break;

				case "wall":
					//ObstacleExtrudeGeometry.create(geomObj.GetAttribute("name"), parsePoints(geomObj), height);
					break;

				case "origin":
				case "destination":
				case "scaledArea":
				case "waitingZone":
				case "portal":
				case "beamExit":
				case "eofWall":
				case "queuingArea":
//					AreaGeometry.createOriginTarget(geomObj.GetAttribute("name"), parsePoints(geomObj));
					break;

				default:
					Debug.Log("Warning: XML geometry parser: Don't know how to parse Object of type '" + geomObj.GetAttribute("type") + "'.");
					break;
				}
			}
		}
		

		// Load pedestrians
		XmlNode output = xmlDoc.SelectSingleNode("//output");
	
		if (output == null) {
			Debug.Log("Debug: No output / pedestrian position data found in file.");
			return;
		}

		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
		foreach(XmlElement floor in output.SelectNodes("floor")) { // TODO: load different floors..
			// a bit complicated, but this should cope even with totally inconsistent line endings:
			using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(floor.InnerText)))) {
				string line;
				while((line = reader.ReadLine()) != null) {
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
						//pl.addPedestrianPosition(new PedestrianPosition(id,time,x,y));
					}
				}
			}
		}
		pl.createPedestrians ();
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
