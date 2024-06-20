using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

    public float degreesPerSecond = 0;
    
    private void Update() {
        transform.Rotate(Vector3.up * degreesPerSecond * Time.deltaTime, Space.World);
    }
}
