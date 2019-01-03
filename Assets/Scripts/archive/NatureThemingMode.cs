using UnityEngine;
using System.Collections;

public class NatureThemingMode : ThemingMode {
	#region ThemingMode implementation

	public override string getTerrainName() {
		return "TerrainNature";
	}

	public override Material getOpenWallMaterial () {
		return (Material) Resources.Load("WallNature", typeof(Material));
	}

	public override Material getWallMaterial () {
		return (Material) Resources.Load("WallNature", typeof(Material));
	}

	public override Material getBoxMaterial () {
		return (Material) Resources.Load("Woodbox", typeof(Material));
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

    public override Material getElevatorMaterial() {
        return (Material)Resources.Load("Woodbox", typeof(Material));
    }

    #endregion
}
