using UnityEngine;
using System.Collections;

public class TrajectoryLoader : MonoBehaviour {

	public TrajectoryLoader(string filepath) {
		loadTrajectoryFile (filepath);
	}

	public void loadTrajectoryFile(string filepath) {
		/*
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
						pl.addPedestrianPosition(new PedestrianPosition(id,time,x,y));
					}
				}
			}
		}
		pl.createPedestrians ();
		*/
	}
		
}
