using UnityEngine;
using System.Collections;

public class Equipment : Component{
    public int ItemID { get; set; }

    public int Vitality { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Defense { get; set; }
    public int Luck { get; set; }
    public int Speed { get; set; }
    public float AttackRange { get; set; }
    public float AttackSpeed { get; set; }
    public int HealthRegen { get; set; }
    public int RegenAmount { get; set; }
}

public class Consumable : Component{
    public int ItemID { get; set; }

    public int HealthRecovery { get; set; }
}


public class Item : MonoBehaviour {

    /* Item Types:
     * 0 = Consumables (potions)
     * 1 = Equipables (armor, weapons)
     * 2 = Everything else?
     */
    public int ItemType;

    public int itemID;

    // Stats if item is equipable
    public int vitality;
    public int strength;
    public int dexterity;
    public int defense;
    public int luck;
    public int speed;
    public float attackRange;
    public float attackSpeed;
    public int healthRegen;
    public int regenAmount;

    // Benefits if item is consumable
    public int healthrecovery;

    private Equipment equipment;
    private Consumable consumable;
    
	// Use this for initialization
	void Start () {
	    if(ItemType == 0){
            consumable = new Consumable();
            consumable.ItemID = itemID;
            consumable.HealthRecovery = healthrecovery;
        } else if(ItemType == 1){
            equipment = new Equipment();
            equipment.ItemID = itemID;
            equipment.Vitality = vitality;
            equipment.Strength = strength;
            equipment.Dexterity = dexterity;
            equipment.Defense = defense;
            equipment.Luck = luck;
            equipment.Speed = speed;
            equipment.AttackRange = attackRange;
            equipment.AttackSpeed = attackSpeed;
            equipment.HealthRegen = healthRegen;
            equipment.RegenAmount = regenAmount;
        }
	}
}
