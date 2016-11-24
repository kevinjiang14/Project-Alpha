using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EXPTextManager : MonoBehaviour {

	private GameObject player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		GetComponent<Text> ().text = "EXP: " + playerScript.getCurrentEXP ().ToString () + "/" + playerScript.getEXPtoLVL ().ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "EXP: " + playerScript.getCurrentEXP ().ToString () + "/" + playerScript.getEXPtoLVL ().ToString ();
	}
}
