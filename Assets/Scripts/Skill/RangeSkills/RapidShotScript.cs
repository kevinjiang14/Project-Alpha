using UnityEngine;
using System.Collections;

public class RapidShotScript : MonoBehaviour, Skill {

	new public RuntimeAnimatorController animation;
	public string skillName;
	public float cooldown;

	private float timer;
	private bool learned;
	private int skillLevel;

	private bool skillCasted;
	private int arrowsFired;
	private int damage;
	private Vector3 pos;
	private int dir;

	private Player playerRef;

	// Use this for initialization
	void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		skillCasted = false;
		arrowsFired = 1;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (skillCasted && arrowsFired % 2 == 0) {
			arrowsFired += 1;
			damage = playerRef.getRangeDamage ();
			GameObject arrow = (GameObject) Instantiate (Resources.Load ("Item/Projectiles/Arrow"), UpdatePosition (pos, dir), GetRotation (dir));
			arrow.GetComponent<Projectile> ().DecreaseDamage (damage - (damage / 2));
		} else if (skillCasted && arrowsFired <= 6) {
			arrowsFired += 1;
		} else{
			arrowsFired = 1;
			skillCasted = false;
		}
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, int direction){
		if (timer >= cooldown && learned && playerRef.getAttackStyle() == 1) {
			pos = position;
			dir = direction;
			skillCasted = true;
			// Put skill on cooldown
			timer = 0;
		}
	}

	public Quaternion GetRotation(int direction){
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		if (direction == 0) {
			rotation = Quaternion.Euler(0, 0, 90);
		} else if (direction == 1) {
			rotation = Quaternion.Euler(0, 0, 0);
		} else if (direction == 2) {
			rotation = Quaternion.Euler(0, 0, -90);
		} else if (direction == 3) {
			rotation = Quaternion.Euler(0, 0, 180);
		}
		return rotation;
	}

	public Vector3 UpdatePosition(Vector3 originPos, int direction){
		Vector3 projectilePos = originPos;
		if (direction == 0) {
			projectilePos.y -= 0.6f;
		} else if (direction == 1) {
			projectilePos.x -= 0.6f;
		} else if (direction == 2) {
			projectilePos.y += 0.6f;
		} else if (direction == 3) {
			projectilePos.x += 0.6f;
		}
		return projectilePos;
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
