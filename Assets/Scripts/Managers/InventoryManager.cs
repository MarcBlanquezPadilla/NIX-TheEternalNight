using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class InventoryManager : MonoBehaviour {

    #region Instance

    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();
            }
            return _instance;
        }
    }

    #endregion

    public struct Items
    {
        public string key;
        public Item item;
        public int amount;
    }

    private InventoryItemTextureController textureItemSelected;

    [Header("Referenced")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform itemContent;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    public GameObject inventory;

    private List<InventoryItemController> inventoryItemControllers = new List<InventoryItemController>();
    public int selectedInvetoryItemController;

    private Dictionary<string, Items> _items = new Dictionary<string, Items>();

    private void Awake() {

        inventory.SetActive(false);
        textureItemSelected = GetComponentInChildren<InventoryItemTextureController>();
    }

    public void OpenInventory(bool open)
    {
        if (open)
        {
            inventory.SetActive(true);
            textureItemSelected.EnableCamera(true);
            LoadItems();
        }
        else
        {
            inventory.SetActive(false);
            textureItemSelected.ShowItem("");
            textureItemSelected.EnableCamera(false);
        }
    }

    public void AddItem(Item item) {

        if (!_items.ContainsKey(item.itemName)) {

            Items j = new Items();
            j.key = item.itemName;
            j.item = item;
            j.amount = 0;
            _items.Add(j.key, j);
        }

        if (_items[item.itemName].item.stackeable || _items[item.itemName].amount == 0)
        {
            Items j = new Items();
            j = _items[item.itemName];
            j.amount++;
            _items[item.itemName] = j;
        }

        if (inventory.gameObject.activeInHierarchy)
        {
            UpdateInventory();
            
        }
    }

    public void AddAmountOfItem(Item item, int amount) {

        if (!_items.ContainsKey(item.itemName)) {

            Items j = new Items();
            j.key = item.itemName;
            j.item = item;
            j.amount = 0;
            _items.Add(j.key, j);
        }

        if (_items[item.itemName].item.stackeable || _items[item.itemName].amount == 0)
        {
            Items j = new Items();
            j = _items[item.itemName];
            j.amount++;
            _items[item.itemName] = j;
        }

        if (inventory.gameObject.activeInHierarchy)
        {
            UpdateInventory();
        }
    }

    public void RemoveItem(Item item) {

        Items j = new Items();
        j = _items[item.itemName];
        j.amount --;
        
        if (j.amount == 0)
        {
            _items.Remove(item.itemName);
        }
        else
        {
            _items[item.itemName] = j;
        }

        if (inventory.gameObject.activeInHierarchy)
        {
            UpdateInventory();
        }
    }

    public void RemoveItem(string itemName)
    {
        _items.Remove(itemName);
    }

    public void RemoveAmountOfItem(Item item, int amount) {

        Items j = new Items();
        j = _items[item.itemName];
        j.amount -= amount;
        if (j.amount <= 0)
        {
            _items.Remove(item.itemName);
        }
        else
        {
            _items[item.itemName] = j;
        }

        if (inventory.gameObject.activeInHierarchy)
        {
            UpdateInventory();
        } 
    }

    public void LoadItems() {

        OrderDictionary();

        for (int i = 0; i < itemContent.childCount; i++) {

            Destroy(itemContent.GetChild(i).gameObject);
        }

        inventoryItemControllers.Clear();

        foreach (KeyValuePair<string,Items> item in _items) {

            if (item.Value.amount > 0) {

                GameObject obj = Instantiate(prefab, itemContent);
                InventoryItemController inventoryItemController = obj.GetComponent<InventoryItemController>();
                inventoryItemController.SetItem(item.Value.item);
                inventoryItemControllers.Add(inventoryItemController);
                obj.transform.SetParent(itemContent);
            }
        }

        if (inventoryItemControllers.Count>0)
        {
            SelectItem(inventoryItemControllers[0]);
        }
    }


    public void SelectItem(InventoryItemController inventoryItemController)
    {
        selectedInvetoryItemController = inventoryItemControllers.IndexOf(inventoryItemController);

        foreach (InventoryItemController iic in inventoryItemControllers)
        {
            iic.SwitchSelected(false);
        }

        inventoryItemController.SwitchSelected(true);
        textureItemSelected.ShowItem(inventoryItemControllers[selectedInvetoryItemController].item.itemName);
        itemName.text = inventoryItemController.item.stringKey;
        itemDescription.text = inventoryItemController.item.descriptionKey;
    }
    
    public void SelectNext(bool down)
    {
        int nextIndex;

        if (down)
        {
            nextIndex = selectedInvetoryItemController+1;
            if (nextIndex == inventoryItemControllers.Count)
            {
                nextIndex = 0;
            }
        }
        else
        {
            nextIndex = selectedInvetoryItemController-1;
            if (nextIndex < 0)
            {
                nextIndex = inventoryItemControllers.Count-1;
            }
        }

        SelectItem(inventoryItemControllers[nextIndex]);
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < itemContent.childCount; i++)
        {
            InventoryItemController inventoryItemController = itemContent.GetChild(i).GetComponent<InventoryItemController>();

            if (inventoryItemControllers.Contains(inventoryItemController))
            {
                inventoryItemController.UpdateItemController();
            }
            else
            {
                Destroy(inventoryItemController.gameObject);
            }
            
        }
        
        SelectItem(inventoryItemControllers[selectedInvetoryItemController]);
    }

    private void OrderDictionary() {

        _items = _items.OrderBy(x => (int)x.Value.item.itemType).ToDictionary(x => x.Key, x => x.Value);
    }

    public int ReturnAmountOfItem(string name) {

        if (!_items.ContainsKey(name))
        {
            return 0;
        }
        else
        {
            return _items[name].amount;
        }
    }

    public void UseItems() {

        Item item = inventoryItemControllers[selectedInvetoryItemController].item;

        if (item.itemName == "LifeSyringe") {

            GameManager.Instance.characterController.healthBehaviour.Heal(GameManager.Instance.characterController.healthBehaviour.ReturnMaxHealth() / 4);

            RemoveItem(item);
        }

        if (item.itemName == "AmmoBox") {

            GameManager.Instance.characterController.gunController.GetAmmoBox();

            RemoveItem(item);
        }

        bool empty = true;
        
        foreach (KeyValuePair<string, Items> i in _items)
        {
            if (i.Value.amount>0)
            {
                empty = false;
            }
        }

        if (empty)
        {
            textureItemSelected.ShowItem("");
        }
        
        LoadItems();
    }
}
