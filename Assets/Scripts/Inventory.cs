using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory{

    private Dictionary<GameObject, int> inventory;
    private GameObject currentHotbar;

    public Inventory()
    {
        inventory = new Dictionary<GameObject, int>();
    }

    // Add item to inventory
    public void AddtoInventory(GameObject item, int quantity)
    {
        // If item exist in inventory increment quantity else add new item
        if (inventory.ContainsKey(item))
        {
            int quantitytemp = inventory[item];
            quantitytemp += quantity;
            inventory[item] = quantitytemp;
        }
        else inventory.Add(item, quantity);

        // If item is consumable then add to hotbar
        if (item.GetComponent<Item>().ItemType == 0)
        {
            AddtoHotbar(item);
        }

    }

    // Remove item from inventory
    public void DecreaseItemQuantity(GameObject item)
    {

        if (inventory[item] > 1)
        {
            int quantity = inventory[item];
            quantity -= 1;
            inventory[item] = quantity;
        }
        else inventory.Remove(item);
    }

    // Add item to hotbar
    public void AddtoHotbar(GameObject item)
    {
        currentHotbar = GameObject.FindGameObjectWithTag("HotbarMenu");
        currentHotbar.GetComponent<HotbarManager>().AddtoHotbar(item);
    }

    // Gets the quantity of item
    public int GetQuantityofItem(GameObject item)
    {
        return inventory[item];
    }

    public Dictionary<GameObject, int> getInventory()
    {
        return inventory;
    }

    public void setInventory(Dictionary<GameObject, int> inventory)
    {
        this.inventory = inventory;
    }
}
