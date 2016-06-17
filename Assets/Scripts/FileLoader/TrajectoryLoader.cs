using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;

public class TrajectoryLoader {

	private StreamReader reader;


	public TrajectoryLoader(string trajFilePath) {
		//FileInfo fi = new FileInfo (utils.getStreamingAssetsPath (relativeTrajFilePath));

		FileInfo fi = new FileInfo (trajFilePath);
		reader = fi.OpenText ();

		/*
		if (properStreamReading) {
			FileInfo fi = new FileInfo ("Assets/Resources/Data/" + relativeTrajFilePath); //_ignore
			reader = fi.OpenText ();
		} else {
			string filedata = utils.loadFileAtRuntimeIntoBuild ("Data/" + relativeTrajFilePath);
			reader = new StreamReader (new MemoryStream (Encoding.UTF8.GetBytes (filedata)));
		}*/
	}

	public bool forceStartAtZero = true;
	private bool timeSubstractTaken = false;


	public void loadTrajectories() {
		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();
		decimal timeSubtract = 0;

		using (reader) { // fs: a bit complicated, but this should cope even with totally inconsistent line endings
			string line = reader.ReadLine(); // skip the 1st line = header
			line = reader.ReadLine(); // 2nd line

			if (forceStartAtZero && !timeSubstractTaken) {
				decimal.TryParse(line.Split(',')[0], out timeSubtract);
				timeSubstractTaken = true;
			}

			while(line != null) {
				string[] values = line.Split(',');
				if (values.Length >= 4) {
					decimal time;
					int id;
					float x, y;
					decimal.TryParse(values[0], out time);
					int.TryParse(values[1], out id);
					float.TryParse(values[2], out x);
					float.TryParse(values[3], out y);
					pl.addPedestrianPosition(new PedestrianPosition(id, time - timeSubtract, x, y));
				}
				line = reader.ReadLine ();
			}
			reader.Close ();
		}
		//TODO find mechanism to not show peds (for quick camera-tour dev)
		//pl.createPedestrians ();
		pl.init ();
	}
		
}
