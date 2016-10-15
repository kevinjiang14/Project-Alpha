using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {

    private Dictionary<GameObject, int> inventory;

    public Inventory(){
        inventory = new Dictionary<GameObject, int>();
    }
    
    public void AddtoInventory(GameObject item, int quantity){
        if (inventory.ContainsKey(item)){
            int quantitytemp = inventory[item];
            quantitytemp += quantity;
            inventory[item] = quantitytemp;
        } else inventory.Add(item, quantity);

        Debug.Log("Player received " + quantity + " " + item.transform.name);
        Debug.Log("Player now has " + inventory[item] + " " + item.transform.name);
    }

    public void RemovefromInventorm(GameObject item){
        if (inventory[item] > 1){
            int quantity = inventory[item];
            quantity -= 1;
            inventory[item] = quantity;
        }  else inventory.Remove(item);
    }

    public Dictionary<GameObject, int> getInventory(){
        return inventory;
    }

    public void setInventory(Dictionary<GameObject, int> inventory){
        this.inventory = inventory;
    }
}
