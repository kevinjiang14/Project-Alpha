using UnityEngine;
using System.Collections;

public class RoundShotScript : MonoBehaviour, Skill {

	new public RuntimeAnimatorController animation;
	public string skillName;
	public float cooldown;

	private float timer;
	private bool learned;
	private int skillLevel;

	private Player playerRef;

	// Use this for initialization
	void Start(){
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, int direction){
		if (timer >= cooldown && learned && playerRef.getAttackStyle() == 1) {
			Vector3 arrowPos;
			Quaternion rotation = Quaternion.Euler (0, 0, 0);
			arrowPos = new Vector3 (position.x - 0.6f, position.y, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 45);
			arrowPos = new Vector3 (position.x - 0.424f, position.y - 0.424f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 90);
			arrowPos = new Vector3 (position.x, position.y - 0.6f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 135);
			arrowPos = new Vector3 (position.x + 0.424f, position.y - 0.424f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 180);
			arrowPos = new Vector3 (position.x + 0.6f, position.y, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 225);
			arrowPos = new Vector3 (position.x + 0.424f, position.y + 0.424f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 270);
			arrowPos = new Vector3 (position.x, position.y + 0.6f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 315);
			arrowPos = new Vector3 (position.x - 0.424f, position.y + 0.424f, position.z);
			Instantiate(Resources.Load("Item/Projectiles/Arrow"), arrowPos, rotation);
			// Put skill on cooldown
			timer = 0;
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
