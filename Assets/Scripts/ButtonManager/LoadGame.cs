using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener (SaveLoad.Load);
    }
}
