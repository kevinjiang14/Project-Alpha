using UnityEngine;
using System.Collections;

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
    public int healthRegen = 5;
    public int freeAttrPoints = 5;
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

    private PlayerStats stats;

	void Awake(){
        stats = new PlayerStats();
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

		if (regenTimer >= stats.healthRegen) {
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
//		foreach (GameObject enemy in enemyList) {
//			if (Vector3.Distance (transform.position, enemy.transform.position) <= attackRange) {
//				enemy.GetComponent<Enemy> ().TakeDamage (damage);
//			}
//		}

		attackTimer = 0f;
	}

	// Respawn player
	public void Respawn(){
		Debug.Log ("You have died!");
		Debug.Log ("Respawning...");

        stats.CurrentHealth = maxHealth;
		// Perhaps some sort of repercussion for dying
		// Respawn player at some predetermined location
		ResetPlayerLocation();
	}

	// Resets player position to spawn position
	public void ResetPlayerLocation(){
		playerTransform.position =  new Vector3(82f, 49f, -0.01f);
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
            stats.CurrentHealth += 1;
		}
		regenTimer = 0;
	}

	public int getEXPtoLVL(){
		return expToLVLUp;
	}

	public int getPlayerLVL(){
		return stats.PlayerLevel;
	}

	public void LevelUp(){
        stats.PlayerLevel += 1;
        stats.freeAttrPoints += 5;
		expToLVLUp = 20 * ((stats.PlayerLevel * stats.PlayerLevel) + stats.PlayerLevel + 3);
		// Increase max health and return player back to full health
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
        stats.CurrentHealth = maxHealth;
	}

	public int getMaxHealth(){
		return maxHealth;
	}

	public int getHealth(){
		return stats.CurrentHealth;
	}

	public int getDamage(){
		return damage;
	}

	public int getEXP(){
		return stats.CurrentEXP;
	}

	public int getVitality(){
		return stats.vitality;
	}

	public void IncreaseVitality(){
		if (stats.freeAttrPoints > 0) {
            stats.vitality += 1;
            stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public int getStrength(){
		return stats.strength;
	}

	public void IncreaseStrength(){
		if (stats.freeAttrPoints > 0) {
            stats.strength += 1;
            stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public int getDexterity(){
		return stats.dexterity;
	}

	public void IncreaseDexterity(){
		if (stats.freeAttrPoints > 0) {
            stats.dexterity += 1;
            stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public int getDefense(){
		return stats.defense;
	}

	public void IncreaseDefense(){
		if (stats.freeAttrPoints > 0) {
            stats.defense += 1;
            stats.freeAttrPoints -= 1;
			UpdateStats ();
		}
	}

	public int getLuck(){
		return stats.luck;
	}

	public void IncreaseLuck(){
		if (stats.freeAttrPoints > 0) {
            stats.luck += 1;
            stats.freeAttrPoints -= 1;
            UpdateStats ();
		}
	}

    public PlayerStats getStats(){
        return stats;
    }

	public void UpdateStats(){
		damage = stats.strength / 5;
		maxHealth = (stats.PlayerLevel * 3) + stats.vitality;
	}

    public void UpdatePlayer(){
        ResetPlayerLocation();
        UpdateStats();
    }

	public int getFreePoints(){
		return stats.freeAttrPoints;
	}
}
