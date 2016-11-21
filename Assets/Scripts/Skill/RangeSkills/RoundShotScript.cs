using UnityEngine;
using System.Collections;

public class RoundShotScript : MonoBehaviour, Skill {

	public string skillName;
	public float cooldown;

	private float timer;
	private bool learned = true;
	private int skillLevel = 0;



	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, Quaternion rotation){
		if (timer >= cooldown && learned) {
			Vector3 arrowPos;
			rotation = Quaternion.Euler (0, 0, 0);
			arrowPos = new Vector3 (position.x - 0.6f, position.y, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 45);
			arrowPos = new Vector3 (position.x - 0.424f, position.y - 0.424f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 90);
			arrowPos = new Vector3 (position.x, position.y - 0.6f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 135);
			arrowPos = new Vector3 (position.x + 0.424f, position.y - 0.424f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 180);
			arrowPos = new Vector3 (position.x + 0.6f, position.y, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 225);
			arrowPos = new Vector3 (position.x + 0.424f, position.y + 0.424f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 270);
			arrowPos = new Vector3 (position.x, position.y + 0.6f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			rotation = Quaternion.Euler (0, 0, 315);
			arrowPos = new Vector3 (position.x - 0.424f, position.y + 0.424f, position.z);
			Instantiate(Resources.Load("Item/Arrow"), arrowPos, rotation);
			// Put skill on cooldown
			timer = 0;
		}
	}

	public void LearnSkill (){
		if (!learned) {
			learned = true;
		}
	}

	public void LevelUp(){
		if (skillLevel < 10){
			skillLevel++;
		}
	}

	public int getSkillLevel(){
		return skillLevel;
	}
}
