using UnityEngine;

public abstract class ThemingMode : Object {

	private GameObject terrain;
	public GameObject getTerrain() {
		if (terrain == null) terrain = GameObject.Instantiate(Resources.Load(getTerrainName())) as GameObject; 
		return terrain;
	}

	public abstract string getTerrainName();
	public abstract Material getWallsMaterial();
	public abstract Material getBoxMaterial();
	public abstract Material getHouseMaterial();
	public abstract Material getRoofMaterial();
	public abstract Vector2 getTextureScaleForHeight (float height);
}
