using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipmentSlotManager : MonoBehaviour {

	private GameObject EquippedItem;
	private Inventory PlayerInventory;

    void Start(){
        gameObject.GetComponent<Button>().onClick.AddListener(UnequipItem);
    }

    // Equip new item
    public void EquipItem (GameObject item){
		if (EquippedItem != null) {
			UnequipItem ();
		}

		if (item != null) {
			// Set new item into equipped slot
			PlayerInventory = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getInventory ();
			EquippedItem = item;
			PlayerInventory.DecreaseItemQuantity (item);
			transform.GetChild (0).GetComponent<Image> ().sprite = item.GetComponent<SpriteRenderer> ().sprite;
		}
	}

	// Unequip current item
	public void UnequipItem(){
        if (EquippedItem != null)
        {
			// Call this to remove the bonus stats being received from the item
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UnequipItem(EquippedItem);
            // Add equipped item back to player's inventory
            PlayerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getInventory();
            PlayerInventory.AddtoInventory(EquippedItem, 1);
            EquippedItem = null;
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
        }
	}

	// Get the item placed in this slot
	public GameObject GetEquippedItem(){
		if (EquippedItem != null) {
			return EquippedItem;
		} else
			return null;
	}
}
