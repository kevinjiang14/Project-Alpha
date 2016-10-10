using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject mapManager;
	public GameObject playerMonitorView;
	public GameObject playerMenu;

	public void Awake(){
		if(MapManager.instance == null){
			Instantiate (mapManager);
		}

		if (playerMonitorView.activeSelf == false) {
			playerMonitorView.SetActive (true);
		}
	}

	public void Update(){
		if (Input.GetButtonDown ("Menu")) {
			if (playerMenu.activeSelf == false) {
				playerMenu.SetActive (true);
				playerMonitorView.SetActive (false);
			} else {
				playerMenu.SetActive (false);
				playerMonitorView.SetActive (true);
			}
		}
	}
}
