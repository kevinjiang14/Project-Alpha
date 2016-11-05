using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	// Player reference
	private Player player;

	// Array of randomly selected items in shop
	private ArrayList shop;
	private GameObject shopMenu;

	// Public array of gameobjects that are possible of being selected to be placed in shop
	public GameObject[] items;

	// Use this for initialization
	void Start () {
        shop = new ArrayList();
        // Randomly generate shop items
        for (int i = 0; i < items.Length; i++) {
            // RNG on item appearing in shop
            int roll = Random.Range(0, 100);

            // Add item to shop if item is rolled
            if (roll <= items[i].GetComponent<Item>().chance) {
                shop.Add(items[i]);
            }

        }        
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			shopMenu.SetActive (true);
		}
	}

	public void setShopMenu(GameObject menu){
		shopMenu = menu;
	}
}
