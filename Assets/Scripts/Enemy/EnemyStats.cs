using UnityEngine;
using System.Collections;

public class EnemyStats : MonoBehaviour {
	public float speed = 0.7f;
	public int enemyLevel = 1;
	public int defense = 5;
	public int strength = 10;
	public int vitality = 5;
	public int currentHealth{ get; set; }
	public float attackRange = 1f;
	public float attackSpeed = 1f;
	// Range when enemy starts moving towards player
	public float MaxRange = 4f;
	public float MinRange = 1f;

	public int enemyType;
}
