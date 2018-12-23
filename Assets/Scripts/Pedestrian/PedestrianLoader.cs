using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PedestrianLoader : MonoBehaviour {

    //private List<PedestrianPosition> positions = new List<PedestrianPosition>();
    //public List<GameObject> pedestrians = new List<GameObject>();
    //public int[] population;

    // this is the prefab we are using to render our pedestrians - is set via the inspector
    public GameObject pedPrefab;

    private PlaybackControl pc;
//	private List<Pedestrian> pedestrians = new List<Pedestrian> ();
	//public float pedScaleFactor = 1f; // 1 is default size

	// creates a dropdown choice in the inspector
	public enum PedModel {
		ConePed
	}
	public PedModel pedestrianModel = PedModel.ConePed;

	private Dictionary<int, Pedestrian> peds = new Dictionary<int, Pedestrian> ();


	void Start() {
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
	}

	public List<Pedestrian> getPedestrians (){
		return peds.Values.ToList<Pedestrian>();
	}

	public void addPedestrianPosition(PedestrianPosition pos) {


        int id = pos.getID ();
		Pedestrian ped = null;
		peds.TryGetValue (id, out ped);

		if (ped == null) {
            GameObject newPedGameObj = Instantiate (pedPrefab, transform.position, Quaternion.identity);
 //           float height = 0.8f + Random.value * 0.8f;
 //           newPedGameObj.transform.localScale = new Vector3(1, height, 1);
            //           GameObject newPedGameObj = (GameObject)Instantiate(Resources.Load(pedestrianModel.ToString()));
            setPedestriansAsParent(newPedGameObj);
			ped = newPedGameObj.GetComponent<Pedestrian> ();
			ped.init (id, pos);
			peds.Add (id, ped);
            newPedGameObj.SetActive(false);
            // TODO remove this and go through Dictionary if all peds are needed elsewhere? performance?
		} else {
			ped.addPos (pos);
		}

		//positions.Add (pos);
		if (pos.getTime () > pc.total_time) 
			pc.total_time = pos.getTime ();
	}

    internal void reset()
    {
        peds = new Dictionary<int, Pedestrian>();
        // reset the playback control in case a scenario was loaded with different time
        pc.Reset();

    }

    public void init() {
        //peds[0].dev();
		List<int> removeList = new List<int> ();
		foreach(KeyValuePair<int, Pedestrian> ped in peds)
			if (ped.Value.getPositionsCount () < 2)
				removeList.Add (ped.Key);
		foreach (int id in removeList)
			peds.Remove (id);
	}


	public void createPedestrians() {
		/*
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
		*/
	}

	private void setPedestriansAsParent (GameObject obj){
		obj.transform.SetParent (GameObject.Find ("Pedestrians").transform);
	}
}
