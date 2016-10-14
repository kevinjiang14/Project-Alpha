using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour {

	// Use this for initialization
	void Start () {    
        gameObject.GetComponent<Button>().onClick.AddListener(SaveLoad.Save);
    }
}
