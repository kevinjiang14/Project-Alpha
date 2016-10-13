﻿using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

	/* Base-Attributes */
	private float speed = 0.7f;
	private int enemyLevel = 1;
	private int defense = 5;
	private int strength = 10;
	private int vitality = 5;

	/* Non-Adjustable Attributes */
	// maxHealth = enemylevel * 3 + vitality
	private int maxHealth;
	private int currentHealth;
	// damage = strength / 5
	private int damage;
	// exp = 15x where x = enemylevel
	private int exp;

	// Starting position of enemy
	private int startX = 66;
	private int startY = 49;

	// Counting timers
	private float attackTimer;
	private float freezeTimer;

	// Range when enemy starts moving towards player
	private float MaxRange = 4f;
	private float MinRange = 1f;

	// Restrictions
	private float freezetime = 0.5f;
	private float attackRange = 1f;
	private float attackSpeed = 1f;

	// Do I really need to explain what this it?
    private GameObject player;
	private Player playerScript;

	private Animator enemyAnimation;
	private Transform HPTextBox;

    // Use this for initialization
    void Start () {
		enemyAnimation = GetComponent<Animator> ();
        player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.GetComponent<Player> ();
		HPTextBox = transform.GetChild (0);

		// Initialize non-adjustable attributes
		maxHealth = enemyLevel * 3 + vitality;
		currentHealth = maxHealth;
		damage = strength / 5;
		exp = enemyLevel * 15;
	}
	
	// Update is called once per frame
	void Update () {
		// Update HP textbox
		HPTextBox.GetComponent<TextMesh>().text = "" + currentHealth + "/" + maxHealth;

		// Timer for attacking
		attackTimer += Time.deltaTime;

		// Check if enemy can attack play when in range and is free to attack
		if (Vector3.Distance (transform.position, player.transform.position) <= attackRange && attackTimer >= attackSpeed) {
			AttackPlayer ();
		}
		
		// Check if player is in range to start moving towards
		if (Vector3.Distance (transform.position, player.transform.position) <= MaxRange && Vector3.Distance (transform.position, player.transform.position) >= MinRange) {
			MoveTowardsPlayer ();
			if (Vector3.Distance (transform.position, player.transform.position) <= MinRange + 1.0f) {
				GetComponent<Rigidbody2D> ().isKinematic = true;
			} else GetComponent<Rigidbody2D> ().isKinematic = false;
		} 
		// If not then return to starting position if displaced
		else if (transform.position.x != startX && transform.position.y != startY && Vector3.Distance (transform.position, player.transform.position) > MaxRange) {
			ResetEnemy ();
		} else
			Stay ();
	
	}


	// Stay still while resetting its restrictions
    private void Stay() {
		enemyAnimation.SetFloat ("Speed", 0.0f);
		freezeTimer += Time.deltaTime;
		if (freezeTimer >= freezetime) {
			freezeTimer = 0f;
		}
    }


	// Move towards player
    private void MoveTowardsPlayer() {
		float diffX = player.transform.position.x - transform.position.x;
		float diffY = player.transform.position.y - transform.position.y;

		if ((int)diffX > 0 && Vector3.Distance(transform.position, player.transform.position) >= MinRange) {
			enemyAnimation.SetInteger ("Direction", 3);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffX < 0 && Vector3.Distance(transform.position, player.transform.position) >= MinRange) {
			enemyAnimation.SetInteger ("Direction", 1);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffY < 0 && Vector3.Distance(transform.position, player.transform.position) >= MinRange) {
			enemyAnimation.SetInteger ("Direction", 0);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else if ((int)diffY > 0 && Vector3.Distance(transform.position, player.transform.position) >= MinRange) {
			enemyAnimation.SetInteger ("Direction", 2);
			enemyAnimation.SetFloat ("Speed", 1.0f);
		} else
			enemyAnimation.SetFloat ("Speed", 1.0f);


        Vector3 direction = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0f);
        transform.Translate(direction * speed * Time.deltaTime);
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
		if (currentHealth != maxHealth) {
			currentHealth += 1;
		}
	}

	// Enemy taking damage 
	public void TakeDamage(int i){
		// damage taken = incoming damage - defense / 10
		i = i - (defense / 10);
		if (currentHealth - i <= 0) {
			Dead ();
		} else
			currentHealth -= i;
	}

	// Enemy is dead
	public void Dead(){
		Destroy (gameObject);
		player.GetComponent<Player> ().GainEXP (exp);
	}

	public void SetSpawn(int x, int y){
		startX = x;
		startY = y;
	}
}
