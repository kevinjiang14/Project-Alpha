using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventorySlotManager : MonoBehaviour {

	private GameObject item;
	private int quantity;

	public void InsertItem(GameObject item, int quantity){
		this.item = item;
		this.quantity = quantity;
		transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
		transform.GetChild(1).GetComponent<Text>().text = quantity.ToString();
	}

	public void RemoveItem(){
		transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
		transform.GetChild(1).GetComponent<Text>().text = "";
	}
}
