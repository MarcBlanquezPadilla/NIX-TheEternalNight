using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasersParents : MonoBehaviour
{
    public MeshRenderer[] rays;

    public void SwitchRays(bool b)
    {
        foreach (MeshRenderer ray in rays)
        {
            ray.enabled = b;
        }
    }

}
