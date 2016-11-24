using UnityEngine;
using System.Collections;

public class VolleyScript : MonoBehaviour, Skill {
	
	public RuntimeAnimatorController animation;
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
			// Original rotation
			Quaternion rotation = GetRotation (direction);
			// Rotation of additional arrows from original rotation
			Quaternion newRotation = rotation;
			Vector3 newPosition = UpdatePosition (position, direction);
			Instantiate(Resources.Load("Item/Arrow"), newPosition, newRotation);
			newRotation = Quaternion.Euler (0, 0, rotation.eulerAngles.z + 30);
			Instantiate(Resources.Load("Item/Arrow"), newPosition, newRotation);
			newRotation = Quaternion.Euler (0, 0, rotation.eulerAngles.z + 15);
			Instantiate(Resources.Load("Item/Arrow"), newPosition, newRotation);
			newRotation = Quaternion.Euler (0, 0, rotation.eulerAngles.z - 30);
			Instantiate(Resources.Load("Item/Arrow"), newPosition, newRotation);
			newRotation = Quaternion.Euler (0, 0, rotation.eulerAngles.z - 15);
			Instantiate(Resources.Load("Item/Arrow"), newPosition, newRotation);
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
