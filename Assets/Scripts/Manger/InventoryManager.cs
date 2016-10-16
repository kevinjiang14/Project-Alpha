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
        inventoryDictionary = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getInventory().getInventory();
        Dictionary<GameObject, int>.KeyCollection tempInventoryHolder = inventoryDictionary.Keys;
        if (tempInventoryHolder != null){
            foreach (GameObject item in tempInventoryHolder)
            {
                Transform slot = transform.Find("ItemSlots").Find("ItemGrid").Find(string.Format("Slot{0}", numOfItems));
                slot.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                slot.GetChild(1).GetComponent<Text>().text = inventoryDictionary[item].ToString();
                numOfItems++;
            }
        }
    }

    public void ClearInventory(){
        for(int i = 0; i < 40; i++){
            Transform tempInventorySlots = transform.Find("ItemSlots").Find("ItemGrid").Find(string.Format("Slot{0}", i));
            tempInventorySlots.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
            tempInventorySlots.GetChild(1).GetComponent<Text>().text = "";
        }
    }
}
