using UnityEngine;
using System.Collections;

public class EvaktischThemingMode : ThemingMode {
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

    public override Material getElevatorMaterial() {
        return (Material)Resources.Load("SolidColorMaterials/Gelb", typeof(Material));
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

    public override Material getEscalatorTreadMaterial() {
        throw new System.NotImplementedException();
    }

    #endregion
}
