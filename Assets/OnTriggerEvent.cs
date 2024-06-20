using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
     public UnityEvent onTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEvent.Invoke();
    }
}
