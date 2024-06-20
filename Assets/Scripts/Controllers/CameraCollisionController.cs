using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    [SerializeField] public LayerMask collisionMask;
    [SerializeField] public Vector3 cameradef;

    private void Awake()
    {
        cameradef = transform.GetChild(0).localPosition;
    }


    private void Update()
    {
        Debug.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward, Color.red);
        CameraCollision();
    }

    void CameraCollision()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.GetChild(0).position, transform.GetChild(0).position + cameradef, out hit, collisionMask))
        {
            Debug.Log("chocando");
            transform.GetChild(0).position += transform.GetChild(0).forward * 5;// -Vector3.Distance(transform.GetChild(0).position, hit.point);
        }
       /* else
        {
            Debug.Log("no chocando");
            transform.GetChild(0).localPosition = cameradef;
        }*/

    }
}
