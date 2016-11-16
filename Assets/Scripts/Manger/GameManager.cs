using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject mapManager;
	public GameObject gameMenu;
	private GameObject currentMenu;

	public void Awake(){
		currentMenu = Instantiate (gameMenu);
		currentMenu.SetActive (false);
		if(MapManager.instance == null){
			Instantiate (mapManager);
		}
	}

	public void Update(){
		if (Input.GetButtonDown ("Cancel")) {
			if (currentMenu.activeSelf == false) {
				currentMenu.SetActive (true);
			} else
				currentMenu.SetActive (false);
		}
	}
}
