using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject mapManager;

	public void Awake(){
		if(MapManager.instance == null){
			Instantiate (mapManager);
		}  
	}
}
