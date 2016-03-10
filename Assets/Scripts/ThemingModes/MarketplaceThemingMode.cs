using UnityEngine;
using System.Collections;

public class MarketplaceThemingMode : ThemingMode {
	#region ThemingMode implementation

	public override string getTerrainName() {
		return "TerrainCity";
	}

	public override Material getOpenWallMaterial () {
		return (Material) Resources.Load("evaktisch/Wand1", typeof(Material));
	}

	public override Material getWallMaterial () {
		return (Material) Resources.Load("evaktisch/Wand2", typeof(Material));
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
