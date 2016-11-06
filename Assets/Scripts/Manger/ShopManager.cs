using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddItemsToShop(List<GameObject> items){
		for (int i = 0; i < items.Count; i++) {
			ShopSlotManager ssManager = transform.Find ("ShopSlots").Find ("Layout").Find (string.Format ("Slot{0}", i)).GetComponent<ShopSlotManager> ();
			ssManager.AddItem (items[i]);
		}
	}
}
