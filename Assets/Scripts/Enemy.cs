using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class EnemyStats : Component{
	public float speed = 0.7f;
	public int enemyLevel = 1;
	public int defense = 5;
	public int strength = 10;
	public int vitality = 5;
	public int currentHealth;
	public float attackRange = 1f;
	public float attackSpeed = 1f;
	// Range when enemy starts moving towards player
	public float MaxRange = 4f;
	public float MinRange = 1f;

	public int enemyType = -1;
}

public class Skeleton : EnemyStats{

    public Skeleton(){
        defense = 7;

		enemyType = 0;
    }
}

public class Slime : EnemyStats{

    public Slime(){
        enemyLevel = 2;
        vitality = 7;

		enemyType = 1;
    }
}

public class Bat : EnemyStats {

    public Bat(){
        strength = 12;

		enemyType = 2;
    }
}

public class SkeletonBoss : EnemyStats {

	public SkeletonBoss(){
		enemyLevel = 10;
		defense = 30;
		strength = 5;
		vitality = 50;

		MaxRange = 10f;

		enemyType = 100;
	}
}

public class Enemy: MonoBehaviour {

	/* Enemy Stats */
	private EnemyStats enemyStats;

	private int maxHealth;	// maxHealth = enemylevel * 3 + vitality
	private int damage;		// damage = strength / 5
	private int exp;		// exp = 15x where x = enemylevel

	// Counting timers
	private float attackTimer;

	// Position of enemy
	private int startX = 66;
	private int startY = 49;

	// Enemy difficulty multiplier based on floor number
	private int multiplier;

	// Do I really need to explain what this it?
    private GameObject player;
	private Player playerScript;

	private GameObject map;
	private MapManager mapScript;

	// Miscellaneous 
	private Animator enemyAnimation;
	private Transform HPTextBox;

	private bool collision = false;

    // Use this for initialization
    void Start () {

		// Animator componenet
		enemyAnimation = GetComponent<Animator> ();

		// Find Player
        player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player> ();

		// Find MapManager
		map = GameObject.FindGameObjectWithTag ("MapManager");
		mapScript = map.GetComponent<MapManager> ();

		HPTextBox = transform.GetChild (0);
		multiplier = mapScript.getFloor ();

		AdjustStats ();

		// Initialize non-adjustable attributes
		maxHealth = enemyStats.enemyLevel * 3 + enemyStats.vitality;
		enemyStats.currentHealth = maxHealth;
		damage = enemyStats.strength / 5;
		exp = enemyStats.enemyLevel * 15;
	}
	
	// Update is called once per frame
	void Update () {
		// Update HP textbox
		HPTextBox.GetComponent<TextMesh>().text = "" + enemyStats.currentHealth + "/" + maxHealth;

		// Timer for attacking
		attackTimer += Time.deltaTime;

		// Check if player is in range to start moving towards
		if (Vector3.Distance (transform.position, player.transform.position) <= enemyStats.MaxRange && collision == false) {
			MoveTowardsPlayer ();
		} 
		// If not then return to starting position if displaced
		else if (transform.position.x != startX && transform.position.y != startY && Vector3.Distance (transform.position, player.transform.position) > enemyStats.MaxRange) {
			ResetEnemy ();
		}

        // Keep enemy awake so collision events continue to be called
        if (gameObject.GetComponent<Rigidbody2D>().IsSleeping()) {
            gameObject.GetComponent<Rigidbody2D>().WakeUp();
        }
	}
		
	void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.tag == "Player") {
			collision = true;
		}
    }

	void OnCollisionStay2D(Collision2D coll) {
        // If collision is with player then attack, if enemy then don't move
        if (coll.gameObject.tag == "Player" && attackTimer > enemyStats.attackSpeed) {
			GetComponent<Rigidbody2D> ().isKinematic = true;
			AttackPlayer ();
			Stay ();
		}
	}

	void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Hitbox") {
            collision = false;
        }
    }

	// Stay still while resetting its restrictions
    private void Stay() {
		enemyAnimation.SetFloat ("Speed", 0.0f);
    }


	// Move towards player
    private void MoveTowardsPlayer() {
		GetComponent<Rigidbody2D> ().isKinematic = false;

		float diffX = player.transform.position.x - transform.position.x;
		float diffY = player.transform.position.y - transform.position.y;

		if ((int)diffX > 0 && Vector3.Distance(transform.position, player.transform.position) >= enemyStats.MinRange) {
			enemyAnimation.SetInteger ("Direction", 3);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffX < 0 && Vector3.Distance(transform.position, player.transform.position) >= enemyStats.MinRange) {
			enemyAnimation.SetInteger ("Direction", 1);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffY < 0 && Vector3.Distance(transform.position, player.transform.position) >= enemyStats.MinRange) {
			enemyAnimation.SetInteger ("Direction", 0);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffY > 0 && Vector3.Distance(transform.position, player.transform.position) >= enemyStats.MinRange) {
			enemyAnimation.SetInteger ("Direction", 2);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else
			enemyAnimation.SetFloat ("Speed", 1.0f);

        Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0f);
		transform.Translate(direction * enemyStats.speed * Time.deltaTime);
    }

	// Attack player
	private void AttackPlayer(){
		playerScript.TakeDamage (damage);
		attackTimer = 0f;
	}

	// Return to starting position
	private void ResetEnemy(){
		// Turn on kinematic so enemy doesnt get stuck behind a wall when returning to original position
		GetComponent<Rigidbody2D> ().isKinematic = true;
		
		Vector3 direction = new Vector3 (startX - transform.position.x, startY - transform.position.y, 0);
		transform.Translate (direction * Time.deltaTime);

		// Regenerate health while not in aggro'd
		if (enemyStats.currentHealth != maxHealth) {
			enemyStats.currentHealth += 1;
		}
	}

	// Enemy taking damage 
	public void TakeDamage(int i){
		// damage taken = incoming damage - defense / 7
		i = i - (enemyStats.defense / 7);
        // Set minimum amount of damage the player can do to enemy as 1
        if(i <= 0) {
            i = 1;
        }

		if (enemyStats.currentHealth - i <= 0) {
			Dead ();
		} else
			enemyStats.currentHealth -= i;
	}

	// Enemy is dead
	public void Dead(){
		if (enemyStats.enemyType % 100 == 0 && enemyStats.enemyType <= 100) {
			// Get the position of the enemy if it was a boss and spawn the ladder there
			Vector3 deathPosition = transform.position;
			GameObject ladder = (GameObject)Instantiate (Resources.Load ("LevelObjects/Ladder"), deathPosition, Quaternion.identity) as GameObject;
			ladder.transform.SetParent (GameObject.Find ("Boss Room").transform);
		}

		Destroy (gameObject);
		player.GetComponent<Player> ().GainEXP (exp);


	}

	public void SetSpawn(int x, int y){
		startX = x;
		startY = y;
	}

	public void AdjustStats(){
		if (multiplier > 1) {
			enemyStats.enemyLevel = enemyStats.enemyLevel + multiplier;
			enemyStats.strength = enemyStats.strength * multiplier;
			enemyStats.defense = enemyStats.defense * multiplier;
			enemyStats.vitality = enemyStats.vitality * multiplier;
		}
	}

	public void EnemyType(int i){
		if(i == 100){
			enemyStats = new SkeletonBoss ();
		} else if(i == 0){
            enemyStats = new Skeleton();
        } else if(i == 1){
            enemyStats = new Slime();
        } else if(i == 2){
            enemyStats = new Bat();
        }
        AdjustStats();
    }
}
