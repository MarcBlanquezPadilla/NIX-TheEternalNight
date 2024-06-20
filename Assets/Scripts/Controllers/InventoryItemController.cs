using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemController : MonoBehaviour {

    public Item item;

    public Image typeIcon;
    public Image background;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCuantity;
    private Material material;

    public Color nonSelectedColor;
    public Color selectedColor;

    public void SetItem(Item newItem){

        item = newItem;
        UpdateItemController();
    }

    public void UpdateItemController()
    {
        typeIcon.sprite = item.typeIcon;
        itemName.text = item.stringKey;
        if (item.stackeable)
        {
            itemCuantity.text = "x" + InventoryManager.Instance.ReturnAmountOfItem(item.itemName).ToString();
            itemCuantity.gameObject.SetActive(true);
        }
        else
        {
            itemCuantity.gameObject.SetActive(false);
        }
    }

    public void UseItem() {

        switch (item.itemType)
        {
            case Item.ItemTypes.Default:
                break;
            case Item.ItemTypes.Resource:
                break;
            case Item.ItemTypes.Weapon:
                break;
            case Item.ItemTypes.Potion:
                break;
        }
    }

    public void SwitchSelected(bool selected)
    {
        if (selected)
        {
            background.color = selectedColor;
        }
        else
        {
            background.color = nonSelectedColor;
        }
    }

    public void OnHover()
    {
        InventoryManager.Instance.SelectItem(this);
    }
}
