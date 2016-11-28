using UnityEngine;
using System.Collections;

public class CrushScript : MonoBehaviour, Skill {

	new public RuntimeAnimatorController animation;
	public string skillName;
	public float cooldown;

	private float timer;
	private bool learned;
	private int skillLevel;

	private Player playerRef;

	private bool skillcasted = false;

	private int damageIncrease;
	private int delayTimer;

	// Use this for initialization
	void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		delayTimer = 0;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (skillcasted && delayTimer == 1) {
			playerRef.DeactivateHitBox ();
			playerRef.UpdateStats ();
			skillcasted = false;
			delayTimer = 0;
		} else if (skillcasted && delayTimer == 0) {
			delayTimer += 1;
		}
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, int direction){
		if (timer >= cooldown && learned && playerRef.getAttackStyle() == 0) {
			damageIncrease = playerRef.getMeleeDamage () / 4;
			if (damageIncrease == 0) {
				damageIncrease = 1;
			}
			playerRef.IncreaseMeleeDamage (damageIncrease);
			ActivateHitBox (direction);
			skillcasted = true;
			timer = 0;
		}
	}

	public void ActivateHitBox(int direction){
		Transform player = GameObject.FindGameObjectWithTag ("Player").transform;
		if (direction == 0) {
			player.Find ("South").gameObject.SetActive (true);
		} else if (direction == 1) {
			player.Find ("West").gameObject.SetActive (true);
		} else if (direction == 2) {
			player.Find ("North").gameObject.SetActive (true);
		} else if (direction == 3) {
			player.Find ("East").gameObject.SetActive (true);
		}
	}

	public void LevelUp(){
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		if (playerRef.getSkillPoints() > 0) {
			if (!learned) {
				learned = true;
				skillLevel++;
				playerRef.DecreaseSkillPoint ();
			} else if (skillLevel < 10) {
				skillLevel++;
				playerRef.DecreaseSkillPoint ();
			}
		}
	}

	public int getSkillLevel(){
		return skillLevel;
	}

	public void setSkillLevel(int level){
		if (level > 0) {
			learned = true;
		}
		skillLevel = level;
	}
}
