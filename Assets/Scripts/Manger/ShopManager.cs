using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	public GameObject menuSlot;

	public void AddItemsToShop(List<GameObject> items){
		for (int i = 0; i < items.Count; i++) {
			GameObject slot = AddSlot ();
			ShopSlotManager ssManager = slot.GetComponent<ShopSlotManager> ();
			ssManager.AddItem (items[i]);
		}
	}

	public GameObject AddSlot(){
		GameObject slot = Instantiate (menuSlot);
		slot.transform.SetParent(transform.Find ("ShopSlots").Find ("Layout"));
		return slot;
	}
}
