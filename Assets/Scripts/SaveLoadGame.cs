using UnityEngine;
using System.Collections;


public class SaveLoadGame {

    public static void Save() {
        PlayerPrefs.SetInt("PlayerLevel", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().PlayerLevel);
        PlayerPrefs.SetInt("CurrentHealth", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().CurrentHealth);
        PlayerPrefs.SetInt("CurrentEXP", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().CurrentEXP);
        PlayerPrefs.SetInt("Vitality", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().vitality);
        PlayerPrefs.SetInt("Strength", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().strength);
        PlayerPrefs.SetInt("Dexterity", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().dexterity);
        PlayerPrefs.SetInt("Defense", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().defense);
        PlayerPrefs.SetInt("Luck", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().luck);
        PlayerPrefs.SetFloat("Speed", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().speed);
        PlayerPrefs.SetFloat("AttackRange", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().attackRange);
        PlayerPrefs.SetFloat("AttackSpeed", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().attackSpeed);
        PlayerPrefs.SetFloat("RegenRate", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().regenRate);
        PlayerPrefs.SetInt("FreeAttrPoints", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().freeAttrPoints);

        PlayerPrefs.SetInt("Floor", GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().floorLevel);
        PlayerPrefs.SetInt("Max", GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().MaxRooms);
        PlayerPrefs.SetInt("Min", GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().MinRooms);

        PlayerPrefs.Save();
    }
    public static void Load() {  
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().CurrentHealth = PlayerPrefs.GetInt("CurrentHealth");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().CurrentEXP = PlayerPrefs.GetInt("CurrentEXP");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().vitality = PlayerPrefs.GetInt("Vitality");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().strength = PlayerPrefs.GetInt("Strength");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().dexterity = PlayerPrefs.GetInt("Dexterity");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().defense = PlayerPrefs.GetInt("Defense");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().luck = PlayerPrefs.GetInt("Luck");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().speed = PlayerPrefs.GetFloat("Speed");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().attackRange = PlayerPrefs.GetFloat("AttackRange");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().attackSpeed = PlayerPrefs.GetFloat("AttackSpeed");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().regenRate = PlayerPrefs.GetFloat("RegenRate");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getStats().freeAttrPoints = PlayerPrefs.GetInt("FreeAttrPoints");

        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().floorLevel = PlayerPrefs.GetInt("Floor");
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().MaxRooms = PlayerPrefs.GetInt("Max");
        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getMapInfo().MinRooms = PlayerPrefs.GetInt("Min");

        GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().UpdateGame();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdatePlayer();
    }
}
