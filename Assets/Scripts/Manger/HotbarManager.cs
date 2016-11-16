using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour {

    // Hotbar attributes
    private int size = 6;
    private int numofItems = 0;
    private GameObject[] items;

    // Player's iventory
    private Player player;

    void Awake(){
        items = new GameObject[6];
    }

    void Update(){
        if (Input.GetButtonDown("Hotslot1")){
            if (items[0] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[0]) == 1){
                    RemoveItemfromHotbar(0);
                } else UseSlot(0);
            }
        }
        if (Input.GetButtonDown("Hotslot2")){
            if (items[1] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[1]) == 1){
                    RemoveItemfromHotbar(1);
                } else UseSlot(1);
            }
        }
        if (Input.GetButtonDown("Hotslot3")){
            if (items[2] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[2]) == 1){
                    RemoveItemfromHotbar(2);
                } else UseSlot(2);
            }
        }
        if (Input.GetButtonDown("Hotslot4")){
            if (items[3] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[3]) == 1){
                    RemoveItemfromHotbar(3);
                } else UseSlot(3);
            }
        }
        if (Input.GetButtonDown("Hotslot5")){
            if (items[4] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[4]) == 1){
                    RemoveItemfromHotbar(4);
                } else UseSlot(4);
            }
        }
        if (Input.GetButtonDown("Hotslot6")){
            if (items[5] != null){
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                // If theres only 1 of the item remaining then remove it else reduce the count of it by 1
                if (player.getInventory().GetQuantityofItem(items[5]) == 1){
                    RemoveItemfromHotbar(5);
                } else UseSlot(5);
            }
        }
    }

    // Add consumable to hotbar if there is room
    public void AddtoHotbar(GameObject item){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Transform temphotitem;

        bool attached = false;
        // Check if item already exist on the hotbar
        for(int i = 0; i < numofItems; i++){
            if(items[i] == item){
                temphotitem = transform.Find(string.Format("HotItem{0}", i));
                temphotitem.GetChild(1).GetComponent<Text>().text = player.getInventory().GetQuantityofItem(item).ToString();
                attached = true;
                break;
            }
        }
        // Check if theres room on the hotbar for item
        if(numofItems < size && !attached){
            items[numofItems] = item;
            temphotitem = transform.Find(string.Format("HotItem{0}", numofItems));
            temphotitem.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            temphotitem.GetChild(1).GetComponent<Text>().text = player.getInventory().GetQuantityofItem(item).ToString();
            numofItems++;
        }
    }

    public void RemoveItemfromHotbar(int slot){
        Transform temphotitem;
        GameObject ItemUsed = items[0];
        items[0] = null;
        temphotitem = transform.Find(string.Format(string.Format("HotItem{0}", slot)));
        temphotitem.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
        temphotitem.GetChild(1).GetComponent<Text>().text = "";
        numofItems--;
        player.UseItem(ItemUsed);
    }

    public void UseSlot(int slot){
        Transform temphotitem;
        player.UseItem(items[slot]);
        temphotitem = transform.Find(string.Format("HotItem{0}", slot));
        temphotitem.GetChild(1).GetComponent<Text>().text = player.getInventory().GetQuantityofItem(items[slot]).ToString();
    }

    public int getNumofItems(){
        return numofItems;
    }
}
