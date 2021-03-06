﻿using UnityEngine;
using System.Collections;

public class Equipment : Component{
    public int ItemID { get; set; }
	public string ItemName { get; set; }
    public int Chance { get; set; }
    public int Cost { get; set; }

    public int Vitality { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Defense { get; set; }
    public int Luck { get; set; }
    public int Speed { get; set; }
    public float AttackRange { get; set; }
    public float AttackSpeed { get; set; }
    public float HealthRegen { get; set; }
    public int RegenAmount { get; set; }
}

public class Consumable : Component{
	public string ItemName { get; set; }
    public int ItemID { get; set; }
    public int Chance { get; set; }
    public int Cost { get; set; }

    public int HealthRecovery { get; set; }
    public int ManaRecovery { get; set; }
}


public class Item : MonoBehaviour {

    /* Item Types:
     * 0 = Consumables (potions)
     * 1 = Equipables (armor, weapons)
     * 2 = Everything else?
     */
    public int ItemType;

	public string itemName;
    public int itemID;
    public int chance;
    public int cost;

    // Stats if item is equipable
    public int vitality;
    public int strength;
    public int dexterity;
    public int defense;
    public int luck;
    public int speed;
    public float attackRange;
    public float attackSpeed;
    public float regenRate;
    public int regenAmount;

    // Benefits if item is consumable
    public int healthRecovery;
    public int manaRecovery;

    private Equipment equipment;
    private Consumable consumable;
    
	// Use this for initialization
	void Start () {
	    if(ItemType == 0){
            consumable = new Consumable();
			consumable.ItemName = itemName;
            consumable.ItemID = itemID;
            consumable.Chance = chance;
            consumable.Cost = cost;

            consumable.HealthRecovery = healthRecovery;
            consumable.ManaRecovery = manaRecovery;
        } else if(ItemType == 1){
            equipment = new Equipment();
			equipment.ItemName = itemName;
            equipment.ItemID = itemID;
            equipment.Chance = chance;
            equipment.Cost = cost;

            equipment.Vitality = vitality;
            equipment.Strength = strength;
            equipment.Dexterity = dexterity;
            equipment.Defense = defense;
            equipment.Luck = luck;
            equipment.Speed = speed;
            equipment.AttackRange = attackRange;
            equipment.AttackSpeed = attackSpeed;
            equipment.HealthRegen = regenRate;
            equipment.RegenAmount = regenAmount;
        }
	}
}
