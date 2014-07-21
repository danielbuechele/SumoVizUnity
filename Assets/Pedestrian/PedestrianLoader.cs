using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PedestrianLoader : MonoBehaviour {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	public List<GameObject> pedestirans = new List<GameObject>();
	public int[] population;

	// Use this for initialization
	void Start () {

	}

	public void addPedestrianPosition(PedestrianPosition p) {
		positions.Add (p);
		PlaybackControl pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
		if (p.getTime ()>pc.total_time) pc.total_time = p.getTime ();
	}

	public void createPedestrians() {
		PlaybackControl pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
		SortedList currentList = new SortedList ();
		population = new int[(int)pc.total_time+1];

		for (int i = 0; i< positions.Count;i++) {
			currentList.Add (positions[i].getTime (),positions[i]);
			population[(int) positions[i].getTime ()]++;
			if ((i == (positions.Count-1) || positions[i].getID()!=positions[i+1].getID()) && currentList.Count>0) {

				GameObject p = (GameObject) Instantiate(Resources.Load("Pedestrian"));
				p.transform.parent = null;
				p.GetComponent<Pedestrian>().setPositions(currentList);
				p.GetComponent<Pedestrian>().setID(positions[i].getID());
				pedestirans.Add(p);
				currentList.Clear();
			}
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
