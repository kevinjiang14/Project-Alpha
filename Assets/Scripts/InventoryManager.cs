using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    private Player playerScript;
    private Inventory playerInventory;

    // Number of items in inventory
    private int numOfItems;

    void Update() {
        numOfItems = 0;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerInventory = playerScript.getInventory();
        Dictionary<GameObject, int>.KeyCollection tempInventoryHolder = playerInventory.getInventory().Keys;
        if (tempInventoryHolder != null){
            foreach (GameObject item in tempInventoryHolder)
            {
                Transform slot = transform.Find("ItemSlots").Find("ItemGrid").Find(string.Format("Slot{0}", numOfItems));
                slot.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                numOfItems++;
            }
        }
    }
}
