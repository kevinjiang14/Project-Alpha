using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : Component{
    public int PlayerLevel = 1;
    public int CurrentHealth;
    public int CurrentEXP;
    // Vitality increases health
    public int vitality = 10;
    // Strength increases damage
    public int strength = 10;
    // Dexterity increases critical rate and damge
    public int dexterity = 10;
    // Defense lowers damage taken
    public int defense = 5;
    // Luck increases rare item finds?
    public int luck = 5;
    public float speed = 4f;
    public float attackRange = 1f;
    public float attackSpeed = 1f;
    public float regenRate = 5f;
    public int freeAttrPoints = 5;
    public int regenAmount = 1;

    public Inventory inventory;

}

public class Player : MonoBehaviour{

	/* Non-Adjustable Attributes */
	// maxHealth = playerlevel * 3 + vitality
	private int maxHealth;
	// damage = strength * 5
	private int damage;
	// exp to level up = 20(x^2 + x + 3) where x = playerlvl
	private int expToLVLUp;

	/* Movement direction */
	private float movex = 0;
	private float movey = 0;

	/* Timers for player action */
	private float attackTimer;
	private float regenTimer;

	/* GameObjects dependent on player */
	private GameObject[] enemyList;

	/* Player transform */
	private Transform playerTransform;
    private Animator playerAnimation;

    /* Player data */
    private PlayerStats stats;
	private GameObject characterMenu;

	void Awake(){
        stats = new PlayerStats();
        stats.inventory = new Inventory();
		playerTransform = GetComponent<Transform> ();
        playerAnimation = GetComponent<Animator>();

		// Initialize non-adjustable attributes
		expToLVLUp = 20 * ((stats.PlayerLevel * stats.PlayerLevel) + stats.PlayerLevel + 3);
		stats.CurrentEXP = 0;
		damage = stats.strength / 5;
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
		stats.CurrentHealth = maxHealth;
	}

	void FixedUpdate(){
        // Timer increments
        attackTimer += Time.deltaTime;
        regenTimer += Time.deltaTime;

		if (regenTimer >= stats.regenRate) {
			HealthRegen();
		}

		Move ();

		// Get all enemies on the map
		enemyList = GameObject.FindGameObjectsWithTag("Enemy");

		// Check if attack button was pressed
		if (Input.GetButtonDown("Attack") && attackTimer >= stats.attackSpeed) {
			Attack ();
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
        else if (Input.GetAxisRaw("Horizontal") > 0){
            playerAnimation.SetInteger("Direction", 3);
            playerAnimation.SetFloat("Speed", 1.0f);
            movex = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * stats.speed);
            direction = new Vector3(movex, 0, 0);
            playerTransform.Translate(direction);
        }
        else if (Input.GetAxisRaw("Vertical") < 0){
            playerAnimation.SetInteger("Direction", 0);
            playerAnimation.SetFloat("Speed", 1.0f);
            movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * stats.speed);
            direction = new Vector3(0, movey, 0);
            playerTransform.Translate(direction);
        }
        else if (Input.GetAxisRaw("Vertical") > 0){
            playerAnimation.SetInteger("Direction", 2);
            playerAnimation.SetFloat("Speed", 1.0f);
            movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * stats.speed);
            direction = new Vector3(0, movey, 0);
            playerTransform.Translate(direction);
        }
        else playerAnimation.SetFloat("Speed", 0.0f);
    }

	// Player attack
	public void Attack(){
        // TODO: Have player only damage enemies it is facing
		// Reduce that enemy's health

		if (playerAnimation.GetInteger ("Direction") == 0) {
			foreach (GameObject enemy in enemyList) {
				if (enemy.transform.position.y >= transform.position.y - stats.attackRange && enemy.transform.position.y < transform.position.y && ((transform.position.y - enemy.transform.position.y > transform.position.x - enemy.transform.position.x && transform.position.x > enemy.transform.position.x) || (transform.position.y - enemy.transform.position.y > enemy.transform.position.x - transform.position.x && transform.position.x < enemy.transform.position.x))) {
					enemy.GetComponent<Enemy> ().TakeDamage (damage);
				}
			}
		}
		else if (playerAnimation.GetInteger ("Direction") == 1) {
			foreach (GameObject enemy in enemyList) {
				if (enemy.transform.position.x >= transform.position.x - stats.attackRange && enemy.transform.position.x < transform.position.x && ((transform.position.x - enemy.transform.position.x > transform.position.y - enemy.transform.position.y && transform.position.y > enemy.transform.position.y) || (transform.position.x - enemy.transform.position.x > enemy.transform.position.y - transform.position.y && transform.position.y < enemy.transform.position.y))) {
					enemy.GetComponent<Enemy> ().TakeDamage (damage);
				}
			}
		}
		else if (playerAnimation.GetInteger ("Direction") == 2) {
			foreach (GameObject enemy in enemyList) {
				if (enemy.transform.position.y <= transform.position.y + stats.attackRange && enemy.transform.position.y > transform.position.y && ((enemy.transform.position.y - transform.position.y > transform.position.x - enemy.transform.position.x && transform.position.x > enemy.transform.position.x) || (enemy.transform.position.y - transform.position.y > enemy.transform.position.x - transform.position.x && transform.position.x < enemy.transform.position.x))) {
					enemy.GetComponent<Enemy> ().TakeDamage (damage);
				}
			}
		}
		else if (playerAnimation.GetInteger ("Direction") == 3) {
			foreach (GameObject enemy in enemyList) {
				if (enemy.transform.position.x <= transform.position.x + stats.attackRange && enemy.transform.position.x > transform.position.x && ((enemy.transform.position.x - transform.position.x > transform.position.y - enemy.transform.position.y && transform.position.y > enemy.transform.position.y) || (enemy.transform.position.x - transform.position.x > enemy.transform.position.y - transform.position.y && transform.position.y < enemy.transform.position.y))) {
					enemy.GetComponent<Enemy> ().TakeDamage (damage);
				}
			}
		}
		attackTimer = 0f;
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
		// damage taken = incoming damage - defense / 7
		i = i - (stats.defense / 7);
		if (i >= 0) {
			if (stats.CurrentHealth - i <= 0) {
				Respawn ();
			} else
                stats.CurrentHealth -= i;
		}
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
            if (stats.CurrentHealth + stats.regenAmount > maxHealth){
                stats.CurrentHealth = maxHealth;
            } else stats.CurrentHealth += stats.regenAmount;

        }
		regenTimer = 0;
	}

	public void LevelUp(){
        stats.PlayerLevel += 1;
        stats.freeAttrPoints += 5;
		expToLVLUp = 20 * ((stats.PlayerLevel * stats.PlayerLevel) + stats.PlayerLevel + 3);
		// Increase max health and return player back to full health
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
        stats.CurrentHealth = maxHealth;
	}

    public void UseItem(GameObject item){
        Item itemScript = item.GetComponent<Item>();
        if (maxHealth - stats.CurrentHealth >= itemScript.healthrecovery)
        {
            stats.CurrentHealth += itemScript.healthrecovery;
            stats.inventory.DecreaseItemQuantity(item);
        } else{
            stats.CurrentHealth = maxHealth;
            stats.inventory.DecreaseItemQuantity(item);
        }
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
		} else if (item.tag == "Offhand") {
			characterMenu.transform.Find ("Offhand").GetComponent<EquipmentSlotManager> ().EquipItem (item);
		}

        // Check for bonus stats from items/potions/etc.
        IncreaseBonusStats(item);

    }

	public void UnequipItem(GameObject item){
        RemoveBonusStats(item);
	}

    public void IncreaseBonusStats(GameObject item){
        Item tempItemScript = item.GetComponent<Item>();
        stats.vitality += tempItemScript.vitality;
        stats.strength += tempItemScript.strength;
        stats.dexterity += tempItemScript.dexterity;
        stats.defense += tempItemScript.defense;
        stats.luck += tempItemScript.luck;
        stats.speed += tempItemScript.speed;
        stats.attackRange += tempItemScript.attackRange;
        stats.attackSpeed -= tempItemScript.attackSpeed;
        stats.regenRate -= tempItemScript.regenRate;
        stats.regenAmount += tempItemScript.regenAmount;
        UpdateStats();
    }

    public void RemoveBonusStats(GameObject item)
    {
        Item tempItemScript = item.GetComponent<Item>();
        stats.vitality -= tempItemScript.vitality;
        stats.strength -= tempItemScript.strength;
        stats.dexterity -= tempItemScript.dexterity;
        stats.defense -= tempItemScript.defense;
        stats.luck -= tempItemScript.luck;
        stats.speed -= tempItemScript.speed;
        stats.attackRange -= tempItemScript.attackRange;
        stats.attackSpeed += tempItemScript.attackSpeed;
        stats.regenRate += tempItemScript.regenRate;
        stats.regenAmount -= tempItemScript.regenAmount;
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

	public void IncreaseDexterity(){
		if (stats.freeAttrPoints > 0) {
            stats.dexterity += 1;
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

	public void IncreaseLuck(){
		if (stats.freeAttrPoints > 0) {
            stats.luck += 1;
            stats.freeAttrPoints -= 1;
            UpdateStats ();
		}
	}

    public void UpdateStats(){
		damage = stats.strength / 5;
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
        if(stats.CurrentHealth > maxHealth)
        {
            stats.CurrentHealth = maxHealth;
        }
	}

    public void UpdatePlayer(){
        ResetPlayerLocation();
        UpdateStats();
    }

    public PlayerStats getStats()
    {
        return stats;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getEXPtoLVL()
    {
        return expToLVLUp;
    }

    public int getDamage()
    {
        return damage;
    }

    public int getPlayerLVL()
    {
        return stats.PlayerLevel;
    }

    public int getHealth()
    {
        return stats.CurrentHealth;
    }

    public int getEXP()
    {
        return stats.CurrentEXP;
    }

    public int getVitality()
    {
        return stats.vitality;
    }

    public int getStrength()
    {
        return stats.strength;
    }

    public int getDexterity()
    {
        return stats.dexterity;
    }

    public int getDefense()
    {
        return stats.defense;
    }

    public int getLuck()
    {
        return stats.luck;
    }

    public int getFreePoints(){
		return stats.freeAttrPoints;
	}

    public Inventory getInventory(){
        return stats.inventory;
    }
}
