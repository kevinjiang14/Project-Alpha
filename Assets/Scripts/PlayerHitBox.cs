using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHitBox : MonoBehaviour {

	private List<> collisionList;

    private Player playerScript;

	void Start(){
		collisionList = new List<Collision2D> ();
	}

	void Update(){
		for (int i = 0; i < collisionList.Count; i++) {
			if (collisionList[i]
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
        playerScript = transform.parent.gameObject.GetComponent<Player>();

		collisionList.Add (coll);

		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Collision Detected");
		}
		if (coll.gameObject.tag == "Enemy") {
			Debug.Log ("Enemy hit for " + playerScript.getDamage () + " damage");
			coll.gameObject.GetComponent<Enemy> ().TakeDamage (playerScript.getDamage ());
		}
    }
}
