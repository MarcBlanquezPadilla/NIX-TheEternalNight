using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class SavedGame
{
    [XmlAttribute]
    public string name;

    [XmlAttribute]
    public int level;

    [XmlAttribute]
    public int positionX;
    
    [XmlAttribute]
    public int positionY;
    
    [XmlAttribute]
    public int positionZ;
}
