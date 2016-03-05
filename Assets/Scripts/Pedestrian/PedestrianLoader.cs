using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PedestrianLoader : MonoBehaviour {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	//public List<GameObject> pedestrians = new List<GameObject>();
	//public int[] population;
	private PlaybackControl pc;
	public List<Pedestrian> pedestrians = new List<Pedestrian> ();

	public float pedScaleFactor = 0.9f; // 1 is default size


	void Start() {
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
	}

	public void addPedestrianPosition(PedestrianPosition p) {
		positions.Add (p);
		if (p.getTime () > pc.total_time) 
			pc.total_time = p.getTime ();
	}

	public void createPedestrians() {
		pedestrians.Clear ();
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
		SortedList currentList = new SortedList ();
		//population = new int[(int) pc.total_time + 1];

		for (int i = 0; i < positions.Count; i ++) {
			if (positions.Count() > (i + 1) && positions[i].getX() == positions[i + 1].getX() && positions[i].getY() == positions[i + 1].getY()) {
				// Only take into account time steps with changed coordinates. We want smooth animation.
				continue;
			}
			decimal timestamp = positions[i].getTime ();
			if(!currentList.Contains(timestamp)) // temp workaround part 1
				currentList.Add(timestamp, positions[i]);
			//population[(int) positions[i].getTime ()] ++;
			if ((i == (positions.Count - 1) || positions[i].getID() != positions[i + 1].getID()) && currentList.Count > 0) {
				GameObject p = (GameObject) Instantiate(Resources.Load("Pedestrian"));

				//pedScaleFactor = Random.value;
				if(pedScaleFactor != 1f)
					p.transform.localScale = new Vector3 (pedScaleFactor, pedScaleFactor, pedScaleFactor);

				setPedestriansAsParent (p);
				Pedestrian ped = p.GetComponent<Pedestrian> ();
				ped.setPositions(currentList);
				ped.setID(positions[i].getID());
				pedestrians.Add(ped);
				currentList.Clear();
			}
		}
	}

	private void setPedestriansAsParent (GameObject obj){
		obj.transform.SetParent (GameObject.Find ("Pedestrians").transform);
	}
}
