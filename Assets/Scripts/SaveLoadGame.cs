using UnityEngine;
using System.Collections;


public class SaveLoadGame {

    public static void Save() {
		// Save information to playerprefs
		SaveStats ();
		SaveFloor ();
		SaveEquipment ();
		SaveInventory ();

		// Save playerprefs to disk
        PlayerPrefs.Save();
    }
    public static void Load() {
		// Load infromation from playerprefs
		LoadFloor ();
		LoadEquipment ();
		LoadInventory ();
		LoadStats ();

		// Update Game
		GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().UpdateGame();
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().UpdatePlayer();
    }

	// Save Player Stats
	public static void SaveStats(){
		PlayerPrefs.SetInt("PlayerLevel", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().PlayerLevel);
		PlayerPrefs.SetInt("CurrentHealth", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().CurrentHealth);
		PlayerPrefs.SetInt("CurrentEXP", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().CurrentEXP);
		PlayerPrefs.SetInt("Vitality", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().vitality);
		PlayerPrefs.SetInt("Strength", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().strength);
		PlayerPrefs.SetInt("Defense", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().defense);
		PlayerPrefs.SetInt("Intelligence", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().intelligence);
		PlayerPrefs.SetInt("Wisdom", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().wisdom);
		PlayerPrefs.SetInt("Dexterity", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().dexterity);
		PlayerPrefs.SetInt("Luck", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().luck);
		PlayerPrefs.SetFloat("Speed", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().speed);
		PlayerPrefs.SetFloat("AttackRange", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().attackRange);
		PlayerPrefs.SetFloat("AttackSpeed", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().attackSpeed);
		PlayerPrefs.SetFloat("HealthRegenRate", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().healthRegenRate);
		PlayerPrefs.SetInt("HealthRegenAmount", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().healthRegenAmount);
		PlayerPrefs.SetFloat("ManaRegenRate", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().manaRegenRate);
		PlayerPrefs.SetInt("ManaRegenAmount", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().manaRegenAmount);
		PlayerPrefs.SetInt("FreeAttrPoints", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().freeAttrPoints);
		PlayerPrefs.SetInt("Gold", GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().gold);
	}

	// Load Player Stats
	public static void LoadStats(){
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().CurrentHealth = PlayerPrefs.GetInt("CurrentHealth");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().CurrentEXP = PlayerPrefs.GetInt("CurrentEXP");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().vitality = PlayerPrefs.GetInt("Vitality");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().strength = PlayerPrefs.GetInt("Strength");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().defense = PlayerPrefs.GetInt("Defense");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().intelligence = PlayerPrefs.GetInt("Intelligence");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().wisdom = PlayerPrefs.GetInt("Wisdom");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().dexterity = PlayerPrefs.GetInt("Dexterity");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().luck = PlayerPrefs.GetInt("Luck");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().speed = PlayerPrefs.GetFloat("Speed");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().attackRange = PlayerPrefs.GetFloat("AttackRange");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().attackSpeed = PlayerPrefs.GetFloat("AttackSpeed");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().healthRegenRate = PlayerPrefs.GetFloat("HealthRegenRate");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().healthRegenAmount = PlayerPrefs.GetInt("HealthRegenAmount");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().manaRegenRate = PlayerPrefs.GetFloat("ManaRegenRate");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().manaRegenAmount = PlayerPrefs.GetInt("ManaRegenAmount");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().freeAttrPoints = PlayerPrefs.GetInt("FreeAttrPoints");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getStats().gold = PlayerPrefs.GetInt("Gold");
	}

	// Save Floor Information
	public static void SaveFloor(){
		PlayerPrefs.SetInt("Floor", GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().floorLevel);
		PlayerPrefs.SetInt("Max", GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().MaxRooms);
		PlayerPrefs.SetInt("Min", GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().MinRooms);
	}

	// Load Floor Information
	public static void LoadFloor(){
		GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().floorLevel = PlayerPrefs.GetInt("Floor");
		GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().MaxRooms = PlayerPrefs.GetInt("Max");
		GameObject.FindGameObjectWithTag ("MapManager").GetComponent<MapManager> ().getMapInfo().MinRooms = PlayerPrefs.GetInt("Min");
	}

	// Save Player's Inventory
	public static void SaveInventory(){
		Inventory tempInventory = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getInventory ();
		string[] itemList = tempInventory.GetInventoryList ();

		PlayerPrefsX.SetStringArray ("Inventory", itemList);
		PlayerPrefsX.SetIntArray ("Quantity", tempInventory.GetQuantityList ());
	}

	// Load Player's Inventory
	public static void LoadInventory(){
		Inventory tempInventory = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().getInventory ();
		// Clear the player's current inventory
		tempInventory.ClearInventory ();

		string[] itemNameList = PlayerPrefsX.GetStringArray ("Inventory");
		int[] itemQuantityList = PlayerPrefsX.GetIntArray ("Quantity");

		// Iterate through saved items and add them to new inventory
		for (int i = 0; i < itemNameList.Length; i++) {
			GameObject item = (GameObject)Resources.Load ("Item/" + itemNameList[i]);
			tempInventory.AddtoInventory (item, itemQuantityList [i]);
		}
	}

	// Save Player's Equipment
	public static void SaveEquipment(){
		string[] equippedItems = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ().GetEquipment();
		// Save only if player had any items equipped
		PlayerPrefsX.SetStringArray ("Equpment", equippedItems);
	}

	// Load Player's Equipment
	public static void LoadEquipment(){
		Player playerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		// Clear the player's current equipments
		playerScript.ClearEquipment ();

		string[] equippedItems = PlayerPrefsX.GetStringArray ("Equpment");
		for (int i = 0; i < equippedItems.Length; i++) {
			GameObject item = (GameObject)Resources.Load ("Item/" + equippedItems [i]);
			playerScript.EquipItem (item);
		}
	}
}
