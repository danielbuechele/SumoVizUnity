using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;

public class ScenarioLoader : MonoBehaviour {

    private XmlDocument xmlDoc = new XmlDocument();
    private SimData simData;
    private List<String> floorIDs;
    private List<Floor> floors;
    private Dictionary<string, XmlElement> floorPropsEntries;
    private Dictionary<string, XmlElement> wunderZoneIdToMorphosisEntry;
    private List<Elevator> elevators;


    // TODO: legacy
    public void loadScenario(string crowditFilePath, string resFolderPath, GeometryLoader gl) {
        getScenario(crowditFilePath, resFolderPath, gl);
    }

    public SimData getScenario(string crowditFilePath, string resFolderPath, GeometryLoader gl) {

        // initialize all variables (could be that they are still set from a former scenario)
        init();

        xmlDoc.LoadXml(utils.loadFileIntoEditor(crowditFilePath));

        // store morphosis entries

        XmlNode morphosis = xmlDoc.SelectSingleNode("//morphosis");
        foreach (XmlElement morphosisEntry in morphosis) {
            string wunderZoneId = morphosisEntry.GetAttribute("wunderZone");
            wunderZoneIdToMorphosisEntry[wunderZoneId] = morphosisEntry;
        }

        // store floorProps entries
        XmlNode floorProps = xmlDoc.SelectSingleNode("//floorProps");
        foreach (XmlElement floorPropEl in floorProps) {
            string floorId = floorPropEl.GetAttribute("floor");
            floorPropsEntries[floorId] = floorPropEl;
        }

        // extract paths to .floor files and parse their content
        XmlNode spatial = xmlDoc.SelectSingleNode("//spatial");

        // first, create floors
        parseFloors(spatial, resFolderPath);

        // now, populate floors
        populateFloors(spatial, resFolderPath);

        // create elevator elements
        createElevators(xmlDoc);

        // create game objects for each floor
        foreach (Floor floor in floors) {
            // create 3D objects
            floor.setBoundingPoints(getBoundingPoints());
            floor.createObjects(simData, gl);
        }

        // for camera position
        simData.setBounds();

        return simData;
    }

    internal void init() {
        floors = new List<Floor>();
        floorPropsEntries = new Dictionary<string, XmlElement>();
        wunderZoneIdToMorphosisEntry = new Dictionary<string, XmlElement>();
        elevators = new List<Elevator>();
        xmlDoc = new XmlDocument();
        simData = new SimData();
        floorIDs = new List<String>();
    }

    internal Floor getFloor(string floorName) {
        return simData.getFloor(floorName);
    }

    private void createElevators(XmlDocument xmlDoc) {
        XmlNode elevatorMatrices = xmlDoc.SelectSingleNode("//elevatorMatrices");
        if (elevatorMatrices != null) {
            foreach (XmlElement elevatorMatrix in elevatorMatrices) {
                foreach (Elevator elev in elevators) {
                    string elevatorID = elevatorMatrix.GetAttribute("ref");
                    if (elevatorID.Equals(elev.getId())) {
                        string[] floorIDs = elevatorMatrix.GetAttribute("floors").Split(',');
                        foreach (String floorID in floorIDs) {
                            elev.addFloor(getFloor(floorID));
                        }
                    }
                }
            }
        }
    }

    private void populateFloors(XmlNode spatial, string resFolderPath) {
        int level = 0;
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
                    //    case "destination":
                    //        actualization = new Destination();
                    //        break;
                    //    case "directedScaledArea":
                    //        actualization = new DirectedScaledArea();
                    //        break;
                        case "elevator":
                            actualization = new Elevator();
                            elevators.Add((Elevator)actualization);
                            break;
                        case "escalator":
                            actualization = new Escalator();
                            break;
                        //case "origin":
                        //    actualization = new Origin();
                        //    break;
                        //case "portal":
                        //    actualization = new Portal();
                        //    break;
                        //case "queueingArea":
                        //    actualization = new QueueingArea();
                        //    break;
                        //case "scaledArea":
                        //    actualization = new ScaledArea();
                        //    break;
                        case "stair":
                            actualization = new Stair();
                            break;
                        //case "waitingZone":
                        //    actualization = new WaitingZone();
                        //    break;
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
    }

    private void parseFloors(XmlNode spatial, string resFolderPath) {
        int level = 0;
        foreach (XmlElement floorEl in spatial.SelectNodes("floor")) {
            string floorId = floorEl.GetAttribute("id");
            Floor floor = new Floor(floorId);
            float elevation;
            float height;
            if (!float.TryParse(floorPropsEntries[floorId].GetAttribute("elevation"), out elevation)) {
                 elevation = 2.0f;
            }
            if (!float.TryParse(floorPropsEntries[floorId].GetAttribute("height"), out height)) {
                height = 3f;
            }
            floor.setMetaData(level++, height, elevation);
            simData.addFloor(floor);
            floorIDs.Add(floorId);
            floors.Add(floor);
        }
    }

    internal SimData getSimData() {
        return simData;
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
	}*/
}
