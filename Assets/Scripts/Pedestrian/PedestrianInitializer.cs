using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Collections.Generic;
using System;

public class PedestrianInitializer : MonoBehaviour {

    private bool forceStartAtZero = false;
    private bool timeSubstractTaken = false;
    private Dictionary<int, Pedestrian> peds = new Dictionary<int, Pedestrian>();

    // get pedestrian prefab
    public GameObject pedPrefab;

    public bool initializePeds(string resFolderPath, SimData simData) {

        string outFolder = Path.Combine(resFolderPath, "out");
        string simXmlFilePath = Path.Combine(outFolder, "sim.xml");
        XmlDocument simXmlDoc = new XmlDocument();
        try {
            simXmlDoc.LoadXml(utils.loadFileIntoEditor(simXmlFilePath));
        } catch (XmlException ex) {
            Debug.Log("no results for this scenario have been found");
            return false;

        }
        XmlNode output = simXmlDoc.SelectSingleNode("//output");

        GameObject pedestrians = new GameObject("Pedestrians");
        simData.setPedestrianGameObject(pedestrians);

        foreach (XmlElement floorCsvAtEl in output.SelectNodes("floor")) {
            string trajFile = floorCsvAtEl.GetAttribute("csvAt");
            string trajFilePath = Path.Combine(outFolder, trajFile);
            string floorName = trajFile.Replace("floor-", "");
            floorName = floorName.Substring(0, floorName.IndexOf("."));
            Floor floor = simData.getFloor(floorName);

            StreamReader reader;
            if (trajFile.EndsWith(".csv")) {
                reader = new StreamReader(trajFilePath);
                readFile(reader, floor, pedestrians, simData);
            } else {
                // via https://stackoverflow.com/a/29372751
                using (FileStream fs = File.OpenRead(trajFilePath))
                using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, true))
                using (reader = new StreamReader(zip)) {
                    //while (!unzip.EndOfStream) {}
                    //using (reader) { // fs: a bit complicated, but this should cope even with totally inconsistent line endings
                    readFile(reader, floor, pedestrians, simData);

                }
            }
        }
        return true;
    }

    private void readFile(StreamReader reader, Floor floor, GameObject pedestrians, SimData simData) {
        string line = reader.ReadLine(); // skip the 1st line = header
        line = reader.ReadLine(); // 2nd line
        float timeSubtract = 0;
        if (forceStartAtZero && !timeSubstractTaken) {
            float.TryParse(line.Split(',')[0], out timeSubtract);
            timeSubstractTaken = true;
        }

        while (line != null) {
            string[] values = line.Split(',');
            if (values.Length >= 4) {
                float time;
                int id;
                float x, y, z;
                float.TryParse(values[0], out time);
                int.TryParse(values[1], out id);
                float.TryParse(values[2], out x);
                float.TryParse(values[3], out y);
                // if no z coordinate is given, just take zero as default
                try {
                    float.TryParse(values[4], out z);
                } catch (IndexOutOfRangeException e) {
                    z = 0.0f;
                }
                Pedestrian ped = createPedestrian(id, new PedestrianPosition(floor.level, time - timeSubtract, x, y, z), pedestrians.transform, simData);
            }
            line = reader.ReadLine();
        }
        reader.Close();
    }


    public Pedestrian createPedestrian(int pedID, PedestrianPosition pos, Transform parent, SimData simData) {
        int id = pedID;
        Pedestrian ped = null;
        peds.TryGetValue(id, out ped);
        GameObject newPedGameObj;
        if (ped == null) {
//            newPedGameObj = (GameObject)Instantiate(Resources.Load(pedPrefab.ToString()));

            newPedGameObj = Instantiate(pedPrefab, pedPrefab.transform.position, pedPrefab.transform.rotation);
//            newPedGameObj.transform.rotation = pedPrefab.transform.rotation;
            // TODO: set different heights
            //           float height = 0.8f + Random.value * 0.8f;
            //           newPedGameObj.transform.localScale = new Vector3(1, height, 1);
            //           GameObject newPedGameObj = (GameObject)Instantiate(Resources.Load(pedestrianModel.ToString()));
            ped = newPedGameObj.GetComponent<Pedestrian>();
            ped.init(id, pos);
            peds.Add(id, ped);
            newPedGameObj.SetActive(false);
            newPedGameObj.transform.SetParent(parent);
        } else {
            ped.addPos(pos);
        }

        if (pos.getTime() > simData.getMaxTime())
            simData.setMaxTime(pos.getTime());

        return ped;
    }

    internal void Reset() {
        peds = new Dictionary<int, Pedestrian>();
    }
}
