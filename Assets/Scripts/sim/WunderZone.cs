using UnityEngine;
using UnityEditor;
using System.Xml;

public class WunderZone : GeometryElement {

    private string wunderZoneId;
    private XmlElement morphosisEntry;

    internal void setWunderZoneId(string wunderZoneId) {
        this.wunderZoneId = wunderZoneId;
    }

    internal void setMorphosisEntry(XmlElement morphosisEntry) {
        this.morphosisEntry = morphosisEntry;
    }
}
