using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloorTextManager : MonoBehaviour {

	private GameObject map;
	private MapManager mapScript;

	// Use this for initialization
	void Start () {
		map = GameObject.FindGameObjectWithTag ("MapManager");
		mapScript = map.GetComponent<MapManager> ();
		GetComponent<Text> ().text = "Floor " + mapScript.getFloor ().ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = "Floor " + mapScript.getFloor ().ToString ();
	}
}
