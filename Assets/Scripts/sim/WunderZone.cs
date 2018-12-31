using UnityEngine;
using System.Xml;

public class WunderZone : GeometryElement {

    private string wunderZoneId;
    internal XmlElement morphosisEntry;

    internal void setWunderZoneId(string wunderZoneId) {
        this.wunderZoneId = wunderZoneId;
    }

    internal void setMorphosisEntry(XmlElement morphosisEntry) {
        this.morphosisEntry = morphosisEntry;
    }

    public override void createObject(GameObject floor) {
        this.createSurfaceObject(new Color(1f, 0f, 0f, .3f), floor);
    }

    public override void createObject(GameObject floor, SimData simData, GeometryLoader gl) {
        this.createSurfaceObject(new Color(1f, 0f, 0f, .3f), floor);
    }

    public void createSurfaceObject(Color color, GameObject parent) {
        AreaGeometry.createPlaneObject(this.id + "-SurfaceObject", this.points, color, floor.elevation, parent);
    }
}
