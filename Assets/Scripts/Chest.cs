using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Chest type
	private int chestType;

	// Item that is contained in the chest
	private int[] itemID;
	private int[] quantityOfItems;
	private int numOfItems;

	// Lets us know if the chest has already been open, initially false
	private bool isOpened = false;

	private Animator chestAnimation;

	// Use this for initialization
	void Start () {
		chestAnimation = gameObject.GetComponent<Animator> ();

		quantityOfItems = new int[3];
		itemID = new int[3];

		// Assign chest type
		chestType= Random.Range (0, 2);

		// 0 is enemy chest
		if (chestType == 0) {
			// Spawn random enemy
		} else {
			// TODO: Assign up to 3 items in the chest
			numOfItems = Random.Range (1, 2);

		}
		for(int i = 0; i < numOfItems; i++){
			itemID[i] = Random.Range(63, 63);
			quantityOfItems[i] = Random.Range (1, 4);
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			chestAnimation.SetBool ("Open", true);
			isOpened = true;
		}
	}
	

}
