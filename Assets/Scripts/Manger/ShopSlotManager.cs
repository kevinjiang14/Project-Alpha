using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopSlotManager : MonoBehaviour {

	private GameObject item;

	private Player playerScript;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (BuyItem);
	}

	public void BuyItem(){
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		if (playerScript.getGold() >= item.GetComponent<Item> ().cost && item != null) {
			playerScript.DecreaseGold (item.GetComponent<Item> ().cost);
			playerScript.getInventory ().AddtoInventory (item, 1);
		}
	}

	public void AddItem(GameObject item){
		this.item = item;

		transform.Find ("ItemIcon").GetComponent<Image> ().sprite = item.GetComponent<SpriteRenderer> ().sprite;
		transform.Find ("ItemName").GetComponent<Text> ().text = item.GetComponent<Item>().itemName;
		transform.Find ("ItemCost").GetComponent<Text> ().text = "" + item.GetComponent<Item> ().cost + "g";
	}
}
