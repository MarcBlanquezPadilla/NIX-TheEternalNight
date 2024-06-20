using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle1Controller : MonoBehaviour {

    [SerializeField] private UnityEvent onEnabled;
    [SerializeField] private UnityEvent onNoEnabled;

    public void TryToEnable() {

        if (InventoryManager.Instance.ReturnAmountOfItem("LightningConnector") > 0) {

            InventoryManager.Instance.RemoveItem("LightningConnector");
            onEnabled.Invoke();
        }
        else {

            onNoEnabled.Invoke();
        }
    }
}
