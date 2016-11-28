using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : Component{
	public int PlayerLevel = 1;
	public int CurrentHealth;
	public int CurrentMana;
	public int CurrentEXP;
	// Vitality increases health
	public int vitality = 10;
	// Strength increases damage
	public int strength = 10;
	// Defense lowers damage taken
	public int defense = 5;
	// Intelligence increase magic damage
	public int intelligence = 10;
	// Wisdom increases mana
	public int wisdom = 5;
	// Dexterity increases range damge
	public int dexterity = 10;
	// Luck increases gold drop and rare item finds?
	public int luck = 5;
	public float speed = 4f;
	public float attackRange = 1f;
	public float attackSpeed = 1f;
	public float healthRegenRate = 5f;
	public int healthRegenAmount = 1;
	public float manaRegenRate = 3f;
	public int manaRegenAmount = 1;
	public int freeAttrPoints = 5;
	public int skillPoints = 10;
	public int gold = 100;

	public Inventory inventory;

}

public class Player : MonoBehaviour{

	/* Non-Adjustable Attributes */
	// MaxHealth = playerlevel * 3 + vitality
	private int maxHealth;
	// MaxMana = Wisdom * 2 + 10
	private int maxMana;
	// Melee Damage = strength / 5
	private int meleeDamage;
	// Range Damage = dexterity / 5
	private int rangeDamage;
	// Magic Damage = intelligence / 5
	private int magicDamage;
	// EXP to level up = 20(x^2 + x + 3) where x = playerlvl
	private int expToLVLUp;
	// Player's attack style 0 = melee, 1 = range, 2 = magic
	private int attackStyle = 0;

	/* Movement direction */
	private float movex = 0;
	private float movey = 0;

	/* Timers for player action */
	private float attackTimer;
	private float healthRegenTimer;
	private float manaRegenTimer;

	/* Player transform */
	private Transform playerTransform;
	private Animator playerAnimation;

	/* Player data */
	private PlayerStats stats;
	private GameObject characterMenu;

	private bool attackFlag = false;

	void Awake(){
		stats = new PlayerStats();
		stats.inventory = new Inventory();
		// Temporary bow given to player for testing
		stats.inventory.AddtoInventory ((GameObject)Resources.Load ("Item/128"), 1);
		// Start player off with a sword and shield
		stats.inventory.AddtoInventory ((GameObject)Resources.Load ("Item/92"), 1);
		stats.inventory.AddtoInventory ((GameObject)Resources.Load ("Item/150"), 1);

		playerTransform = GetComponent<Transform> ();
		playerAnimation = GetComponent<Animator>();

		// Initialize non-adjustable attributes
		expToLVLUp = 20 * ((stats.PlayerLevel * stats.PlayerLevel) + stats.PlayerLevel + 3);
		stats.CurrentEXP = 0;
		UpdateStats ();
		stats.CurrentHealth = maxHealth;
		stats.CurrentMana = maxMana;
	}

	void FixedUpdate(){
		// Timer increments
		attackTimer += Time.deltaTime;
		healthRegenTimer += Time.deltaTime;
		manaRegenTimer += Time.deltaTime;

		if (healthRegenTimer >= stats.healthRegenRate) {HealthRegen();}

		if (manaRegenTimer >= stats.manaRegenRate) {ManaRegen();}

		Move ();

		// Check if attack button was pressed
		if (Input.GetButtonDown ("Attack") && attackTimer >= stats.attackSpeed) {
			Attack ();
		} else if (attackFlag) {
			DeactivateHitBox ();
			attackFlag = false;
		}
	}

	// Player movement
	public void Move(){
		Vector3 direction;

		if (Input.GetAxisRaw("Horizontal") < 0){
			playerAnimation.SetInteger("Direction", 1);
			playerAnimation.SetFloat("Speed", 1.0f);
			movex = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * stats.speed);
			direction = new Vector3(movex, 0, 0);
			playerTransform.Translate(direction);
		}
		if (Input.GetAxisRaw("Horizontal") > 0){
			playerAnimation.SetInteger("Direction", 3);
			playerAnimation.SetFloat("Speed", 1.0f);
			movex = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * stats.speed);
			direction = new Vector3(movex, 0, 0);
			playerTransform.Translate(direction);
		}
		if (Input.GetAxisRaw("Vertical") < 0){
			playerAnimation.SetInteger("Direction", 0);
			playerAnimation.SetFloat("Speed", 1.0f);
			movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * stats.speed);
			direction = new Vector3(0, movey, 0);
			playerTransform.Translate(direction);
		}
		if (Input.GetAxisRaw("Vertical") > 0){
			playerAnimation.SetInteger("Direction", 2);
			playerAnimation.SetFloat("Speed", 1.0f);
			movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * stats.speed);
			direction = new Vector3(0, movey, 0);
			playerTransform.Translate(direction);
		}
		if(Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) {
			playerAnimation.SetFloat("Speed", 0.0f);
		}
	}

	// Player attack
	public void Attack(){
		attackFlag = true;
		// Reduce that enemy's health
		if (attackStyle == 0) {
			// MELEE ATTACKS
			if (playerAnimation.GetInteger ("Direction") == 0) {
				transform.Find ("South").gameObject.SetActive (true);
			} else if (playerAnimation.GetInteger ("Direction") == 1) {
				transform.Find ("West").gameObject.SetActive (true);
			} else if (playerAnimation.GetInteger ("Direction") == 2) {
				transform.Find ("North").gameObject.SetActive (true);
			} else if (playerAnimation.GetInteger ("Direction") == 3) {
				transform.Find ("East").gameObject.SetActive (true);
			}
		} else if (attackStyle == 1) {
			// RANGED ATTACKS
			Vector3 projectilePos = transform.position;
			Quaternion rotation = Quaternion.Euler(0, 0, 0);
			if (playerAnimation.GetInteger ("Direction") == 0) {
				projectilePos.y -= 0.6f;
				rotation = Quaternion.Euler(0, 0, 90);
			} else if (playerAnimation.GetInteger ("Direction") == 1) {
				projectilePos.x -= 0.6f;
				rotation = Quaternion.Euler(0, 0, 0);
			} else if (playerAnimation.GetInteger ("Direction") == 2) {
				projectilePos.y += 0.6f;
				rotation = Quaternion.Euler(0, 0, -90);
			} else if (playerAnimation.GetInteger ("Direction") == 3) {
				projectilePos.x += 0.6f;
				rotation = Quaternion.Euler(0, 0, 180);
			} 
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), projectilePos, rotation);
		} else if(attackStyle == 0){
			// MAGIC ATTACKS

		}
		attackTimer = 0f;
	}

	public void DeactivateHitBox() {
		transform.Find("South").gameObject.SetActive(false);
		transform.Find("West").gameObject.SetActive(false);
		transform.Find("North").gameObject.SetActive(false);
		transform.Find("East").gameObject.SetActive(false);
	}

	// Respawn player
	public void Respawn(){
		Debug.Log ("You have died!");
		Debug.Log ("Respawning...");

		stats.CurrentHealth = maxHealth;
		// TODO:Perhaps some sort of repercussion for dying
		// Respawn player at some predetermined location
		ResetPlayerLocation();
	}

	// Resets player position to spawn position
	public void ResetPlayerLocation(){
		playerTransform.position =  new Vector3(77f, 44f, -0.01f);
	}

	// PLayer taking damage
	public void TakeDamage(int i){
		// damage taken = incoming damage - defense / 5
		i = i - (stats.defense / 5);
		// Ensures the player always take 1 point of damage no matter how high their defense is
		if (i < 1) {i = 1;}

		if (i >= 0) {
			if (stats.CurrentHealth - i <= 0) {Respawn ();}
			else stats.CurrentHealth -= i;
		}
	}

	// PLayer use mana
	public bool UseMana(int i){
		if (stats.CurrentMana >= i) {
			stats.CurrentMana -= i;
			return true;
		} else return false;
	}

	// Player EXP gain
	public void GainEXP(int i){
		stats.CurrentEXP += i;

		if (stats.CurrentEXP >= expToLVLUp) {
			stats.CurrentEXP -= expToLVLUp;
			LevelUp ();
		}
	}

	// Player health regeneration
	public void HealthRegen(){
		if (stats.CurrentHealth < maxHealth) {
			if (stats.CurrentHealth + stats.healthRegenAmount > maxHealth){stats.CurrentHealth = maxHealth;}
			else stats.CurrentHealth += stats.healthRegenAmount;
		}
		healthRegenTimer = 0;
	}

	// Player mana regeneration
	public void ManaRegen(){
		if (stats.CurrentMana < maxMana) {
			if (stats.CurrentMana + stats.manaRegenAmount > maxMana){stats.CurrentMana = maxMana;}
			else stats.CurrentMana += stats.manaRegenAmount;
		}
		manaRegenTimer = 0;
	}

	public void LevelUp(){
		stats.PlayerLevel += 1;
		stats.freeAttrPoints += 5;
		stats.skillPoints += 1;
		expToLVLUp = 20 * ((stats.PlayerLevel * stats.PlayerLevel) + stats.PlayerLevel + 3);
		// Increase max health and return player back to full health
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
		stats.CurrentHealth = maxHealth;
		stats.CurrentMana = maxMana;
	}

	public void UseItem(GameObject item){
		Item itemScript = item.GetComponent<Item>();
		if (maxHealth - stats.CurrentHealth >= itemScript.healthRecovery) {stats.CurrentHealth += itemScript.healthRecovery;}
		else stats.CurrentHealth = maxHealth;

		if (maxMana - stats.CurrentMana >= itemScript.manaRecovery) {stats.CurrentMana += itemScript.manaRecovery;}
		else stats.CurrentMana = maxMana;

		stats.inventory.DecreaseItemQuantity(item);
	}

	// Method call to sell item, decreasing item quantity and giving player gold
	public void SellItem(GameObject Item){
		IncreaseGold (Item.GetComponent<Item> ().cost / 2);
		getInventory ().DecreaseItemQuantity (Item);
	}

	// TODO: Possibly implement requirements to equip item
	public void EquipItem(GameObject item){
		characterMenu = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getCurrentCharacter();
		characterMenu.SetActive(true);
		if (item.tag == "Head") {
			characterMenu.transform.Find ("Head").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Body") {
			characterMenu.transform.Find ("Body").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Hand") {
			characterMenu.transform.Find ("Hand").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Leg") {
			characterMenu.transform.Find ("Leg").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Feet") {
			characterMenu.transform.Find ("Feet").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Crown") {
			characterMenu.transform.Find ("Accessory1").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Necklace") {
			characterMenu.transform.Find ("Accessory2").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Ring") {
			characterMenu.transform.Find ("Accessory3").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		} else if (item.tag == "Mainhand") {
			characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().EquipItem (item);
			if(item.GetComponent<Item>().ItemType == 20){
				attackStyle = 2;
				playerAnimation.SetInteger ("AttackType", 2);
			}
			else if(item.GetComponent<Item>().ItemType == 10){
				attackStyle = 1;
				playerAnimation.SetInteger ("AttackType", 1);
			}
			else if(item.GetComponent<Item>().ItemType < 10){
				attackStyle = 0;
				playerAnimation.SetInteger ("AttackType", 0);
			}
		} else if (item.tag == "Offhand") {
			characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().EquipItem (item);
			if (characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
				if (characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().tag == "2-Handed") {
					characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().UnequipItem ();
					attackStyle = 0;
				}
			}
		} else if (item.tag == "2-Handed") {
			characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().EquipItem (item);
			characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().UnequipItem ();
			playerAnimation.SetInteger ("AttackType", item.GetComponent<Item> ().ItemType);
			if(item.GetComponent<Item>().ItemType == 20){
				attackStyle = 2;
				playerAnimation.SetInteger ("AttackType", 2);
			}
			else if(item.GetComponent<Item>().ItemType == 10){
				attackStyle = 1;
				playerAnimation.SetInteger ("AttackType", 1);
			}
			else if(item.GetComponent<Item>().ItemType < 10){
				attackStyle = 0;
				playerAnimation.SetInteger ("AttackType", 0);
			}
		}
		// Check for bonus stats from items/potions/etc.
		IncreaseBonusStats(item);
	}

	public void UnequipItem(GameObject item){
		RemoveBonusStats(item);
		if (item.tag == "Mainhand" || item.tag == "2-Handed") {
			// Reset player's attack animation to hand
			playerAnimation.SetInteger ("AttackType", 0);
		}
	}

	public string[] GetEquipment(){
		characterMenu = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getCurrentCharacter();
		string[] unfittedEquipments = new string[10];
		int counter = 0;
		if (characterMenu.transform.Find ("Head").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Head").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Body").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Body").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Hand").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Hand").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Leg").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Leg").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Feet").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Feet").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Accessory1").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Accessory1").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Accessory2").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Accessory2").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Accessory3").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Accessory3").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		if (characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem () != null) {
			unfittedEquipments [counter] = characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().GetEquippedItem ().name;
			counter++;
		}
		string[] tempEquipment = new string[counter];
		for (int i = 0; i < counter; i++) {
			tempEquipment [i] = unfittedEquipments [i];
		}
		return tempEquipment;
	}

	public void ClearEquipment(){
		characterMenu = GameObject.FindGameObjectWithTag("MapManager").GetComponent<MapManager>().getCurrentCharacter();
		characterMenu.transform.Find ("Head").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Body").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Hand").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Leg").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Feet").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Accessory1").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Accessory2").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Accessory3").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Mainhand").GetComponent<EquipmentSlotManager> ().UnequipItem ();
		characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().UnequipItem ();
	}

	public void IncreaseBonusStats(GameObject item){
		Item tempItemScript = item.GetComponent<Item>();
		stats.vitality += tempItemScript.vitality;
		stats.strength += tempItemScript.strength;
		stats.defense += tempItemScript.defense;
		stats.intelligence += tempItemScript.intelligence;
		stats.wisdom += tempItemScript.wisdom;
		stats.dexterity += tempItemScript.dexterity;
		stats.luck += tempItemScript.luck;
		stats.speed += tempItemScript.speed;
		stats.attackRange += tempItemScript.attackRange;
		stats.attackSpeed -= tempItemScript.attackSpeed;
		stats.healthRegenRate -= tempItemScript.healthRegenRate;
		stats.healthRegenAmount += tempItemScript.healthRegenAmount;
		UpdateStats();
	}

	public void RemoveBonusStats(GameObject item)
	{
		Item tempItemScript = item.GetComponent<Item>();
		stats.vitality -= tempItemScript.vitality;
		stats.strength -= tempItemScript.strength;
		stats.defense -= tempItemScript.defense;
		stats.intelligence -= tempItemScript.intelligence;
		stats.wisdom -= tempItemScript.wisdom;
		stats.dexterity -= tempItemScript.dexterity;
		stats.luck -= tempItemScript.luck;
		stats.speed -= tempItemScript.speed;
		stats.attackRange -= tempItemScript.attackRange;
		stats.attackSpeed += tempItemScript.attackSpeed;
		stats.healthRegenRate += tempItemScript.healthRegenRate;
		stats.healthRegenAmount -= tempItemScript.healthRegenAmount;
		stats.manaRegenRate += tempItemScript.manaRegenRate;
		stats.manaRegenAmount -= tempItemScript.manaRegenAmount;
		UpdateStats();
	}

	public void IncreaseVitality(){
		if (stats.freeAttrPoints > 0) {
			stats.vitality += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseStrength(){
		if (stats.freeAttrPoints > 0) {
			stats.strength += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseDefense(){
		if (stats.freeAttrPoints > 0) {
			stats.defense += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseIntelligence(){
		if (stats.freeAttrPoints > 0) {
			stats.intelligence += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseWisdom(){
		if (stats.freeAttrPoints > 0) {
			stats.wisdom += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseDexterity(){
		if (stats.freeAttrPoints > 0) {
			stats.dexterity += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public void IncreaseLuck(){
		if (stats.freeAttrPoints > 0) {
			stats.luck += 1;
			stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	// Decrease skill point by 1
	public void DecreaseSkillPoint(){
		stats.skillPoints -= 1;
	}

	public void UpdateStats(){
		meleeDamage = stats.strength / 5;
		rangeDamage = stats.dexterity / 10;
		magicDamage = stats.intelligence / 10;
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
		maxMana = stats.wisdom * 2 + 10;
		if(stats.CurrentHealth > maxHealth){stats.CurrentHealth = maxHealth;}
		if(stats.CurrentMana > maxMana){stats.CurrentMana = maxMana;}
	}

	public void UpdatePlayer(){
		ResetPlayerLocation();
		UpdateStats();
	}

	public void IncreaseGold(int amount){stats.gold += amount;}

	public void DecreaseGold(int amount){stats.gold -= amount;}

	public PlayerStats getStats(){return stats;}

	public int getMeleeDamage(){return meleeDamage;}

	public int getRangeDamage(){return rangeDamage;}

	public int getMagicDamage(){return magicDamage;}

	public int getMaxHealth(){return maxHealth;}

	public int getMaxMana(){return maxMana;}

	public int getEXPtoLVL(){return expToLVLUp;}

	public int getPlayerLVL(){return stats.PlayerLevel;}

	public int getCurrentHealth(){return stats.CurrentHealth;}

	public int getCurrentMana(){return stats.CurrentMana;}

	public int getCurrentEXP(){return stats.CurrentEXP;}

	public int getVitality(){return stats.vitality;}

	public int getStrength(){return stats.strength;}

	public int getDefense(){return stats.defense;}

	public int getIntelligence(){return stats.intelligence;}

	public int getWisdom(){return stats.wisdom;}

	public int getDexterity(){return stats.dexterity;}

	public int getLuck(){return stats.luck;}

	public float getSpeed(){return stats.speed;}

	public float getAttackRange(){return stats.attackRange;}

	public float getAttackSpeed(){return stats.attackSpeed;}

	public float getHealthRegenRate(){return stats.healthRegenRate;}

	public int getHealthRegenAmount(){return stats.healthRegenAmount;}

	public float getManaRegenRate(){return stats.manaRegenRate;}

	public int getManaRegenAmount(){return stats.manaRegenAmount;}

	public int getFreePoints(){return stats.freeAttrPoints;}

	public int getSkillPoints(){return stats.skillPoints;}

	public int getGold(){return stats.gold;}

	public int getAttackStyle(){return attackStyle;}

	public void IncreaseDefenseTemp(int bonus){stats.defense += bonus;}

	public void DecreaseDefenseTemp(int bonus){stats.defense -= bonus;}

	public void IncreaseStrengthTemp(int bonus){stats.strength += bonus;}

	public void DecreaseStrengthTemp(int bonus){stats.strength -= bonus;}

	public void IncreaseDexterityTemp(int bonus){stats.dexterity += bonus;}

	public void DecreaseDexterityTemp(int bonus){stats.dexterity -= bonus;}

	public void IncreaseMeleeDamage(int bonus){meleeDamage += bonus;}

	public void DecreaseMeleeDamage(int bonus){meleeDamage -= bonus;}

	public void IncreaseRangeDamage(int bonus){rangeDamage += bonus;}

	public void DecreaseRangeDamage(int bonus){rangeDamage -= bonus;}

	public void IncreaseMagicDamage(int bonus){magicDamage += bonus;}

	public void DecreaseMagicDamage(int bonus){magicDamage -= bonus;}

	public Inventory getInventory(){return stats.inventory;}
}