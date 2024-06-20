using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxController : Interactable
{
    [SerializeField] private Item item;

    public override void Interact() {

        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }



}
