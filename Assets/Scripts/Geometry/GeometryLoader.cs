using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GeometryLoader : MonoBehaviour {

    public GameObject obstaclePrefab;
	[HideInInspector]
	private ThemingMode theme;
	[HideInInspector]
	public Groundplane groundplane;


    public void Start()
    {
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
