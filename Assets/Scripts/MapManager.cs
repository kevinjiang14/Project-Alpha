using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapManager : MonoBehaviour {

    public static MapManager instance = null;

	// Map Validation variables
	private int numOfRooms = 0;
	private int MinRooms = 20;
	private int MaxRooms = 35;
	private bool validMap = false;
	private GameObject Map;


	// Level Indexing variables
    private int index;
	private float startColumn = 5;
	private float startRow = 5;

	//
    private GameObject[] levelList;
    private GameObject currentLevel;

	// Public GameObjects to be instantiated
    public GameObject levelObject;
    public GameObject playerList;
    public GameObject Camera;

	// Player GameObject that is instantiated
	private GameObject player;

    public void Awake(){

		// Verifying only one instance of MapManager is in existance
		if (instance == null) {
			instance = this;
		} 
		// If another exist then destroy this one
		else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		Initialization ();
		while (validMap == false) {
			IsMapOfValidSize ();
		}
		spawnPlayer(startColumn, startRow);
	}

	/* 
	 * Initialization of new GameObjects being filled with levels
	 * Call this method when creating a new map for any reason
	*/
	public void Initialization (){
		levelList = new GameObject[100];
		Map = new GameObject ("Map");
		MapInitization ();
	}

    // Map Initialization
    public void MapInitization(){
        CreateMap(null, startColumn, startRow);
    }

	// Recursively fills in the map with levels
	public void CreateMap (GameObject level, float Tcolumn, float Trow){
		bool nExit = false;
		bool eExit = false;
		bool sExit = false;
		bool wExit = false;

		if (level == null){
            index = (((int)Tcolumn) * 10) + ((int)Trow);
			currentLevel = Instantiate(levelObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			currentLevel.GetComponent<LevelManager> ().SetPosition ((int)Tcolumn, (int)Trow);
			currentLevel.GetComponent<LevelManager>().CreateLevel();
            currentLevel.transform.Translate((int)Tcolumn * 15, (int)Trow * 9, 0);
			currentLevel.transform.SetParent (Map.transform);
			levelList[index] = currentLevel;
			numOfRooms += 1;
            CreateMap(currentLevel, Tcolumn, Trow);
        }
        else if ((int)Tcolumn * 10 + (int)Trow < 100 && (int)Tcolumn * 10 + (int)Trow > 0){
            LevelManager levelscript = level.GetComponent<LevelManager>();
            //TODO: Set the edge levels to have a closed exit on their side corresponding to which edge they're at
			//TODO: OPTIMIZE! Recreate this with seperate method to reduce code duplication
            if (levelscript.hasNExit() == true && ((int)Tcolumn * 10) + ((int)Trow + 1) < 100 && (int)Trow <= 9){
                if (levelList[((int)Tcolumn * 10) + ((int)Trow + 1)] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Trow++;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject levelTemp = Instantiate(levelObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					LevelManager tempLevelManager = levelTemp.GetComponent<LevelManager> ();

					tempLevelManager.setSexit (1);
					sExit = true;
					// Check if new level is on edge to close off exits
					// This level is in the north-most edge
					if (Trow == 9){
						tempLevelManager.setNexit (0);
						nExit = true;
					}
					// This level is in the east-most edge
					if (Tcolumn == 9){
						tempLevelManager.setEexit (0);
						eExit = true;
					}
					// This level is in the west-most edge
					if (Tcolumn == 0){
						tempLevelManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring levels of new level and if theres a connecting exit from neighbor to create exits for it
					// Check if north level exist and if so and has a south exit then create a north exit in this level
					if (nExit == false && levelList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
						GameObject northLevel = levelList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northLevel.GetComponent<LevelManager> ().hasSExit ()) {
							tempLevelManager.setNexit (1);
							nExit = true;
						} else {
							tempLevelManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if east level exist and if so and has a south exit then create a north exit in this level
					if (eExit == false && levelList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastLevel = levelList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastLevel.GetComponent<LevelManager> ().hasWExit ()) {
                            tempLevelManager.setEexit (1);
							eExit = true;
						} else {
							tempLevelManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;

					// Check if west level exist and if so and has a south exit then create a north exit in this level
					if (wExit == false && levelList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westLevel = levelList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westLevel.GetComponent<LevelManager> ().hasEExit ()) {
                            tempLevelManager.setWexit (1);
							wExit = true;
						} else {
							tempLevelManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					levelTemp.GetComponent<LevelManager> ().SetPosition ((int)Tcolumn, (int)Trow);
                    levelTemp.GetComponent<LevelManager>().CreateLevel();
                    levelTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					levelTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    levelList[index] = levelTemp;
                    CreateMap(levelTemp, Tcolumn, Trow);
                    Trow -= 1;
                }
            }
            if (levelscript.hasSExit() == true && ((int)Tcolumn) * 10 + ((int)Trow - 1) >= 0 && (int)Trow >= 0){
                if (levelList[((int)Tcolumn) * 10 + ((int)Trow - 1)] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Trow--;
                    index = (((int)Tcolumn) * 10) + (int)Trow;

					GameObject levelTemp = Instantiate(levelObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					LevelManager tempLevelManager = levelTemp.GetComponent<LevelManager> ();

					tempLevelManager.setNexit (1);
					nExit = true;
					// Check if new level is on edge to close off exits
					// This level is in the south-most edge
					if (Trow == 0){
						tempLevelManager.setSexit (0);
						sExit = true;
					}
					// This level is in the east-most edge
					if (Tcolumn == 9){
						tempLevelManager.setEexit (0);
						eExit = true;
					}
					// This level is in the west-most edge
					if (Tcolumn == 0){
						tempLevelManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring levels of new level and if theres a connecting exit from neighbor to create exits for it
					// Check if south level exist and if so and has a north exit then create a south exit in this level
					if (sExit == false && levelList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southLevel = levelList [(int)Tcolumn * 10 + ((int)Trow - 1)];
						if (southLevel.GetComponent<LevelManager> ().hasNExit ()) {
                            tempLevelManager.setSexit (1);
							sExit = true;
						} else {
							tempLevelManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;
					
					// Check if east level exist and if so and has a south exit then create a north exit in this level
					if (eExit == false && levelList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastLevel = levelList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastLevel.GetComponent<LevelManager> ().hasWExit ()) {
                            tempLevelManager.setEexit (1);
							eExit = true;
						} else {
							tempLevelManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;

					// Check if west level exist and if so and has a south exit then create a north exit in this level
					if (wExit == false && levelList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westLevel = levelList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westLevel.GetComponent<LevelManager> ().hasEExit ()) {
                            tempLevelManager.setWexit (1);
							wExit = true;
						} else {
							tempLevelManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					levelTemp.GetComponent<LevelManager> ().SetPosition ((int)Tcolumn, (int)Trow);
					levelTemp.GetComponent<LevelManager>().CreateLevel();
                    levelTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					levelTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    levelList[index] = levelTemp;
                    CreateMap(levelTemp, Tcolumn, Trow);
                    Trow += 1;
                }
            }
            if (levelscript.hasEExit() == true && ((int)Tcolumn + 1) * 10 + (int)Trow < 100 && (int)Tcolumn <= 9){
                if (levelList[((int)Tcolumn + 1) * 10 + (int)Trow] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Tcolumn++;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject levelTemp = Instantiate(levelObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					LevelManager tempLevelManager = levelTemp.GetComponent<LevelManager> ();

					tempLevelManager.setWexit (1);
					wExit = true;
					// Check if new level is on edge to close off exits
					// This level is in the north-most edge
					if (Trow == 9){
						tempLevelManager.setNexit (0);
						nExit = true;
					}
					// This level is in the east-most edge
					if (Tcolumn == 9){
						tempLevelManager.setEexit (0);
						eExit = true;
					}
					// This level is in the south-most edge
					if (Trow == 0){
						tempLevelManager.setSexit (0);
						sExit = true;
					}

					// Check neighboring levels of new level and if theres a connecting exit from neighbor to create exits for it
					// Check if north level exist and if so and has a south exit then create a north exit in this level
					if (nExit == false && levelList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
						GameObject northLevel = levelList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northLevel.GetComponent<LevelManager> ().hasSExit ()) {
                            tempLevelManager.setNexit (1);
							nExit = true;
						} else {
							tempLevelManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if east level exist and if so and has a south exit then create a north exit in this level
					if (eExit == false && levelList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastLevel = levelList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastLevel.GetComponent<LevelManager> ().hasWExit ()) {
                            tempLevelManager.setEexit (1);
							eExit = true;
						} else {
							tempLevelManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;
					
					// Check if south level exist and if so and has a north exit then create a south exit in this level
					if (sExit == false && levelList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southLevel = levelList [(int)Tcolumn * 10 + (int)Trow - 1];
						if (southLevel.GetComponent<LevelManager> ().hasNExit ()) {
                            tempLevelManager.setSexit (1);
							sExit = true;
						} else {
							tempLevelManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;

					levelTemp.GetComponent<LevelManager> ().SetPosition ((int)Tcolumn, (int)Trow);
					levelTemp.GetComponent<LevelManager>().CreateLevel();
                    levelTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					levelTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    levelList[index] = levelTemp;
                    CreateMap(levelTemp, Tcolumn, Trow);
                    Tcolumn -= 1;
                }
            }
            if (levelscript.hasWExit() == true && ((int)Tcolumn - 1) * 10 + (int)Trow >= 0 && (int)Tcolumn >= 0){
                if (levelList[((int)Tcolumn - 1) * 10 + (int)Trow] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Tcolumn--;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject levelTemp = Instantiate(levelObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					LevelManager tempLevelManager = levelTemp.GetComponent<LevelManager> ();

					tempLevelManager.setEexit (1);
					eExit = true;
					// Check if new level is on edge to close off exits
					// This level is in the north-most edge
					if (Trow == 9){
						tempLevelManager.setNexit (0);
						nExit = true;
					}
					// This level is in the south-most edge
					if (Trow == 0){
						tempLevelManager.setSexit (0);
						sExit = true;
					}
					// This level is in the west-most edge
					if (Tcolumn == 0){
						tempLevelManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring levels of new level and if theres a connecting exit from neighbor to create exits for it
					// Check if north level exist and if so and has a south exit then create a north exit in this level
					if (nExit == false && levelList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
                        GameObject northLevel = levelList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northLevel.GetComponent<LevelManager> ().hasSExit ()) {
							tempLevelManager.setNexit (1);
							nExit = true;
						} else {
							tempLevelManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if south level exist and if so and has a north exit then create a south exit in this level
					if (sExit == false && levelList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southLevel = levelList [((int)Tcolumn) * 10 + (int)Trow - 1];
						if (southLevel.GetComponent<LevelManager> ().hasNExit ()) {
                            tempLevelManager.setSexit (1);
							sExit = true;
						} else {
							tempLevelManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;

					// Check if west level exist and if so and has a south exit then create a north exit in this level
					if (wExit == false && levelList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westLevel = levelList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westLevel.GetComponent<LevelManager> ().hasEExit ()) {
                            tempLevelManager.setWexit (1);
							wExit = true;
						} else {
							tempLevelManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					levelTemp.GetComponent<LevelManager> ().SetPosition ((int)Tcolumn, (int)Trow);
					levelTemp.GetComponent<LevelManager>().CreateLevel();
                    levelTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					levelTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    levelList[index] = levelTemp;
                    CreateMap(levelTemp, Tcolumn, Trow);
                    Tcolumn += 1;
                }
            }          
        }
	}
    //TODO: Make player spawn to be independent of the level to prevent player from spawning in each level
    public void spawnPlayer(float Pcolumn, float Prow){
		Vector3 playerInitPosition = new Vector3(Pcolumn * 15f + 7f, Prow * 9f + 4f, -0.01f);
        player = Instantiate(playerList, playerInitPosition, Quaternion.identity) as GameObject;
		SetCamera(player, playerInitPosition);
    }

    public void SetCamera(GameObject player, Vector3 cameraPosition){
        GameObject playerCamera = Instantiate(Camera, new Vector3(cameraPosition.x, cameraPosition.y, -10f), Quaternion.identity) as GameObject;
        playerCamera.transform.SetParent(player.transform);
    }

	public void IsMapOfValidSize(){
		if (numOfRooms >= MinRooms && numOfRooms <= MaxRooms) {
			validMap = true;
		} else {
			validMap = false;
			RecreateMap ();
		}
	}

	public void RecreateMap(){
		Destroy (Map);
		numOfRooms = 0;
		Initialization ();
	}
}
