using UnityEngine;
using System.Collections;

public class PlayerHitBox : MonoBehaviour {

    private Player playerScript;

	void OnCollisionEnter2D(Collision2D coll) {
        playerScript = transform.parent.gameObject.GetComponent<Player>();
        Debug.Log("Collision Detected");
        if (coll.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy hit for " + playerScript.getDamage() + " damage");
            coll.gameObject.GetComponent<Enemy>().TakeDamage(playerScript.getDamage());
        }
    }
}
