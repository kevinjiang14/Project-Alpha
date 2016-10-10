using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StrengthTextManager : MonoBehaviour {

	private GameObject player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		GetComponent<Text> ().text = "Strength:\t" + playerScript.getStrength ().ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "Strength:\t" + playerScript.getStrength ().ToString ();
	}
}
