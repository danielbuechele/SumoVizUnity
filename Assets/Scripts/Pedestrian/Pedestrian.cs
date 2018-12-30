using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Pedestrian : MonoBehaviour {
	
	Vector3 start;
	Vector3 target;
	//float movement_time_total;
	//float movement_time_elapsed;
	private float speed;
	//int densityReload;
	//int densityReloadInterval = 10;

	//int id;
	private List<PedestrianPosition> positions = new List<PedestrianPosition> ();
	private Color myColor;
	//bool trajectoryVisible;
	//VectorLine trajectory;

	//private InfoText it;
	//private PedestrianLoader pl;
	private PlaybackControl pc;
	//private Renderer r;
	//private GeometryLoader gl;
	//private Groundplane gp;
	//GameObject tile;

	private AgentView agentView = null;
	#pragma warning disable 108
	private Animation animation;
	#pragma warning restore 108
	private LODGroup lodGroup;
    private int index;
	private bool targetReached = true;

    public void init() {

        //	void Start () {
        gameObject.SetActive(true);
        gameObject.AddComponent<BoxCollider>(); // TODO what for?
		transform.Rotate (0, 90, 0);
		//myColor = new Color (Random.value, Random.value, Random.value);
        gameObject.GetComponentInChildren<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
        //GetComponentInChildren<Renderer>().materials[1].color = myColor;
        //addTile ();
        //it = GameObject.Find ("InfoText").GetComponent<InfoText> ();
        //pl = GameObject.Find ("PedestrianLoader").GetComponent<PedestrianLoader> ();
        pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControl> ();
        //GameObject world = GameObject.Find("World").GetComponent<RuntimeInitializer>();

        animation = GetComponentInChildren<Animation> ();
		lodGroup = GetComponentInChildren<LODGroup>();

  //     if (lodGroup == null) { // "Pedestrian" model
		//	r = GetComponentInChildren<Renderer>() as Renderer;
		//	r.materials [0].color = myColor;
		//} else { // "LOD_Ped" model
		//	Transform PedModelsTransform = transform.GetChild (0).transform;
		//	PedModelsTransform.GetChild (0).GetComponent<Renderer> ().materials [1].color = myColor;
		//	PedModelsTransform.GetChild (1).GetComponent<Renderer> ().materials [1].color = myColor;
		//}
        
		AgentView agentViewComponent = GameObject.Find("CameraMode").GetComponent<AgentView>();
		if (agentViewComponent.enabled)
			agentView = agentViewComponent;

		reset();
	}

    internal int getCurrentFloorID() {
        return positions[index].getFloorID();
    }

    //internal Renderer getRenderer() {
    //    return r;
    //}

    internal void dev() {
        foreach (PedestrianPosition pos in positions) {
            Debug.Log(pos.getTime() + ": " + pos.getX() + ", " + pos.getY() + ", " + pos.getZ());
        }
    }

    public int getPositionsCount() {
		return positions.Count;
	}

	public void init(int id, PedestrianPosition pos) {
		this.name = "Pedestrian_" + id;
        addOrderedPos(pos);
    }

	public void addPos(PedestrianPosition pos) {
		//if(!positions[positions.Count - 1].equals(pos)) // add only if pos is different to the previously added one
		addOrderedPos(pos);
    }

    private void addOrderedPos(PedestrianPosition pos) {
        if (positions.Count == 0) {
            positions.Add(pos);
            return;
        }
        int i = 0;
        while (i < positions.Count && positions[i].getTime() < pos.getTime()) {
            i += 1;
        }
        positions.Insert(i, pos);

    }

	/*
	void OnMouseDown(){
		if (Cursor.lockState != CursorLockMode.None && !trajectoryVisible && !pc.drawLine && hideFlags!=HideFlags.HideInHierarchy) {
			showTrajectory();
		} else if (Cursor.lockState != CursorLockMode.None && trajectoryVisible && !pc.drawLine && hideFlags!=HideFlags.HideInHierarchy) {
			hideTrajectory();
		}
	}

	public void hideTrajectory() {
		VectorLine.Destroy(ref trajectory);
		trajectoryVisible = false;
	}

	public void showTrajectory() {
		VectorLine.SetCamera (GameObject.Find ("Flycam").GetComponent<Camera>());
		
		List <Vector3> points = new List<Vector3>();
		for (int i = 0; i<positions.Count-1; i++) {
			PedestrianPosition a = (PedestrianPosition)positions.GetByIndex (i);
			points.Add (new Vector3 (a.getX (), 0.01f, a.getY ()));
		}
		
		trajectory = VectorLine.SetLine3D (myColor, points.ToArray());
		trajectory.lineWidth = 3.0f;
		pc.trajectoriesShown = true;
		trajectoryVisible = true;
	}
	
	void addTile() {
		float side = 1.0f;
		tile = new GameObject ("tile"+id, typeof(MeshFilter), typeof(MeshRenderer));
		MeshFilter mesh_filter = tile.GetComponent<MeshFilter> ();
		tile.GetComponent<Renderer>().material = (Material) Resources.Load("Tilematerial", typeof(Material));
		tile.GetComponent<Renderer>().material.color = Color.red;
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[] {new Vector3 (-side/2, 0.01f, -side/2),new Vector3 (side/2, 0.01f, -side/2),new Vector3 (-side/2, 0.01f, side/2),new Vector3 (side/2, 0.01f, side/2)};
		mesh.triangles = new int[] {2,1,0,1,2,3};


		Vector2[] uvs = new Vector2[mesh.vertices.Length];
		int i = 0;
		while (i < uvs.Length) {
			uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
			i++;
		}
		mesh.uv = uvs;

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh_filter.mesh = mesh;

		tile.transform.position = gameObject.transform.position;
		tile.transform.parent = gameObject.transform;
	}
	*/

	public void reset() {
		index = 0;
		rendererEnabled (true);
		targetReached = false;
		PedestrianPosition pos = positions[0];
		transform.position = new Vector3 (pos.getX (), pos.getZ(), pos.getY ());
	}

	private void rendererEnabled(bool truefalse) {
		//if (lodGroup == null)
		//	r.enabled = truefalse;
		//else
		//	lodGroup.enabled = truefalse;
		if (truefalse)
			animation.Play ();
	}

	private bool showPed() {
		if (agentView != null)
			return agentView.getCurrentPed () != gameObject;
		return true;
	}

	public void move (decimal currentTime) {
		if (!targetReached) {
			if (showPed ())
				rendererEnabled (true);
			else
				rendererEnabled (false);

			/*
			updateCalls ++;
			float dist = Vector3.Distance (gameObject.transform.position, Camera.main.transform.position);

			[...]

			if (r.enabled) // pc.playing &&
				animation.Play ();
			else
				animation.Stop ();

			int index = _getTrait(positions, pc.current_time);

			if (pc.current_time > positions [index + 1].getTime ()
			    && index != positions.Count - 2) {
				index += 1;
			}
			if (index < positions.Count - 2 && pc.current_time > positions[index + 1].getTime()) {
				index += 1;
			}
			if (index < positions.Count - 1) {
			*/

			while (index <= positions.Count - 2 && currentTime >= positions [index + 1].getTime ()) // && index < positions.Count - 2
			index += 1;

			PedestrianPosition pos = (PedestrianPosition)positions [index];
			PedestrianPosition pos2 = (PedestrianPosition)positions [index + 1];
			start = new Vector3 (pos.getX (), pos.getZ(), pos.getY ()); // the y-coord in Unity is the z-coord from the kernel: the up and down direction
			target = new Vector3 (pos2.getX (), pos2.getZ(), pos2.getY ());
			float time = (float)currentTime;
			float timeStepLength = Mathf.Clamp ((float)pos2.getTime () - (float)pos.getTime (), 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
			float movement_percentage = ((float)time - (float)pos.getTime ()) / timeStepLength;
			Vector3 newPosition = Vector3.Lerp (start, target, movement_percentage);
			Vector3 relativePos = target - start;
			speed = relativePos.magnitude;
			animation ["walking"].speed = speed / timeStepLength;
            // TODO: not needed for cylinders
            //			if (start != target)
//				transform.rotation = Quaternion.LookRotation (relativePos);
			transform.position = newPosition;

			if (index >= positions.Count - 2) { // = target reached
				rendererEnabled (false);
				animation.Stop ();
				targetReached = true;
			}
		}
				/*
				//check if line is crossed

				if (gp.point1active && gp.point2active) {
					if (FasterLineSegmentIntersection(new Vector2(gp.point1.x,gp.point1.z), new Vector2(gp.point2.x,gp.point2.z), new Vector2(transform.position.x, transform.position.z), new Vector2(newPosition.x, newPosition.z))) {
						gp.lineCross(speed);
					}
				}

				//Tile coloring
				if (pc.tileColoringMode != TileColoringMode.TileColoringNone) {

					tile.GetComponent<Renderer>().enabled = true;

					if (pc.tileColoringMode == TileColoringMode.TileColoringSpeed) {
						tile.GetComponent<Renderer>().material.color = ColorHelper.ColorForSpeed(getSpeed());
						//it.updateSpeed(speed);
					} else if (pc.tileColoringMode == TileColoringMode.TileColoringDensity) {
						densityReload = (densityReload+1)%densityReloadInterval;
						if (densityReload==0) {
							getDensity();
						}
						float density = getDensity();
						if (density>=pc.threshold) {
							tile.GetComponent<Renderer>().material.color = ColorHelper.ColorForDensity(density);
						} else {
							tile.GetComponent<Renderer>().enabled = false;
						}
					}
				else {
					tile.GetComponent<Renderer>().enabled = false;
				}
				gameObject.hideFlags = HideFlags.None;
			}
			}
			else {
				showPed = false;
				//r.enabled = false;
				//tile.GetComponent<Renderer>().enabled = false;
				//gameObject.hideFlags = HideFlags.HideInHierarchy;
			}

			//r.enabled = showPed;
			//if (agentView.getCurrentPed() == gameObject)
			//showPed = false;
	  	}
		else
			GetComponent <Animation> ().Stop ();*/
	}

	/*
	public float getDensity() {

		if (hideFlags==HideFlags.HideInHierarchy) return -1;
		int nearbys = 0;
		float radius = 2.0f;
		foreach (GameObject p in pl.pedestrians) {
			if (p!=this && Vector3.Distance(transform.position,p.transform.position)<radius && p.hideFlags!=HideFlags.HideInHierarchy) {
				nearbys++;
			}
		}
		float density = nearbys/(radius*radius*Mathf.PI);
		it.updateDensity(density);
		return density;
	}

	public float getDensityF() {

		if (hideFlags==HideFlags.HideInHierarchy) return -1;
		List<float> nearbys = new List<float>();
		foreach (GameObject p in pl.pedestrians) {
			if (p!=this && p.hideFlags!=HideFlags.HideInHierarchy) {
				float distance = Vector3.Distance(transform.position,p.transform.position);
				if (nearbys.Count == 0) nearbys.Add(distance);
				else if (nearbys[0]>distance) {nearbys.Insert(0,distance);}
				else {nearbys.Add (distance);}
			}
		}
		float density = 8/(nearbys[7]*nearbys[7]*Mathf.PI);
		it.updateDensity(density);

		return density;
	}

	// http://www.stefanbader.ch/faster-line-segment-intersection-for-unity3dc/
	bool FasterLineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {

		Vector2 a = p2 - p1;
		Vector2 b = p3 - p4;
		Vector2 c = p1 - p3;
		
		float alphaNumerator = b.y*c.x - b.x*c.y;
		float alphaDenominator = a.y*b.x - a.x*b.y;
		float betaNumerator  = a.x*c.y - a.y*c.x;
		float betaDenominator  = alphaDenominator; //2013/07/05, fix by Deniz
		
		bool doIntersect = true;
		
		if (alphaDenominator == 0 || betaDenominator == 0) {
			doIntersect = false;
		} else {
			
			if (alphaDenominator > 0) {
				if (alphaNumerator < 0 || alphaNumerator > alphaDenominator) {
					doIntersect = false;
				}
			} else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator) {
				doIntersect = false;
			}
			
			if (doIntersect && betaDenominator > 0) {
				if (betaNumerator < 0 || betaNumerator > betaDenominator) {
					doIntersect = false;
				}
			} else if (betaNumerator > 0 || betaNumerator < betaDenominator) {
				doIntersect = false;
			}
		}
		return doIntersect;
	}

	private int _getTrait(SortedList thisList, decimal thisValue) {
		for (int i = 0; i < thisList.Count; i ++) {
			if ((decimal) thisList.GetKey(i) > thisValue) 
				return i - 1;
		}
		return -1;*/
		/*
		// Check to see if we need to search the list.
		if (thisList == null || thisList.Count <= 0) { return -1; }
		if (thisList.Count == 1) { return 0; }
		
		// Setup the variables needed to find the closest index
		int lower = 0;
		int upper = thisList.Count - 1;
		int index = (lower + upper) / 2;
		
		// Find the closest index (rounded down)
		bool searching = true;
		while (searching)
		{
			int comparisonResult = decimal.Compare(thisValue, (decimal) thisList.GetKey(index));
			if (comparisonResult == 0) { return index; }
			else if (comparisonResult < 0) { upper = index - 1; }
			else { lower = index + 1; }
			Debug.Log (thisValue + " : " + (decimal) thisList.GetKey(index));
			index = (lower + upper) / 2;
			if (lower > upper) { searching = false; }
		}

		// Check to see if we are under or over the max values.
		if (index >= thisList.Count - 1) { return thisList.Count - 1; }
		if (index < 0) { return 0; }
		
		// Check to see if we should have rounded up instead
		//if (thisList.Keys[index + 1] - thisValue < thisValue - (thisList.Keys[index])) { index++; }
		
		// Return the correct/closest string
		return index;
	//}

	public void setID(int id) {
		//this.id = id;
		//densityReload = id % densityReloadInterval;
		this.name = "Pedestrian " + id;
	}

	public void setPositions(List<PedestrianPosition> p) { // comes in sorted here
		//positions.Clear();
		positions = p;
		PedestrianPosition pos = (PedestrianPosition) p[0];
		transform.position = new Vector3 (pos.getX(), 0, pos.getY());
	}
	*/	
}
