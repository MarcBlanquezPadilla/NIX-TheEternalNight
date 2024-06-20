using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour {

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float lightSpeed;

    [SerializeField] private Light lightObj;
    [SerializeField] private Transform target;
    [SerializeField] private Transform map;

    private int up = -1;

    private void Update() {

        transform.LookAt(target);
        map.rotation = Quaternion.Euler(map.rotation.eulerAngles.x, map.rotation.eulerAngles.y + Time.deltaTime * rotationSpeed, map.rotation.eulerAngles.z);

        if (lightObj.intensity < 25) {

            up = 1;
            lightObj.intensity += Time.deltaTime * lightSpeed * up;

        }
        else if (lightObj.intensity > 150) {

            up = -1;
            lightObj.intensity += Time.deltaTime * lightSpeed * up;

        }

        lightObj.intensity += Time.deltaTime * lightSpeed * up;
    }
}
