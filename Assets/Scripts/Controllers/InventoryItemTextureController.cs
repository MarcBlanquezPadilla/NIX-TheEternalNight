using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryItemTextureController : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [Serializable]
    public struct ItemToShow
    {
        [NonSerialized] public string itemName;
        public GameObject objToShow;
    }

    public List<ItemToShow> itemsToShow = new List<ItemToShow>();

    private void Awake()
    {
        for (int i = 0; i < itemsToShow.Count; i++)
        {
            ItemToShow nextItemToShow = new ItemToShow();
            nextItemToShow.objToShow = itemsToShow[i].objToShow;
            nextItemToShow.itemName = nextItemToShow.objToShow.name;
            nextItemToShow.objToShow.SetActive(false);
            itemsToShow[i] = nextItemToShow;
        }

        camera.gameObject.SetActive(false);
    }

    public void ShowItem(string itemName)
    {
        foreach (ItemToShow showItem in itemsToShow)
        {
            if (showItem.itemName == itemName)
                showItem.objToShow.SetActive(true);
            else
                showItem.objToShow.SetActive(false);
        }
    }

    public void EnableCamera(bool b)
    {
        camera.gameObject.SetActive(b);
    }
}
