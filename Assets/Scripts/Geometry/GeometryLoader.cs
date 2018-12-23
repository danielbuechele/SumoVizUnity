using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryLoader : MonoBehaviour {

	[HideInInspector]
	public List<Geometry> geometry;
	[HideInInspector]
	public ThemingMode theme;
	[HideInInspector]
	public Groundplane groundplane;

	public void setTheme(ThemingMode mode) {
		this.theme = mode;
		GameObject terrain = theme.getTerrain ();
		groundplane = terrain.GetComponent<Groundplane> ();
		setWorldAsParent (terrain);
	}

	public void setWorldAsParent (GameObject obj){
		obj.transform.SetParent (GameObject.Find ("World").transform);
	}

	public void addObject(Geometry obj) {
		geometry.Add(obj);
	}

    public void Reset()
    {
        geometry.Clear();
    }
}
