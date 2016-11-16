using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener (QuitGame);
	}

	void QuitGame(){
		Application.Quit ();
	}
}
