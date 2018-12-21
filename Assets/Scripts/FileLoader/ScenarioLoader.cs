using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;

public class ScenarioLoader {

    private XmlDocument xmlDoc = new XmlDocument();
    private SimData simData = new SimData();

    public ScenarioLoader() {}

    public void loadScenario(string crowditFilePath, string resFolderPath) {

        xmlDoc.LoadXml(utils.loadFileIntoEditor(crowditFilePath));

        // store morphosis entries
        Dictionary<string, XmlElement> wunderZoneIdToMorphosisEntry = new Dictionary<string, XmlElement>();
        XmlNode morphosis = xmlDoc.SelectSingleNode("//morphosis");
        foreach (XmlElement morphosisEntry in morphosis) {
            string wunderZoneId = morphosisEntry.GetAttribute("wunderZone");
            wunderZoneIdToMorphosisEntry[wunderZoneId] = morphosisEntry;
        }

        // store floorProps entries
        Dictionary<string, XmlElement> floorPropsEntries = new Dictionary<string, XmlElement>();
        XmlNode floorProps = xmlDoc.SelectSingleNode("//floorProps");
        foreach (XmlElement floorPropEl in floorProps) {
            string floorId = floorPropEl.GetAttribute("floor");
            floorPropsEntries[floorId] = floorPropEl;
        }

        // extract paths to .floor files and parse their content
        XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");
        int level = 0;

        List<Floor> floors = new List<Floor>();

        // first, create floors
        foreach (XmlElement floorEl in spatial.SelectNodes("floor"))
        {
            string floorId = floorEl.GetAttribute("id");
            Floor floor = new Floor(floorId, simData);
            float elevation = float.Parse(floorPropsEntries[floorId].GetAttribute("elevation"));
            float height = float.Parse(floorPropsEntries[floorId].GetAttribute("height"));
            floor.setMetaData(level++, height, elevation);
            simData.addFloor(floor);

            floors.Add(floor);

        }

        // now, populate floors
        level = 0;
        List<Elevator> elevators = new List<Elevator>();

        foreach (XmlElement floorEl in spatial.SelectNodes("floor")) {
            string floorId = floorEl.GetAttribute("id");
            string floorAtFullPath = Path.Combine(resFolderPath, floorEl.GetAttribute("floorAt"));
            XmlDocument floorXmlDoc = new XmlDocument();
            floorXmlDoc.LoadXml(utils.loadFileIntoEditor(floorAtFullPath));

            Floor floor = floors[level++];

            resetFloorBoundingValues();

            XmlNode root = floorXmlDoc.SelectSingleNode("//floor");
            foreach (XmlElement layerEl in root.SelectNodes("layer")) {
                // WUNDERZONES
                foreach (XmlElement wunderZoneEl in layerEl.SelectNodes("wunderZone")) {
                    string wunderZoneId = wunderZoneEl.GetAttribute("id");
                    if (!wunderZoneIdToMorphosisEntry.ContainsKey(wunderZoneId)) {
                        continue;
                    }
                    XmlElement morphosisEntry = wunderZoneIdToMorphosisEntry[wunderZoneId];
                    WunderZone actualization = null;
                    switch (morphosisEntry.Name) {
                        case "destination":
                            actualization = new Destination();
                            break;
                        case "directedScaledArea":
                            actualization = new DirectedScaledArea();
                            break;
                        case "elevator":
                            actualization = new Elevator();
                            elevators.Add((Elevator)actualization);
                            break;
                        case "escalator":
                            actualization = new Escalator();
                            break;
                        case "origin":
                            actualization = new Origin();
                             break;
                        case "portal":
                            actualization = new Portal();
                            break;
                        case "queueingArea":
                            actualization = new QueueingArea();
                            break;
                        case "scaledArea":
                            actualization = new ScaledArea();
                            break;
                        case "stair":
                            actualization = new Stair();
                            break;
                        case "waitingZone":
                            actualization = new WaitingZone();
                            break;
                        default:
                            Debug.LogWarning("Scenario parser: unknown geometry element type " + morphosisEntry.Name);
                            break;
                    }

                    if (actualization == null) {
                        continue;
                    }
                    actualization.setFloor(floor);
                    actualization.setId(morphosisEntry.GetAttribute("id"));
                    actualization.setLayerId(layerEl.GetAttribute("id"));
                    actualization.setMorphosisEntry(morphosisEntry);
                    actualization.setWunderZoneId(wunderZoneId);
                    actualization.setPoints(parsePoints(wunderZoneEl));

                    floor.addWunderZone(actualization);
                    simData.addWunderZoneToMap(wunderZoneId, actualization);
                }


                // WALLS
                foreach (XmlElement wallEl in layerEl.SelectNodes("wall")) {
                    Wall wall = null;
                    if (wallEl.GetAttribute("closed") == "true") { // via stackoverflow.com/a/9742736
                        wall = new ClosedWall();
                    } else {
                        wall = new OpenWall();
                    }
                    wall.setFloor(floor);
                    wall.setId(wallEl.GetAttribute("id"));
                    wall.setLayerId(layerEl.GetAttribute("id"));
                    wall.setPoints(parsePoints(wallEl));

                    floor.addWall(wall);
                }
            }


        }

        XmlNode elevatorMatrices = xmlDoc.SelectSingleNode("//elevatorMatrices");

        foreach (XmlElement elevatorMatrix in elevatorMatrices)
        {
            foreach (Elevator elev in elevators)
            {
                string elevatorID = elevatorMatrix.GetAttribute("ref");
                if (elevatorID.Equals(elev.getId()))
                {

                    string[] floorIDs = elevatorMatrix.GetAttribute("floors").Split(',');
                    foreach (String floorID in floorIDs)
                    {
                        elev.addFloor(simData.getFloor(floorID));
                    }
                }

            }
        }

        foreach (Floor floor in floors)
        {
            // create 3D objects
            floor.setBoundingPoints(getBoundingPoints());
            floor.createObjects();
        }
    }

    float minX, maxX, minY, maxY;

    private void resetFloorBoundingValues() {
        minX = float.MaxValue;
        maxX = 0;
        minY = float.MaxValue;
        maxY = 0;
    }

    // Parse an XmlElement full of <point> XmlElements into a coordinate list 
    private List<Vector2> parsePoints(XmlElement polyPoints) {
        List<Vector2> list = new List<Vector2>();
        foreach (XmlElement point in polyPoints.SelectNodes("point")) {
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
            if (y > maxY)
                maxY = y;
        }
        return list;
    }

    public List<float> getBoundingPoints() {
        return new List<float>() { minX, minY, maxX, maxY };
    }

    /*
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
		
	public void loadScenarioOld() {
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

		Debug.Log ("minX: " + minX);
		Debug.Log ("maxX: " + maxX);
		Debug.Log ("minY: " + minY);
		Debug.Log ("maxY: " + maxY);
	}

	static string listToString(List<Vector2> list) {
		string str = "";
		foreach (Vector2 v in list) {
			str += "x:" + v.x + "/y:" + v.y + "_";
		}
		return str;
	}
	*/
}
