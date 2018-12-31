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
    private ScenarioLoader sl;
    //	private List<Pedestrian> pedestrians = new List<Pedestrian> ();
    //public float pedScaleFactor = 1f; // 1 is default size

    // creates a dropdown choice in the inspector
    public enum PedModel {
        ConePed
    }
    public PedModel pedestrianModel = PedModel.ConePed;

    private Dictionary<int, Pedestrian> peds = new Dictionary<int, Pedestrian>();


    void Start() {
        //pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControl>();
        //sl = GameObject.Find("ScenarioLoader").GetComponent<ScenarioLoader>();
    }

    public List<Pedestrian> getPedestrians() {
        return peds.Values.ToList<Pedestrian>();
    }

    public Pedestrian createPedestrian(int pedID, PedestrianPosition pos, Transform parent) {
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

        if (pos.getTime() > sl.getSimData().getMaxTime())
            sl.getSimData().setMaxTime(pos.getTime());

        // TODO: old code
        //positions.Add (pos);
        //if (pos.getTime() > pc.total_time)
        //    pc.total_time = pos.getTime();

        return ped;
    }

    internal void reset() {
        peds = new Dictionary<int, Pedestrian>();
        // reset the playback control in case a scenario was loaded with different time
        pc.Reset();

    }

    public void init() {
        // remove all peds that only have one position
        List<int> removeList = new List<int>();
        foreach (KeyValuePair<int, Pedestrian> ped in peds)
            if (ped.Value.getPositionsCount() < 2)
                removeList.Add(ped.Key);
        foreach (int id in removeList)
            peds.Remove(id);
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
}
