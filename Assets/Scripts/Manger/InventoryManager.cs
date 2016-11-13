using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour{

    // Character menu reference for when equiping items
    private GameObject characterMenu;

    // Inventory dictionary reference for displaying the items with their quantity
    private Dictionary<GameObject, int> inventoryDictionary;

    // Number of items in inventory
    private int numOfItems;

    void Update() {
        ClearInventory();

        numOfItems = 0;
        inventoryDictionary = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getInventory().GetInventory();
        Dictionary<GameObject, int>.KeyCollection tempInventoryHolder = inventoryDictionary.Keys;
        if (tempInventoryHolder != null){
            foreach (GameObject item in tempInventoryHolder)
            {
				InventorySlotManager slot = transform.Find ("ItemSlots").Find ("ItemGrid").Find (string.Format ("Slot{0}", numOfItems)).GetComponent<InventorySlotManager> ();
				slot.InsertItem (item, inventoryDictionary [item]);
				numOfItems++;
            }
        }
    }

    public void ClearInventory(){
        for(int i = 0; i < 40; i++){
			InventorySlotManager tempInventorySlots = transform.Find ("ItemSlots").Find ("ItemGrid").Find (string.Format ("Slot{0}", i)).GetComponent<InventorySlotManager> ();
			tempInventorySlots.RemoveItem ();
        }
    }
}
