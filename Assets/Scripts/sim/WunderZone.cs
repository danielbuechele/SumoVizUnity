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

    public override void createObject() {
        this.createSurfaceObject(new Color(1f, 0f, 0f, .3f));
    }

    public void createSurfaceObject(Color color) {
        AreaGeometry.createOriginTarget(this.id + "-SurfaceObject", this.points, color, floor.elevation);
    }
}
