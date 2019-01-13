using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GeometryLoader : MonoBehaviour {

    // set via Inspector
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject treadPrefab;

    [HideInInspector]
	private ThemingMode theme;
	[HideInInspector]
	public Groundplane groundplane;

    internal GameObject getObstaclePrefab() {
        return obstaclePrefab;
    }

    internal GameObject getTreadPrefab() {
        return treadPrefab;

    }

    public void setTheme(ThemingMode mode) {
		this.theme = mode;
		GameObject terrain = theme.getTerrain ();
		groundplane = terrain.GetComponent<Groundplane> ();
		setWorldAsParent (terrain);
	}

	public void setWorldAsParent (GameObject obj){
		obj.transform.SetParent (GameObject.Find ("World").transform);
	}

    internal ThemingMode getTheme() {
        return theme;
    }
}
