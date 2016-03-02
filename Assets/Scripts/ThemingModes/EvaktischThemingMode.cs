using UnityEngine;
using System.Collections;

public class EvaktischThemingMode : ThemingMode {
	#region ThemingMode implementation

	public override string getTerrainName() {
		return "TerrainCity";
	}

	public override Material getWallsMaterial () {
		return (Material) Resources.Load("Woodbox", typeof(Material));
	}

	public override Material getBoxMaterial () {
		return null;
	}

	public override Material getHouseMaterial () {
		return null;
	}

	public override Material getRoofMaterial () {
		return null;
	}

	public override Vector2 getTextureScaleForHeight (float height) {
		return new Vector2();
	}

	#endregion
}
