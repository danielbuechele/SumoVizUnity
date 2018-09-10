using UnityEngine;
using System.IO;
using System.IO.Compression;
using System.Xml;

public class TrajectoryLoader {

	public bool forceStartAtZero = true;
	private bool timeSubstractTaken = false;

	public void loadTrajectories(string resFolderPath) {

        new GameObject("Pedestrians");

        string outFolder = Path.Combine(resFolderPath, "out");
        string simXmlFilePath = Path.Combine(outFolder, "sim.xml");
        XmlDocument simXmlDoc = new XmlDocument();
        simXmlDoc.LoadXml(utils.loadFileIntoEditor(simXmlFilePath));
        XmlNode output = simXmlDoc.SelectSingleNode("//output");

        PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

        foreach (XmlElement floorCsvAtEl in output.SelectNodes("floor")) {
            string trajFilePath = Path.Combine(outFolder, floorCsvAtEl.GetAttribute("csvAt"));
            //if (trajFilePath.EndsWith(".gz")) // TODO

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
                        pl.addPedestrianPosition(new PedestrianPosition(id, time - timeSubtract, x, y, z));
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
            }

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
