using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{

	/* Base-Attributes */
	private float speed = 4f;
	private int playerLevel = 1;
	// Vitality increases health
    private int vitality = 10;
	// Strength increases damage
    private int strength = 10;
	// Dexterity increases critical rate and damge
    private int dexterity = 10;
	// Defense lowers damage taken
    private int defense = 5;
	// Luck increases rare item finds?
    private int luck = 5;

	/* Non-Adjustable Attributes */
	// maxHealth = playerlevel * 3 + vitality
	private int maxHealth;
	private int currentHealth;
	// damage = strength * 5
	private int damage;
	private int currentEXP;
	// exp to level up = 20(x^2 + x + 3) where x = playerlvl
	private int expToLVLUp;
	// Miscellaneous
	private float attackRange = 1.5f;
	private float attackSpeed = 1;
	private int healthRegen = 5;
	private int freeAttrPoints = 0;

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

	void Awake(){
		playerTransform = GetComponent<Transform> ();
        playerAnimation = GetComponent<Animator>();

		// Initialize non-adjustable attributes
		expToLVLUp = 20 * ((playerLevel * playerLevel) + playerLevel + 3);
		currentEXP = 0;
		damage = strength / 5;
		maxHealth = (playerLevel * 3) + vitality;
		currentHealth = maxHealth;
	}

	void FixedUpdate(){
		// Timer increments
		attackTimer += Time.deltaTime;
		regenTimer += Time.deltaTime;

		if (regenTimer >= healthRegen) {
			HealthRegen();
		}

		Move ();

		// Get all enemies on the map
		enemyList = GameObject.FindGameObjectsWithTag("Enemy");

		// Check if attack button was pressed
		if (Input.GetButtonDown("Attack") && attackTimer >= attackSpeed) {
			Attack ();
		}
	}

	public void Move(){
        Vector3 direction;

        if (Input.GetAxisRaw("Horizontal") < 0){
            playerAnimation.SetInteger("Direction", 1);
            playerAnimation.SetFloat("Speed", 1.0f);
            movex = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * speed);
            direction = new Vector3(movex, 0, 0);
            playerTransform.Translate(direction);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0){
            playerAnimation.SetInteger("Direction", 3);
            playerAnimation.SetFloat("Speed", 1.0f);
            movex = Input.GetAxisRaw("Horizontal") * (Time.deltaTime * speed);
            direction = new Vector3(movex, 0, 0);
            playerTransform.Translate(direction);
        }
        else if (Input.GetAxisRaw("Vertical") < 0){
            playerAnimation.SetInteger("Direction", 0);
            playerAnimation.SetFloat("Speed", 1.0f);
            movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * speed);
            direction = new Vector3(0, movey, 0);
            playerTransform.Translate(direction);
        }
        else if (Input.GetAxisRaw("Vertical") > 0){
            playerAnimation.SetInteger("Direction", 2);
            playerAnimation.SetFloat("Speed", 1.0f);
            movey = Input.GetAxisRaw("Vertical") * (Time.deltaTime * speed);
            direction = new Vector3(0, movey, 0);
            playerTransform.Translate(direction);
        }
        else playerAnimation.SetFloat("Speed", 0.0f);
    }

	// Player attack
	public void Attack(){
        // TODO: Have player only damage enemies it is facing
		// Reduce that enemy's health 
		foreach (GameObject enemy in enemyList) {
			if (Vector3.Distance (transform.position, enemy.transform.position) <= attackRange) {
				enemy.GetComponent<Enemy> ().TakeDamage (damage);
			}
		}

		attackTimer = 0f;
	}

	// Respawn player
	public void Respawn(){
		Debug.Log ("You have died!");
		Debug.Log ("Respawning...");

		currentHealth = maxHealth;
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
		i = i - (defense / 7);
		if (i >= 0) {
			if (currentHealth - i <= 0) {
				Respawn ();
			} else
				currentHealth -= i;
		}
	}

	// Player EXP gain
	public void GainEXP(int i){
		currentEXP += i;

		if (currentEXP >= expToLVLUp) {
			currentEXP -= expToLVLUp;
			LevelUp ();
		}
	}

	// Player health regeneration
	public void HealthRegen(){
		if (currentHealth < maxHealth) {
			currentHealth += 1;
		}
		regenTimer = 0;
	}

	public int getEXPtoLVL(){
		return expToLVLUp;
	}

	public int getPlayerLVL(){
		return playerLevel;
	}

	public void LevelUp(){
		playerLevel += 1;
		freeAttrPoints += 5;
		expToLVLUp = 20 * ((playerLevel * playerLevel) + playerLevel + 3);
		// Increase max health and return player back to full health
		maxHealth = (playerLevel * 3) + vitality;
		currentHealth = maxHealth;
	}

	public int getMaxHealth(){
		return maxHealth;
	}

	public int getHealth(){
		return currentHealth;
	}

	public int getDamage(){
		return damage;
	}

	public int getEXP(){
		return currentEXP;
	}

	public int getVitality(){
		return vitality;
	}

	public int getStrength(){
		return strength;
	}

	public int getDexterity(){
		return dexterity;
	}

	public int getDefense(){
		return defense;
	}

	public int getLuck(){
		return luck;
	}

	public int getFreePoints(){
		return freeAttrPoints;
	}
}
