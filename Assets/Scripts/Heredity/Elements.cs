using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elements : MonoBehaviour {

    public List<Hability> habilities = new List<Hability>();

    public void CastHability(int habilityNumber) {

        habilities[habilityNumber].TryCast(); 
    }

    public virtual void OnChangeToElement(bool b)
    {

    }
}
