using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DefenseButton : MonoBehaviour {

	private GameObject player;
	private Player playerScript;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		gameObject.GetComponent<Button> ().onClick.AddListener (IncreaseVitality);
	}

	void IncreaseVitality(){
		playerScript.IncreaseDefense ();
	}
}
