using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class RoomManager : MonoBehaviour {

	// Room position
	private int roomX;
	private int roomY;

	// Size of the room
	private int rows;
	private int columns;

	// Room type
	private bool enemyRoom;
	private bool chestRoom;
	private bool ladderRoom;
	private bool npcRoom;
    private bool bossRoom;

	// Variables to determine whether exits exist on this room (0 = exit does not exist, 1 = exit exist)
	private int nExit, eExit, sExit, wExit;

	// Tile Sprites used to build room
	public GameObject[] bottom_walls;
	public GameObject[] top_walls;
	public GameObject[] left_walls;
	public GameObject[] right_walls;
	public GameObject[] topright_walls;
	public GameObject[] topleft_walls;
	public GameObject[] bottomright_walls;
	public GameObject[] bottomleft_walls;
	public GameObject[] floors;
    public GameObject southDoor;
    public GameObject westDoor;
    public GameObject northDoor;
    public GameObject eastDoor;

	// GameObjects to exist in room
	public GameObject[] bosses;
	public GameObject[] enemies;
	public GameObject[] chests;
	public GameObject[] npc;
	public GameObject[] ladder;

	public void Awake(){
		Initialization ();
	}

	// Method used for any initialization needed to be done
	public void Initialization (){
		nExit = Random.Range (0, 2);
		eExit = Random.Range (0, 2);
		sExit = Random.Range (0, 2);
		wExit = Random.Range (0, 2);
	}

	// Creates the room gameObject, also acts as a Recreate Room method
	public void CreateRoom(){
		// Set room size
		rows = 8;
		columns = 14;

		// Destroy existing room if it exist
		if (this.transform.childCount > 0) {
			foreach (Transform child in this.transform) {
				Destroy (child.gameObject);
			}
		}

		// Checks if it's an enemy room
		if (enemyRoom == true) {
			// TODO: Restriction checks will be done in MapManager
			SpawnEnemies ();
		}
		// Checks if it's a chest room
		else if (chestRoom == true) {
			SpawnChest ();
		} 
		// Checks if it's a ladder room
		else if (ladderRoom == true) {
			SpawnLadder ();
		} 
		// Checks if it's an NPC room
		else if (npcRoom == true) {
			SpawnNPC ();
		}

		// Create the floor and walls for the room
		for (int x = 0; x <= columns; x++) {
			for (int y = 0; y <= rows; y++) {
                GameObject instance;
                GameObject toInstantiate;
                if (x == 7 && y == 0 && sExit == 1)
                {
                    toInstantiate = southDoor;
                    instance = Instantiate(toInstantiate, new Vector3(x, y, -0.1f), Quaternion.identity) as GameObject;

                    instance.GetComponent<DoorManager>().SetDoortoRoom((roomX * 10 + roomY), 0);
                }
                else if (x == 0 && y == 4 && wExit == 1)
                {
                    toInstantiate = westDoor;
                    instance = Instantiate(toInstantiate, new Vector3(x, y, -0.1f), Quaternion.identity) as GameObject;

                    instance.GetComponent<DoorManager>().SetDoortoRoom((roomX * 10 + roomY), 1);
                }
                else if (x == 7 && y == rows && nExit == 1)
                {
                    toInstantiate = northDoor;
                    instance = Instantiate(toInstantiate, new Vector3(x, y, -0.1f), Quaternion.identity) as GameObject;

                    instance.GetComponent<DoorManager>().SetDoortoRoom((roomX * 10 + roomY), 2);
                }
                else if (x == columns && y == 4 && eExit == 1)
                {
                    toInstantiate = eastDoor;
                    instance = Instantiate(toInstantiate, new Vector3(x, y, -0.1f), Quaternion.identity) as GameObject;

                    instance.GetComponent<DoorManager>().SetDoortoRoom((roomX * 10 + roomY), 3);
                }
                else if (x == 0 && y == 0)
                {
                    //bottom left corner
                    toInstantiate = bottomleft_walls[Random.Range(0, bottomleft_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (x == 0 && y == rows)
                {
                    //top left corner
                    toInstantiate = topleft_walls[Random.Range(0, topleft_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (x == columns && y == 0)
                {
                    //bottom right corner
                    toInstantiate = bottomright_walls[Random.Range(0, bottomright_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (x == columns && y == rows)
                {
                    //top right corner
                    toInstantiate = topright_walls[Random.Range(0, topright_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (x == 0)
                {
                    //left row
                    toInstantiate = left_walls[Random.Range(0, left_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (y == 0)
                {
                    //bottom row
                    toInstantiate = bottom_walls[Random.Range(0, bottom_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (x == columns)
                {
                    //right row
                    toInstantiate = right_walls[Random.Range(0, right_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else if (y == rows)
                {
                    //top row
                    toInstantiate = top_walls[Random.Range(0, top_walls.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                }
                else {
                    toInstantiate = floors[Random.Range(0, floors.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                }

				instance.transform.SetParent (this.transform);
			}
		}
    }

	public void CreateBossRoom(int floor){
		// Set room type
		setRoomAsBoss ();

		// Set the size of Boss Room
		rows = 24;
		columns = 42;

		// Destroy existing room if it exist
		if (this.transform.childCount > 0) {
			foreach (Transform child in this.transform) {
				Destroy (child);
			}
		}

		// Spawn Boss
		if (bossRoom == true) {
			SpawnBoss (floor);
		}

		// Create the floor and walls for the room
		for (int x = 0; x <= columns; x++) {
			for (int y = 0; y <= rows; y++) {
				GameObject instance;
				GameObject toInstantiate;
				if (x == 21 && y == 0 && sExit == 1)
				{
					toInstantiate = southDoor;
					instance = Instantiate(toInstantiate, new Vector3(x, y, -0.1f), Quaternion.identity) as GameObject;

					instance.GetComponent<DoorManager>().SetDoortoRoom((roomX * 10 + roomY), 0);
				}
				else if (x == 0 && y == 0)
				{
					//bottom left corner
					toInstantiate = bottomleft_walls[Random.Range(0, bottomleft_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (x == 0 && y == rows)
				{
					//top left corner
					toInstantiate = topleft_walls[Random.Range(0, topleft_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (x == columns && y == 0)
				{
					//bottom right corner
					toInstantiate = bottomright_walls[Random.Range(0, bottomright_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (x == columns && y == rows)
				{
					//top right corner
					toInstantiate = topright_walls[Random.Range(0, topright_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (x == 0)
				{
					//left row
					toInstantiate = left_walls[Random.Range(0, left_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (y == 0)
				{
					//bottom row
					toInstantiate = bottom_walls[Random.Range(0, bottom_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (x == columns)
				{
					//right row
					toInstantiate = right_walls[Random.Range(0, right_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else if (y == rows)
				{
					//top row
					toInstantiate = top_walls[Random.Range(0, top_walls.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

				}
				else {
					toInstantiate = floors[Random.Range(0, floors.Length)];
					instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				}

				instance.transform.SetParent (this.transform);
			}
		}
	}

	// Creates the boss in the room
	public void SpawnBoss(int floor){
		// Boss Level Check
		GameObject Boss = Instantiate (bosses [floor / 5 - 1], new Vector3 (19f, 15f, -0.1f), Quaternion.identity) as GameObject;
		Boss.GetComponent<Enemy> ().SetSpawn (roomX * 14 + 19, roomY * 8 + 15);
		Boss.transform.SetParent (this.transform);
	}

	// Creates the enemies in the room 
	public void SpawnEnemies(){
		int choice = Random.Range (0, enemies.Length);
		GameObject enemy = Instantiate (enemies [choice], new Vector3 (3f, 2f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (roomX * 14 + 3, roomY * 8 + 2);
		enemy.transform.SetParent (this.transform);

		choice = Random.Range (0, enemies.Length);
		enemy = Instantiate (enemies [choice], new Vector3 (3f, 6f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (roomX * 14 + 3, roomY * 8 + 6);
		enemy.transform.SetParent (this.transform);

		choice = Random.Range (0, enemies.Length);
		enemy = Instantiate (enemies [choice], new Vector3 (11f, 2f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (roomX * 14 + 11, roomY * 8 + 2);
		enemy.transform.SetParent (this.transform);

		choice = Random.Range (0, enemies.Length);
		enemy = Instantiate (enemies [choice], new Vector3 (11f, 6f, -0.01f), Quaternion.identity) as GameObject;
		enemy.GetComponent<Enemy> ().SetSpawn (roomX * 14 + 11, roomY * 8 + 6);
		enemy.transform.SetParent (this.transform);
	}

	// Creates the ladder in the room
	public void SpawnLadder(){
		GameObject ladderObject = Instantiate (ladder[0], new Vector3(7f, 4f, -0.1f), Quaternion.identity) as GameObject;
		ladderObject.transform.SetParent (this.transform);
	}

	// Createst the chest in the room
	public void SpawnChest(){
		GameObject chestObject = Instantiate (chests[0], new Vector3(7f, 4f, -0.1f), Quaternion.identity) as GameObject;
		chestObject.transform.SetParent (this.transform);
	}

	// Creats the NPC in the room
	public void SpawnNPC(){
		GameObject NPCObject = Instantiate (npc [0], new Vector3 (7f, 4f, -0.1f), Quaternion.identity) as GameObject;
		NPCObject.transform.SetParent (this.transform);
	}

	// Sets the room coordinate
	public void SetPosition(int row, int column){
		roomX = row;
		roomY = column;
	}

	// Method for checking if the room has any exits
    public bool hasExits() {
        if (nExit == 1 || eExit == 1 || sExit == 1 || wExit == 1) {
            return true;
        }
        else return false;
    }

	// Method for checking if the room has a north exit
    public bool hasNExit() {
        if (nExit == 1) {
            return true;
        }
        else return false;
    }

	// Method for checking if the room has a east exit
    public bool hasEExit() {
        if (eExit == 1) {
            return true;
        }
        else return false;
    }

	// Method for checking if the room has a south exit
	public bool hasSExit() {
        if (sExit == 1) {
            return true;
        }
        else return false;
    }

	// Method for checking if the room has a west exit
    public bool hasWExit() {
        if (wExit == 1) {
            return true;
        }
        else return false;
    }

	/* Method for setting the existance of north exit */
	public void setNexit(int exist) {
		nExit = exist;
	}

	/* Method for setting the existance of east exit */
	public void setEexit(int exist) {
		eExit = exist;
	}

	/* Method for setting the existance of south exit */
	public void setSexit(int exist) {
		sExit = exist;
	}

	/* Method for setting the existance of west exit */
	public void setWexit(int exist) {
		wExit = exist;
	}

	/* Sets this room as an enemy room */
	public void setRoomAsEnemy(){
		enemyRoom = true;
		chestRoom = false;
		ladderRoom = false;
		npcRoom = false;
		bossRoom = false;
		this.gameObject.name = "Enemy Room";
	}

	/* Sets this room as a chest room */
	public void setRoomAsChest(){
		enemyRoom = false;
		chestRoom = true;
		ladderRoom = false;
		npcRoom = false;
		bossRoom = false;
		this.gameObject.name = "Chest Room";
	}

	/* Sets this room as a ladder room */
	public void setRoomAsLadder(){
		enemyRoom = false;
		chestRoom = false;
		ladderRoom = true;
		npcRoom = false;
		bossRoom = false;
		this.gameObject.name = "Ladder Room";
	}

	/* Sets this room as an NPC room */
	public void setRoomAsNPC(){
		enemyRoom = false;
		chestRoom = false;
		ladderRoom = false;
		npcRoom = true;
		bossRoom = false;
		this.gameObject.name = "NPC Room";
	}

	public void setRoomAsBoss(){
		enemyRoom = false;
		chestRoom = false;
		ladderRoom = false;
		npcRoom = false;
		bossRoom = true;
		this.gameObject.name = "Boss Room";
	}

	/* Method to get the number of rows in the room */
	public int getRow(){
		return rows;
	}

	/* Method to get the number of columns in the room */
	public int getColumn(){
		return columns;
	}
}