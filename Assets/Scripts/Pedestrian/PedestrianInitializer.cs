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

    public void initializePeds(string resFolderPath, SimData simData) {
        string outFolder = Path.Combine(resFolderPath, "out");
        string simXmlFilePath = Path.Combine(outFolder, "sim.xml");
        XmlDocument simXmlDoc = new XmlDocument();
        try {
            simXmlDoc.LoadXml(utils.loadFileIntoEditor(simXmlFilePath));
        } catch (XmlException ex) {
            Debug.Log("no results for this scenario have been found");
            return;

        }
        XmlNode output = simXmlDoc.SelectSingleNode("//output");

        GameObject peds = new GameObject("Pedestrians");
        simData.setPedestrianGameObject(peds);

        foreach (XmlElement floorCsvAtEl in output.SelectNodes("floor")) {
            string trajFile = floorCsvAtEl.GetAttribute("csvAt");
            string trajFilePath = Path.Combine(outFolder, trajFile);
            string floorName = trajFile.Replace("floor-", "");
            floorName = floorName.Substring(0, floorName.IndexOf("."));
            Floor floor = simData.getFloor(floorName);

            decimal timeSubtract = 0;

            // via https://stackoverflow.com/a/29372751
            using (FileStream fs = File.OpenRead(trajFilePath))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, true))
            using (StreamReader reader = new StreamReader(zip)) {
                //while (!unzip.EndOfStream) {}
                //using (reader) { // fs: a bit complicated, but this should cope even with totally inconsistent line endings
                string line = reader.ReadLine(); // skip the 1st line = header
                line = reader.ReadLine(); // 2nd line

                if (forceStartAtZero && !timeSubstractTaken) {
                    decimal.TryParse(line.Split(',')[0], out timeSubtract);
                    timeSubstractTaken = true;
                }

                while (line != null) {
                    string[] values = line.Split(',');
                    if (values.Length >= 4) {
                        decimal time;
                        int id;
                        float x, y, z;
                        decimal.TryParse(values[0], out time);
                        int.TryParse(values[1], out id);
                        float.TryParse(values[2], out x);
                        float.TryParse(values[3], out y);
                        float.TryParse(values[4], out z);
                        Pedestrian ped = createPedestrian(id, new PedestrianPosition(floor.level, time - timeSubtract, x, y, z), peds.transform, simData);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
            }
        }
    }

    public Pedestrian createPedestrian(int pedID, PedestrianPosition pos, Transform parent, SimData simData) {
        int id = pedID;
        Pedestrian ped = null;
        peds.TryGetValue(id, out ped);
        GameObject newPedGameObj;
        if (ped == null) {
            newPedGameObj = Instantiate(pedPrefab, transform.position, Quaternion.identity);
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
