using UnityEngine;
using System.Collections;

public class FloorChange : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			Debug.Log ("Entering next floor");
			GameObject mapManager = GameObject.FindGameObjectWithTag("MapManager");
			MapManager mapScript = mapManager.GetComponent<MapManager> ();
			mapScript.IncreaseFloor ();
			mapScript.RecreateMap ();

			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			Player playerScript = player.GetComponent<Player> ();
			playerScript.ResetPlayerLocation ();
		}
	}
}