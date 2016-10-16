using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

    // Chest type
    //	private int chestType;

    // Item that is contained in the chest
    //	private int[] itemID;
    //	private int[] quantityOfItems;
    //	private int numOfItems;

    public GameObject[] possibleItems;
    
    private GameObject item;

	// Lets us know if the chest has already been open, initially false
	private bool isOpened = false;

	private Animator chestAnimation;

    private Player player;
	// Use this for initialization
	void Start () {
		chestAnimation = gameObject.GetComponent<Animator> ();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Randomly assign an item to the chest
        int choice = Random.Range(0, 3);
        item = possibleItems[choice];
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player" && isOpened == false) {
			chestAnimation.SetBool ("Open", true);
			isOpened = true;
            player.getInventory().AddtoInventory(item, 1);
		}
	}
	

}
