using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeometryLoader : MonoBehaviour {

	public List<Geometry> geometry;
	public ThemingMode theme;
	public Groundplane groundplane;

	public void setTheme(ThemingMode mode) {
		theme = mode;
		groundplane = theme.getTerrain().GetComponent<Groundplane> ();
	}

	public void addObject(Geometry obj) {
		geometry.Add(obj);
	}
}
