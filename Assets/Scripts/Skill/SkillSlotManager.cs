using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillSlotManager : MonoBehaviour {

	private GameObject skill;

	// Update is called once per frame
	void Update () {
		transform.Find ("SkillLevel").GetComponent<Text> ().text = skill.GetComponent<Skill> ().getSkillLevel ().ToString();
	}

	public void AddSkill(GameObject skill){
		this.skill = skill;
		AddSkillIconFunction ();
		AddButtonFunction ();
		transform.Find ("SkillName").GetComponent<Text> ().text = skill.GetComponent<Skill>().GetSkillName();
	}

	public void AddSkillIconFunction(){
		Transform icon = transform.Find ("SkillIcon");
		icon.GetComponent<Image> ().sprite = skill.GetComponent<SpriteRenderer> ().sprite;
		icon.GetComponent<Button> ().onClick.AddListener (AddToHotbar);
	}

	public void AddButtonFunction(){
		transform.Find ("SkillLevelButton").GetComponent<Button> ().onClick.AddListener (skill.GetComponent<Skill>().LevelUp);
	}

	public void AddToHotbar(){
		GameObject currentHotbar = GameObject.FindGameObjectWithTag("HotbarMenu");
		currentHotbar.GetComponent<HotbarManager>().AddtoHotbar(skill);
	}

}
