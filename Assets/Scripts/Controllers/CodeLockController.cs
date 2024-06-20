using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockController : MonoBehaviour {

    [SerializeField] private Animator doorAnimator;

    public void TryToOpen() {

        if (InventoryManager.Instance.ReturnAmountOfItem("note1") > 0 && InventoryManager.Instance.ReturnAmountOfItem("note2") > 0) {

            doorAnimator.SetTrigger("Open");
        }
        else { 
        
            // algo
        }
    }
}
