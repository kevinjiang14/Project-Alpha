﻿using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	// Level position
	private int levelX;
	private int levelY;

	// Size of the level
	private int rows = 8;
	private int columns = 14;

	// Tile Sprites used to build level
	public GameObject[] bottom_walls;
	public GameObject[] top_walls;
	public GameObject[] left_walls;
	public GameObject[] right_walls;
	public GameObject[] topright_walls;
	public GameObject[] topleft_walls;
	public GameObject[] bottomright_walls;
	public GameObject[] bottomleft_walls;
	public GameObject[] floors;

	// GameObjects to exist in level
	public GameObject[] enemies;
	public GameObject[] chests;
	public GameObject[] items;
	public GameObject[] npc;

	// Transform holder for this level gameObject
	private Transform levelHolder;

	// List of vectors of all tiles on this level
	private List<Vector3> gridPositions = new List<Vector3> ();

	// Variables to determine whether exits exist on this level
	private int nExit, eExit, sExit, wExit;

	public void Awake(){
		Initialization ();
		SetupLevel ();
	}

	//Method used for any initialization needed to be done
	public void Initialization (){
		nExit = Random.Range (0, 2);
		eExit = Random.Range (0, 2);
		sExit = Random.Range (0, 2);
		wExit = Random.Range (0, 2);
	}

	public void SetupLevel(){
		InitialiseGrid ();
	}

	// Creates the level gameObject
	public void CreateLevel(){
		levelHolder = new GameObject ("Current Level").transform;

		if (levelX != 5 || levelY != 5) {
			SpawnEnemies ();
		}

		for (int x = 0; x <= columns; x++) {
			for (int y = 0; y <= rows; y++) {
				GameObject toInstantiate = floors [Random.Range (0, floors.Length)];
				if ((x == 7 && y == 0 && sExit == 1) || (x == 7 && y == rows && nExit == 1) || (x == 0 && y == 4 && wExit == 1) || (x == columns && y == 4 && eExit == 1)){
					toInstantiate = floors [Random.Range (0, floors.Length)];
				}
				else if (x == 0 && y == 0){
					//bottom left corner
					toInstantiate = bottomleft_walls [Random.Range (0, bottomleft_walls.Length)];
				}
				else if (x == 0 && y == rows){
					//top left corner
					toInstantiate = topleft_walls [Random.Range (0, topleft_walls.Length)];
				}
				else if (x == columns && y == 0){
					//bottom right corner
					toInstantiate = bottomright_walls [Random.Range (0, bottomright_walls.Length)];
				}
				else if (x == columns && y == rows){
					//top right corner
					toInstantiate = topright_walls [Random.Range (0, topright_walls.Length)];
				}
				else if (x == 0){
					//left row
					toInstantiate = left_walls [Random.Range (0, left_walls.Length)];
				}
				else if (y == 0){
					//bottom row
					toInstantiate = bottom_walls [Random.Range (0, bottom_walls.Length)];
				}
				else if (x == columns){
					//right row
					toInstantiate = right_walls [Random.Range (0, right_walls.Length)];
				}
				else if (y == rows){
					//top row
					toInstantiate = top_walls [Random.Range (0, top_walls.Length)];
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (levelHolder);
			}
		}

        levelHolder.transform.SetParent(this.transform);
    }

    //Creates a new list of board position with blank Vectors
    public void InitialiseGrid(){
		gridPositions.Clear ();

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	// TODO: set enemy's transform parent to an object that is first in hierarchy to prevent enemy falling below floor when moving into a lvl that they didnt spawn from
	public void SpawnEnemies(){
		GameObject enemy = Instantiate (enemies [0], new Vector3 (3f, 2f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (levelX * 15 + 3, levelY * 9 + 2);
		enemy.transform.SetParent (levelHolder);

		enemy = Instantiate (enemies [0], new Vector3 (3f, 6f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (levelX * 15 + 3, levelY * 9 + 6);
		enemy.transform.SetParent (levelHolder);

		enemy = Instantiate (enemies [0], new Vector3 (11f, 2f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (levelX * 15 + 11, levelY * 9 + 2);
		enemy.transform.SetParent (levelHolder);

		enemy = Instantiate (enemies [0], new Vector3 (11f, 6f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (levelX * 15 + 11, levelY * 9 + 6);
		enemy.transform.SetParent (levelHolder);
	}

	public void SetPosition(int x, int y){
		levelX = x;
		levelY = y;
	}

    public bool hasExits() {
        if (nExit == 1 || eExit == 1 || sExit == 1 || wExit == 1) {
            return true;
        }
        else return false;
    }

    public bool hasNExit() {
        if (nExit == 1) {
            return true;
        }
        else return false;
    }

    public bool hasEExit() {
        if (eExit == 1) {
            return true;
        }
        else return false;
    }

	public bool hasSExit() {
        if (sExit == 1) {
            return true;
        }
        else return false;
    }

    public bool hasWExit() {
        if (wExit == 1) {
            return true;
        }
        else return false;
    }

	public void setNexit(int exist) {
		nExit = exist;
	}

	public void setEexit(int exist) {
		eExit = exist;
	}

	public void setSexit(int exist) {
		sExit = exist;
	}

	public void setWexit(int exist) {
		wExit = exist;
	}
}