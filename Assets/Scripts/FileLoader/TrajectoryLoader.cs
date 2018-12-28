using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Collections.Generic;

public class TrajectoryLoader : MonoBehaviour{

	public bool forceStartAtZero = false;
	private bool timeSubstractTaken = false;
    private string resFolderPath;
    private PedestrianLoader pl;
    private RuntimeInitializer ri;
    private ScenarioLoader sl;

    public void Start()
    {
        pl = GetComponent<PedestrianLoader>();
        ri = GameObject.Find("RuntimeInitializer").GetComponent<RuntimeInitializer>();
        sl = GameObject.Find("ScenarioLoader").GetComponent<ScenarioLoader>();
    }

    public void loadTrajectories() {

        resFolderPath = ri.getResFolderPath();

        string outFolder = Path.Combine(resFolderPath, "out");
        string simXmlFilePath = Path.Combine(outFolder, "sim.xml");
        XmlDocument simXmlDoc = new XmlDocument();
        try
        {
            simXmlDoc.LoadXml(utils.loadFileIntoEditor(simXmlFilePath));
        }
        catch (XmlException ex)
        {
            Debug.Log("no results for this scenario have been found");
            return;

        }
        XmlNode output = simXmlDoc.SelectSingleNode("//output");

        pl.reset();
        new GameObject("Pedestrians");

        foreach (XmlElement floorCsvAtEl in output.SelectNodes("floor")) {
            string trajFile = floorCsvAtEl.GetAttribute("csvAt");
            string trajFilePath = Path.Combine(outFolder, trajFile);
            string floorName = trajFile.Replace("floor-", "");
            floorName = floorName.Substring(0, floorName.IndexOf("."));
            Floor floor = sl.getFloor(floorName);

            //GameObject floor = GameObject.Find(floorName);
            List<Pedestrian> pedsPerFloor = new List<Pedestrian>();

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
                        Pedestrian ped = pl.createPedestrian(new PedestrianPosition(id, time - timeSubtract, x, y, z));
                        pedsPerFloor.Add(ped);
                     }
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            floor.addPeds(pedsPerFloor);
        }

		//TODO find mechanism to not show peds (for quick camera-tour dev)
		//pl.createPedestrians ();
		pl.init ();
	}

    /*
    private StreamReader reader;

    public TrajectoryLoader() {
    //FileInfo fi = new FileInfo (utils.getStreamingAssetsPath (relativeTrajFilePath));

    //FileInfo fi = new FileInfo (trajFilePath);
    //reader = fi.OpenText ();


    if (properStreamReading) {
        FileInfo fi = new FileInfo ("Assets/Resources/Data/" + relativeTrajFilePath); //_ignore
        reader = fi.OpenText ();
    } else {
        string filedata = utils.loadFileAtRuntimeIntoBuild ("Data/" + relativeTrajFilePath);
        reader = new StreamReader (new MemoryStream (Encoding.UTF8.GetBytes (filedata)));
    }
    */
}
