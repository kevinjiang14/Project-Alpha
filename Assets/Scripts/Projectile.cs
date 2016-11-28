using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed;
	private int damage;
	private Vector3 movementVector;

	void Start(){
		damage = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getRangeDamage ();
		movementVector = new Vector3 (speed * -1f, 0f, 0f);
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (movementVector * Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag != "Player" && coll.gameObject.tag != "Projectile") {
			if (coll.gameObject.tag == "Enemy") {
				coll.gameObject.GetComponent<Enemy> ().TakeDamage (damage);
			}
			Destroy (gameObject);
		}
	}

	public void DecreaseDamage(int bonus){
		damage -= bonus;
	}

	public void IncreaseDamage(int bonus){
		damage += bonus;
	}
}
