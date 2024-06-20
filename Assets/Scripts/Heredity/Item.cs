using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject {
    public enum ItemTypes
    {
        Default,
        Resource,
        Weapon,
        Potion,
        Puzzle
    }

    public int id;
    public string itemName;
    public string stringKey;
    public string descriptionKey;
    public bool stackeable;
    public ItemTypes itemType;
    public Sprite typeIcon;
    public GameObject itemObj;
}
