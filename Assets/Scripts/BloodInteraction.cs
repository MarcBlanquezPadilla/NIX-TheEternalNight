using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodInteraction : MonoBehaviour {

    [SerializeField] private UnityEvent onInteract;

    public void Interact() {

        onInteract.Invoke();
    }
}
