using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHitBox : MonoBehaviour {

    private Player playerScript;
 
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            playerScript = transform.parent.gameObject.GetComponent<Player>();
            coll.gameObject.GetComponent<Enemy>().TakeDamage(playerScript.getMeleeDamage());
        }
    }
}
