using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class PedestrianLoader : MonoBehaviour {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	//public List<GameObject> pedestrians = new List<GameObject>();
	//public int[] population;
	private PlaybackControl pc;
	private List<Pedestrian> pedestrians = new List<Pedestrian> ();
	//public float pedScaleFactor = 1f; // 1 is default size

	// creates a dropdown choice in the inspector
	public enum PedModel {
		Pedestrian, LOD_Ped
	}
	public PedModel pedestrianModel = PedModel.Pedestrian;

	private Dictionary<int, GameObject> peds = new Dictionary<int, GameObject> ();


	void Start() {
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
	}

	public List<Pedestrian> getPedestrians (){
		return pedestrians;
	}

	public void addPedestrianPosition(PedestrianPosition pedPos) {
		/*
		GameObject pedGameObj = null;
		peds.TryGetValue (pedPos.getID (), out pedGameObj);

		if(pedGameObj != null) {
			Pedestrian pedScript = pedGameObj.GetComponent<Pedestrian> ();	
			pedScript.handlePedPos (pedPos); //TODO
		} else {
			pedGameObj = (GameObject) Instantiate (Resources.Load (pedestrianModel.ToString ()));
			setPedestriansAsParent (pedGameObj);
			Pedestrian pedScript = pedGameObj.GetComponent<Pedestrian> ();	
			pedestrians.Add (pedScript);
			peds.Add(pedPos.getID(), pedGameObj);
		}
		*/
		positions.Add (pedPos);
		if (pedPos.getTime () > pc.total_time) 
			pc.total_time = pedPos.getTime ();
	}

	public void createPedestrians() {
		//pc.total_time += 1; // ~bd: attempt to avoid the while-loop in pedestrian update() ever to grab an index thats beyond positions

		pedestrians.Clear ();
		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
		SortedList currentList = new SortedList ();
		//population = new int[(int) pc.total_time + 1];

		//TODO try to get rid of SortedList here as well by using HashMaps and passing the entries directly to the respective Peds to handle them

		for (int i = 0; i < positions.Count; i ++) {
			if (positions.Count() > (i + 1) && positions[i].getX() == positions[i + 1].getX() && positions[i].getY() == positions[i + 1].getY()) {
				// Only take into account time steps with changed coordinates. We want smooth animation.
				continue;
			}
			decimal timestamp = positions[i].getTime();
			if(!currentList.Contains(timestamp)) // temp workaround: output error kernel issue #116
				currentList.Add(timestamp, positions[i]);
			//population[(int) positions[i].getTime ()] ++;
			if (currentList.Count > 0
				&& (i == (positions.Count - 1) || positions[i].getID() != positions[i + 1].getID())) {
				if (currentList.Count > 1) { // don't instantiate peds that stay in the same place for the entire total_time, they will throw index out of bound arrows in the index-while-loop in Pedestrian
					GameObject p = (GameObject) Instantiate (Resources.Load (pedestrianModel.ToString ()));
					//if(pedScaleFactor != 1f) p.transform.localScale = new Vector3 (pedScaleFactor, pedScaleFactor, pedScaleFactor);
					setPedestriansAsParent (p);
					Pedestrian ped = p.GetComponent<Pedestrian> ();	

					List<PedestrianPosition> list = new List<PedestrianPosition> ();
					foreach (PedestrianPosition pedPos in currentList.Values)
						list.Add (pedPos);
					
					ped.setPositions (list);
					ped.setID (positions [i].getID ());
					pedestrians.Add (ped);
				}
				currentList.Clear ();
			}
		}
	}

	private void setPedestriansAsParent (GameObject obj){
		obj.transform.SetParent (GameObject.Find ("Pedestrians").transform);
	}
}
