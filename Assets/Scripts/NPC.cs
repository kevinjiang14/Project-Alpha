using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	// Player reference
	private Player player;

	// Array of randomly selected items in shop
	private GameObject[] shop;
	private GameObject shopMenu;

	// Public array of gameobjects that are possible of being selected to be placed in shop
	public GameObject[] items;

	// Use this for initialization
	void Start () {
		// Randomly generate shop items
	}
	
	// Update is called once per frame
	void Update () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

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
