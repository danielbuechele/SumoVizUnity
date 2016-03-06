﻿using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;

public class TrajectoryLoader {

	private string filedata;


	public TrajectoryLoader(string filepath) {
		filedata = utils.loadFileAtRuntimeIntoBuild (filepath);
	}

	public void loadTrajectories() {
		PedestrianLoader pl = GameObject.Find("PedestrianLoader").GetComponent<PedestrianLoader>();

		using (StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(filedata)))) { // fs: a bit complicated, but this should cope even with totally inconsistent line endings
			string line = reader.ReadLine(); // skip the first line = header
			while((line = reader.ReadLine()) != null) {
				string[] values = line.Split(',');
				if (values.Length == 7) { // TODO will it always be 7?
					decimal time;
					int id;
					float x, y;
					decimal.TryParse(values[0], out time);
					int.TryParse(values[1], out id);
					float.TryParse(values[2], out x);
					float.TryParse(values[3], out y);
					pl.addPedestrianPosition(new PedestrianPosition(id, time, x, y));
				}
			}
		}
		pl.createPedestrians ();
	}
		
}
