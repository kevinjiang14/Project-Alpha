using UnityEngine;
using System.Collections;

public class SharpenScript : MonoBehaviour, Skill {

	new public RuntimeAnimatorController animation;
	public string skillName;
	public float cooldown;
	public float buffDuration;
	// Array of bonus given based on level of skill
	public int[] bonus;
	// Index of the bonus stat given 
	private int indexBonus;

	// Timer for skill cooldown
	private float timer;
	// Timer for buff's active time
	private float buffTimer;
	private bool learned;
	private int skillLevel;

	// Player reference
	private Player playerRef;
	private BuffManager buffBar;

	private bool skillcasted;

	// Use this for initialization
	void Start(){
		buffBar = GameObject.FindGameObjectWithTag ("BuffBar").GetComponent<BuffManager> ();
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		skillcasted = false;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (skillcasted) {
			buffTimer += Time.deltaTime;
		}
		if (buffTimer >= buffDuration && skillcasted) {
			EndBuff ();
		}
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, int direction){
		if (timer >= cooldown && learned) {
			BuffSetup ();
			skillcasted = true;
			buffTimer = 0;
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

	public void BuffSetup(){
		// Ensure player only get the bonus stat once
		if (!skillcasted) {
			// Add buff to UI buffbar
			buffBar.AddBuff (this.gameObject);
			// Give player bonus stat
			playerRef.IncreaseStrengthTemp (bonus[skillLevel - 1]);
			indexBonus = skillLevel;
		}
	}

	public void EndBuff(){
		// Let us know the skill is not in effect anymore so player can receive bonus stats again
		skillcasted = false;
		// Remove buff from UI buffbar
		buffBar.RemoveBuff (this.gameObject);
		// Remove bonus stat from player
		playerRef.DecreaseStrengthTemp (bonus[indexBonus - 1]);
	}
}
