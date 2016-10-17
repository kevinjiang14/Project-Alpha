using UnityEngine;
using System.Collections;

public class MapInformation : Component {
    // Current floor level
    public int floorLevel = 1;
    // TODO: minimum and maximum should increase as player gets to lower floors
    public int MinRooms = 20;
    public int MaxRooms = 35;
}

public class MapManager : MonoBehaviour {

    public static MapManager instance = null;

    // Player's current menus
    private GameObject currentHotbar;
    private GameObject currentMonitor;
    private GameObject currentMenu;
    private GameObject currentInventory;
    private GameObject currentCharacter;

    // Map Validation variables
    private int numOfRooms = 0;
	private int MinRooms = 20;
	private int MaxRooms = 35;
	private bool validMap = false;

	// Map GameObject of the current map
	private GameObject Map;


	// Room Indexing variables
    private int index;
	private float startColumn = 5;
	private float startRow = 5;

	// Room GameObject list
	private GameObject[] roomList;
	private GameObject currentRoom;
	private bool ladderExist;

    // Public GameObjects to be instantiated
    public GameObject playerHotbar;
    public GameObject playerMonitorView;
    public GameObject playerMenu;
    public GameObject playerInventory;
    public GameObject playerCharacter;
    public GameObject roomObject;
    public GameObject playerList;
    public GameObject Camera;

	// Player GameObject that is instantiated
	private GameObject player;
    
    private MapInformation mapInfo;

    public void Awake(){
		// Verifying only one instance of MapManager is in existance
		if (instance == null) {
			instance = this;
		} 
		// If another exist then destroy this one
		else if (instance != this) {
			Destroy (gameObject);
		}

        mapInfo = new MapInformation();

		DontDestroyOnLoad (gameObject);

		spawnPlayer(startColumn, startRow);

		Initialization ();

        CreateMenus ();
	}

    public void Update(){
        if (Input.GetButtonDown("StatsMenu"))
        {
            if (currentMenu.activeSelf == false)
            {
                currentMenu.SetActive(true);
                currentCharacter.SetActive(false);
            }
            else
            {
                currentMenu.SetActive(false);
                currentCharacter.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if(currentInventory.activeSelf == false)
            {
                currentInventory.SetActive(true);
            }
            else
            {
                currentInventory.SetActive(false);
            }
        }

        if (Input.GetButtonDown("CharacterMenu"))
        {
            if (currentCharacter.activeSelf == false)
            {
                currentCharacter.SetActive(true);
                currentMenu.SetActive(false);
            }
            else
            {
                currentCharacter.SetActive(false);
            }
        }
    }

	/* 
	 * Initialization of new GameObjects being filled with rooms
	 * Call this method when creating a new map for any reason
	*/
	public void Initialization (){
		ladderExist = false;
		roomList = new GameObject[100];
		Map = new GameObject ("Map");
		MapInitization ();
		while (validMap == false) {
			IsMapOfValidSize ();
		}
	}

    // Map Initialization
    public void MapInitization(){
        CreateMap(null, startColumn, startRow);
    }

	// Recursively fills in the map with rooms
	public void CreateMap (GameObject room, float Tcolumn, float Trow){
		bool nExit = false;
		bool eExit = false;
		bool sExit = false;
		bool wExit = false;

		// First room will contain no room type hence an empty room
		if (room == null){
            index = (((int)Tcolumn) * 10) + ((int)Trow);
			currentRoom = Instantiate(roomObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
			currentRoom.GetComponent<RoomManager> ().SetPosition ((int)Tcolumn, (int)Trow);
			currentRoom.GetComponent<RoomManager>().CreateRoom();
            currentRoom.transform.Translate((int)Tcolumn * 15, (int)Trow * 9, 0);
			currentRoom.transform.SetParent (Map.transform);
			roomList[index] = currentRoom;
			numOfRooms += 1;
            CreateMap(currentRoom, Tcolumn, Trow);
        }
        else if ((int)Tcolumn * 10 + (int)Trow < 100 && (int)Tcolumn * 10 + (int)Trow > 0){
			RoomManager roomScript = room.GetComponent<RoomManager>();
			//TODO: OPTIMIZE! Recreate this with seperate method to reduce code duplication
            if (roomScript.hasNExit() == true && ((int)Tcolumn * 10) + ((int)Trow + 1) < 100 && (int)Trow <= 9){
                if (roomList[((int)Tcolumn * 10) + ((int)Trow + 1)] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Trow++;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject roomTemp = Instantiate(roomObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					RoomManager tempRoomManager = roomTemp.GetComponent<RoomManager> ();

					tempRoomManager.setSexit (1);
					sExit = true;
					// Check if new room is on edge to close off exits
					// This room is in the north-most edge
					if (Trow == 9){
						tempRoomManager.setNexit (0);
						nExit = true;
					}
					// This room is in the east-most edge
					if (Tcolumn == 9){
						tempRoomManager.setEexit (0);
						eExit = true;
					}
					// This room is in the west-most edge
					if (Tcolumn == 0){
						tempRoomManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring rooms of new room and if theres a connecting exit from neighbor to create exits for it
					// Check if north room exist and if so and has a south exit then create a north exit in this room
					if (nExit == false && roomList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
						GameObject northRoom = roomList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northRoom.GetComponent<RoomManager> ().hasSExit ()) {
							tempRoomManager.setNexit (1);
							nExit = true;
						} else {
							tempRoomManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if east room exist and if so and has a south exit then create a north exit in this room
					if (eExit == false && roomList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastRoom = roomList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastRoom.GetComponent<RoomManager> ().hasWExit ()) {
                            tempRoomManager.setEexit (1);
							eExit = true;
						} else {
							tempRoomManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;

					// Check if west room exist and if so and has a south exit then create a north exit in this room
					if (wExit == false && roomList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westRoom = roomList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westRoom.GetComponent<RoomManager> ().hasEExit ()) {
                            tempRoomManager.setWexit (1);
							wExit = true;
						} else {
							tempRoomManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					// Generate a random room type
					int roomType = UnityEngine.Random.Range (0, 10);

					if (roomType == 0 && ladderExist == false) {
						tempRoomManager.setRoomAsLadder ();
						ladderExist = true;
					} else if (roomType >= 1 && roomType <= 5) {
						tempRoomManager.setRoomAsEnemy ();
					} else if (roomType >= 6 && roomType <= 7) {
						tempRoomManager.setRoomAsChest ();
					} else if (roomType >= 8 && roomType <= 9) {
						tempRoomManager.setRoomAsNPC ();
					}
					tempRoomManager.SetPosition ((int)Tcolumn, (int)Trow);
                    tempRoomManager.CreateRoom();
                    roomTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					roomTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    roomList[index] = roomTemp;
                    CreateMap(roomTemp, Tcolumn, Trow);
                    Trow -= 1;
                }
            }
            if (roomScript.hasSExit() == true && ((int)Tcolumn) * 10 + ((int)Trow - 1) >= 0 && (int)Trow >= 0){
                if (roomList[((int)Tcolumn) * 10 + ((int)Trow - 1)] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Trow--;
                    index = (((int)Tcolumn) * 10) + (int)Trow;

					GameObject roomTemp = Instantiate(roomObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					RoomManager tempRoomManager = roomTemp.GetComponent<RoomManager> ();

					tempRoomManager.setNexit (1);
					nExit = true;
					// Check if new room is on edge to close off exits
					// This room is in the south-most edge
					if (Trow == 0){
						tempRoomManager.setSexit (0);
						sExit = true;
					}
					// This room is in the east-most edge
					if (Tcolumn == 9){
						tempRoomManager.setEexit (0);
						eExit = true;
					}
					// This room is in the west-most edge
					if (Tcolumn == 0){
						tempRoomManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring rooms of new room and if theres a connecting exit from neighbor to create exits for it
					// Check if south room exist and if so and has a north exit then create a south exit in this room
					if (sExit == false && roomList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southRoom = roomList [(int)Tcolumn * 10 + ((int)Trow - 1)];
						if (southRoom.GetComponent<RoomManager> ().hasNExit ()) {
                            tempRoomManager.setSexit (1);
							sExit = true;
						} else {
							tempRoomManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;
					
					// Check if east room exist and if so and has a south exit then create a north exit in this room
					if (eExit == false && roomList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastRoom = roomList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastRoom.GetComponent<RoomManager> ().hasWExit ()) {
                            tempRoomManager.setEexit (1);
							eExit = true;
						} else {
							tempRoomManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;

					// Check if west room exist and if so and has a south exit then create a north exit in this room
					if (wExit == false && roomList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westRoom = roomList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westRoom.GetComponent<RoomManager> ().hasEExit ()) {
                            tempRoomManager.setWexit (1);
							wExit = true;
						} else {
							tempRoomManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					// Generate a random room type
					int roomType = UnityEngine.Random.Range (0, 10);

					if (roomType == 0 && ladderExist == false) {
						tempRoomManager.setRoomAsLadder ();
						ladderExist = true;
					} else if (roomType >= 1 && roomType <= 5) {
						tempRoomManager.setRoomAsEnemy ();
					} else if (roomType >= 6 && roomType <= 7) {
						tempRoomManager.setRoomAsChest ();
					} else if (roomType >= 8 && roomType <= 9) {
						tempRoomManager.setRoomAsNPC ();
					}
					tempRoomManager.SetPosition ((int)Tcolumn, (int)Trow);
					tempRoomManager.CreateRoom();
                    roomTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					roomTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    roomList[index] = roomTemp;
                    CreateMap(roomTemp, Tcolumn, Trow);
                    Trow += 1;
                }
            }
            if (roomScript.hasEExit() == true && ((int)Tcolumn + 1) * 10 + (int)Trow < 100 && (int)Tcolumn <= 9){
                if (roomList[((int)Tcolumn + 1) * 10 + (int)Trow] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Tcolumn++;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject roomTemp = Instantiate(roomObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					RoomManager tempRoomManager = roomTemp.GetComponent<RoomManager> ();

					tempRoomManager.setWexit (1);
					wExit = true;
					// Check if new room is on edge to close off exits
					// This room is in the north-most edge
					if (Trow == 9){
						tempRoomManager.setNexit (0);
						nExit = true;
					}
					// This room is in the east-most edge
					if (Tcolumn == 9){
						tempRoomManager.setEexit (0);
						eExit = true;
					}
					// This room is in the south-most edge
					if (Trow == 0){
						tempRoomManager.setSexit (0);
						sExit = true;
					}

					// Check neighboring rooms of new room and if theres a connecting exit from neighbor to create exits for it
					// Check if north room exist and if so and has a south exit then create a north exit in this room
					if (nExit == false && roomList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
						GameObject northRoom = roomList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northRoom.GetComponent<RoomManager> ().hasSExit ()) {
                            tempRoomManager.setNexit (1);
							nExit = true;
						} else {
							tempRoomManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if east room exist and if so and has a south exit then create a north exit in this room
					if (eExit == false && roomList [((int)Tcolumn + 1) * 10 + (int)Trow] != null) {
						GameObject eastRoom = roomList [((int)Tcolumn + 1) * 10 + (int)Trow];
						if (eastRoom.GetComponent<RoomManager> ().hasWExit ()) {
                            tempRoomManager.setEexit (1);
							eExit = true;
						} else {
							tempRoomManager.setEexit (0);
							eExit = true;
						}
					} else
						eExit = true;
					
					// Check if south room exist and if so and has a north exit then create a south exit in this room
					if (sExit == false && roomList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southRoom = roomList [(int)Tcolumn * 10 + (int)Trow - 1];
						if (southRoom.GetComponent<RoomManager> ().hasNExit ()) {
                            tempRoomManager.setSexit (1);
							sExit = true;
						} else {
							tempRoomManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;

					// Generate a random room type
					int roomType = UnityEngine.Random.Range (0, 10);

					if (roomType == 0 && ladderExist == false) {
						tempRoomManager.setRoomAsLadder ();
						ladderExist = true;
					} else if (roomType >= 1 && roomType <= 5) {
						tempRoomManager.setRoomAsEnemy ();
					} else if (roomType >= 6 && roomType <= 7) {
						tempRoomManager.setRoomAsChest ();
					} else if (roomType >= 8 && roomType <= 9) {
						tempRoomManager.setRoomAsNPC ();
					}
					tempRoomManager.SetPosition ((int)Tcolumn, (int)Trow);
					tempRoomManager.CreateRoom();
                    roomTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					roomTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    roomList[index] = roomTemp;
                    CreateMap(roomTemp, Tcolumn, Trow);
                    Tcolumn -= 1;
                }
            }
            if (roomScript.hasWExit() == true && ((int)Tcolumn - 1) * 10 + (int)Trow >= 0 && (int)Tcolumn >= 0){
                if (roomList[((int)Tcolumn - 1) * 10 + (int)Trow] == null){
                    nExit = false;
                    eExit = false;
                    sExit = false;
                    wExit = false;
                    Tcolumn--;
                    index = ((int)Tcolumn * 10) + (int)Trow;

					GameObject roomTemp = Instantiate(roomObject, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
					RoomManager tempRoomManager = roomTemp.GetComponent<RoomManager> ();

					tempRoomManager.setEexit (1);
					eExit = true;
					// Check if new room is on edge to close off exits
					// This room is in the north-most edge
					if (Trow == 9){
						tempRoomManager.setNexit (0);
						nExit = true;
					}
					// This room is in the south-most edge
					if (Trow == 0){
						tempRoomManager.setSexit (0);
						sExit = true;
					}
					// This room is in the west-most edge
					if (Tcolumn == 0){
						tempRoomManager.setWexit (0);
						wExit = true;
					}

					// Check neighboring rooms of new room and if theres a connecting exit from neighbor to create exits for it
					// Check if north room exist and if so and has a south exit then create a north exit in this room
					if (nExit == false && roomList [(int)Tcolumn * 10 + (int)Trow + 1] != null) {
						GameObject northRoom = roomList [(int)Tcolumn * 10 + (int)Trow + 1];
						if (northRoom.GetComponent<RoomManager> ().hasSExit ()) {
							tempRoomManager.setNexit (1);
							nExit = true;
						} else {
							tempRoomManager.setNexit (0);
							nExit = true;
						}
					} else
						nExit = true;
					
					// Check if south room exist and if so and has a north exit then create a south exit in this room
					if (sExit == false && roomList [(int)Tcolumn * 10 + (int)Trow - 1] != null) {
						GameObject southRoom = roomList [((int)Tcolumn) * 10 + (int)Trow - 1];
						if (southRoom.GetComponent<RoomManager> ().hasNExit ()) {
                            tempRoomManager.setSexit (1);
							sExit = true;
						} else {
							tempRoomManager.setSexit (0);
							sExit = true;
						}
					} else
						sExit = true;

					// Check if west room exist and if so and has a south exit then create a north exit in this room
					if (wExit == false && roomList [((int)Tcolumn - 1) * 10 + (int)Trow] != null) {
						GameObject westRoom = roomList [((int)Tcolumn - 1) * 10 + (int)Trow];
						if (westRoom.GetComponent<RoomManager> ().hasEExit ()) {
                            tempRoomManager.setWexit (1);
							wExit = true;
						} else {
							tempRoomManager.setWexit (0);
							wExit = true;
						}
					} else
						wExit = true;

					// Generate a random room type
					int roomType = UnityEngine.Random.Range (0, 10);

					if (roomType == 0 && ladderExist == false) {
						tempRoomManager.setRoomAsLadder ();
						ladderExist = true;
					} else if (roomType >= 1 && roomType <= 5) {
						tempRoomManager.setRoomAsEnemy ();
					} else if (roomType >= 6 && roomType <= 7) {
						tempRoomManager.setRoomAsChest ();
					} else if (roomType >= 8 && roomType <= 9) {
						tempRoomManager.setRoomAsNPC ();
					}
					tempRoomManager.SetPosition ((int)Tcolumn, (int)Trow);
					tempRoomManager.CreateRoom();
                    roomTemp.transform.Translate(Tcolumn * 15f, Trow * 9f, 0f);
					roomTemp.transform.SetParent (Map.transform);
					numOfRooms += 1;

                    roomList[index] = roomTemp;
                    CreateMap(roomTemp, Tcolumn, Trow);
                    Tcolumn += 1;
                }
            }          
        }
	}

    // Spawns player
	public void spawnPlayer(float Pcolumn, float Prow){
		Vector3 playerInitPosition = new Vector3(Pcolumn * 15f + 7f, Prow * 9f + 4f, -0.01f);
        player = Instantiate(playerList, playerInitPosition, Quaternion.identity) as GameObject;
		SetCamera(player, playerInitPosition);
    }

    // Creates the camera to follow player
    public void SetCamera(GameObject player, Vector3 cameraPosition){
        GameObject playerCamera = Instantiate(Camera, new Vector3(cameraPosition.x, cameraPosition.y, -10f), Quaternion.identity) as GameObject;
        playerCamera.transform.SetParent(player.transform);
    }

    // Check if map has acceptable number of rooms
	public void IsMapOfValidSize(){
		if (numOfRooms >= MinRooms && numOfRooms <= MaxRooms) {
			validMap = true;
		} else {
			validMap = false;
			RecreateMap ();
		}
	}

    // Recreates the map
	public void RecreateMap(){
		Destroy (Map);
		numOfRooms = 0;
		validMap = false;
		Initialization ();
	}

    // Increase the floor
	public void IncreaseFloor(){
		mapInfo.floorLevel += 1;
	}

    // Get current floor
	public int getFloor(){
		return mapInfo.floorLevel;
	}

    // Gets MapInformation
    public MapInformation getMapInfo(){
        return mapInfo;
    }

    // Creates the player menus
    public void CreateMenus(){
        currentMenu = Instantiate(playerMenu);
        currentMenu.SetActive(false);
        currentInventory = Instantiate(playerInventory);
        currentInventory.SetActive(false);
        currentCharacter = Instantiate(playerCharacter);
        currentCharacter.SetActive(false);
        currentMonitor = Instantiate(playerMonitorView);
        currentHotbar = Instantiate(playerHotbar);
    }

    // Update game when game data is loaded
    public void UpdateGame(){
        RecreateMap();
    }

    public GameObject getCurrentHotbar(){
        return currentHotbar;
    }

    public GameObject getCurrentMonitor(){
        return currentMonitor;
    }

    public GameObject getCurrentMenu(){
        return currentMenu;
    }

    public GameObject getCurrentInventory(){
        return currentInventory;
    }

    public GameObject getCurrentCharacter(){
        return currentCharacter;
    }
}
