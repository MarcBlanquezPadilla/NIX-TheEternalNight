using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : Interactable {

    [SerializeField] private UnityEvent OnPress;
    [SerializeField] private bool justOneTime;

    private bool done = false;
    public override void Interact() {

        if (!(justOneTime && done))
        {
            OnPress.Invoke();
        }
    }
}
