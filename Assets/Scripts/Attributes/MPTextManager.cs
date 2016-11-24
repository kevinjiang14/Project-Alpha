using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MPTextManager : MonoBehaviour {

	private GameObject player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		GetComponent<Text> ().text = "MP: " + playerScript.getCurrentMana ().ToString () + "/" + playerScript.getMaxMana ().ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "MP: " + playerScript.getCurrentMana ().ToString () + "/" + playerScript.getMaxMana ().ToString ();
	}
}
