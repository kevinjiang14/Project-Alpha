using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHitBox : MonoBehaviour {

    private Player playerScript;
//	private GameObject player;
 
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy"){
//			player = GameObject.FindGameObjectWithTag ("Player");
            playerScript = transform.parent.gameObject.GetComponent<Player>();
            coll.gameObject.GetComponent<Enemy>().TakeDamage(playerScript.getMeleeDamage());
        }
    }
}
