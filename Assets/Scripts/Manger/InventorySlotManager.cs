using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventorySlotManager : MonoBehaviour {

    private Player playerScript;

	private GameObject item;

    void Start(){
        gameObject.GetComponent<Button>().onClick.AddListener(EquipItem);
    }

    // Button click calls this method
    public void EquipItem(){
        if (item != null)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            playerScript.EquipItem(item);
        }
    }

    // Insert item into an inventory slot
	public void InsertItem(GameObject item, int quantity){
		this.item = item;
		transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
		transform.GetChild(1).GetComponent<Text>().text = quantity.ToString();
	}

    // Remove item from inventory slots
	public void RemoveItem(){
        this.item = null;
		transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
		transform.GetChild(1).GetComponent<Text>().text = "";
	}
}
