using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteAfterCinematics : MonoBehaviour
{
    [SerializeField] UnityEvent onColision;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        onColision.Invoke();
    }
}
