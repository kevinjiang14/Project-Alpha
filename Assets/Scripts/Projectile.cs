using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed;
	private int damage;
	private int direction;
	private Vector3 movementVector;

	void Start(){
		Debug.Log ("Direction is " + direction.ToString ());
		if (direction == 0 || direction == 1 || direction == 2 || direction == 3) {
			movementVector = new Vector3 (speed * -1f, 0f, 0f);
		}
	}

	// Update is called once per frame
	void Update () {
		transform.Translate (movementVector * Time.deltaTime);
	}

	public void SetArrowsParams(int damage, int direction){
		this.damage = damage;
		this.direction = direction;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<Enemy> ().TakeDamage (damage);
			Debug.Log ("Enemy Hit");
		}
		Destroy (gameObject);
	}
}
