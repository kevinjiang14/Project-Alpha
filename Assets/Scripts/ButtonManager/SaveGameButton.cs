using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Button>().onClick.AddListener(SaveLoadGame.Save);
    }
}
