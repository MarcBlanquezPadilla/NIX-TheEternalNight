using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour {

    public Dictionary<string, Elements> elements = new Dictionary<string, Elements>();
    public string currentElement;

    public void Awake() {

        foreach (Elements element in GetComponentsInChildren<Elements>()) {

            elements.Add(element.name, element);
        }
    }

    public void CastHability(int hability) {

        elements[currentElement].CastHability(hability);
    }

    public void CastHability(int hability, string elementName)
    {

        elements[elementName].CastHability(hability);
    }


    public void ChangeElement(string newElement) {

        currentElement = newElement;
    }

    public string ReturnElement() {

        return currentElement;
    }
}
