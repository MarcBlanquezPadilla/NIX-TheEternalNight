using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightInteraction : MonoBehaviour {

    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private float timeToInteract;
    [SerializeField] private bool justOneTime = true;
    
    private float timer=0;
    private bool interacted= false;
    public void Interact() {

        timer += Time.deltaTime;
        if (timer>timeToInteract && !(justOneTime && interacted))
        {
            onInteract.Invoke();
            timer = 0;
            interacted = true;
        }

    }
}
