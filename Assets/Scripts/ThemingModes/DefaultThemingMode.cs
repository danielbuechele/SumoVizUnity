using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DefaultThemingMode : ThemingMode {
	#region ThemingMode implementation


	List<Material> solidColorMaterials = new List<Material>();

	public DefaultThemingMode() {
		solidColorMaterials.Add ((Material) Resources.Load ("SolidColorMaterials/White", typeof(Material)));
		solidColorMaterials.Add ((Material) Resources.Load ("SolidColorMaterials/Gelb", typeof(Material)));
		solidColorMaterials.Add ((Material) Resources.Load ("SolidColorMaterials/Green", typeof(Material)));
		solidColorMaterials.Add ((Material) Resources.Load ("SolidColorMaterials/Red", typeof(Material)));
		solidColorMaterials.Add ((Material) Resources.Load ("SolidColorMaterials/Blue", typeof(Material)));
	}

	public override string getTerrainName() {
		return "TerrainCity";
	}

	private Material getRandomSolidColorMaterial() {
		return solidColorMaterials [Random.Range (0, solidColorMaterials.Count)];
	}

	public override Material getOpenWallMaterial () {
		return (Material) Resources.Load ("SolidColorMaterials/Silver", typeof(Material));
	}

	public override Material getWallMaterial () {
		return getRandomSolidColorMaterial ();
	}

	public override Material getBoxMaterial () {
		return null;
	}

	public override Material getHouseMaterial () {
		return (Material) Resources.Load("House", typeof(Material));
	}

	public override Material getRoofMaterial () {
		return (Material) Resources.Load("Roof", typeof(Material));
	}

	public override Vector2 getTextureScaleForHeight (float height) {
		float y = 0;
		if (height < 5) 
			y = height / 0.44f;
		else if 
			(height < 7) y = height / 0.67f;
		else {
			int fulltextures = (int) height / 7;
			y = height / fulltextures;
		}
		return new Vector2 (0.5f, 1 / y);
	}

	#endregion
}
