using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlotManager : MonoBehaviour {

	private GameObject skill;

	// Update is called once per frame
	void Update () {
		transform.Find ("SkillLevel").GetComponent<Text> ().text = skill.GetComponent<Skill> ().getSkillLevel ().ToString();
	}

	// Add skill to skill menu
	public void AddSkill(GameObject skill){
		this.skill = skill;
		AddSkillIconFunction ();
		AddButtonFunction ();
		transform.Find ("SkillName").GetComponent<Text> ().text = skill.GetComponent<Skill>().GetSkillName();
	}

	// Add the button function for the skill icon
	public void AddSkillIconFunction(){
		Transform icon = transform.Find ("SkillIcon");
		icon.GetComponent<Image> ().sprite = skill.GetComponent<SpriteRenderer> ().sprite;
		icon.GetComponent<Button> ().onClick.AddListener (AddToHotbar);
	}

	// Add button function to increase skill level
	public void AddButtonFunction(){
		transform.Find ("SkillLevelButton").GetComponent<Button> ().onClick.AddListener (skill.GetComponent<Skill>().LevelUp);
	}

	// Add Skill to hotbar
	public void AddToHotbar(){
		if (skill.GetComponent<Skill> ().getSkillLevel () > 0) {
			GameObject currentHotbar = GameObject.FindGameObjectWithTag ("HotbarMenu");
			currentHotbar.GetComponent<HotbarManager> ().AddtoHotbar (skill);
		}
	}

}
