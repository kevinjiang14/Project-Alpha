using UnityEngine;
using System.Collections;

public class SkillHolder : MonoBehaviour {

	public GameObject[] meleeSkills;
	public GameObject[] rangeSkills;
	public GameObject[] magicSkills;

	void Start(){
		foreach (GameObject skill in meleeSkills) {
			skill.GetComponent<Skill> ().setSkillLevel (0);
		}
		foreach (GameObject skill in rangeSkills) {
			skill.GetComponent<Skill> ().setSkillLevel (0);
		}
		foreach (GameObject skill in magicSkills) {
			skill.GetComponent<Skill> ().setSkillLevel (0);
		}
	}

	public GameObject[] GetMeleeSkillsList(){
		return meleeSkills;
	}

	public GameObject[] GetRangeSkillsList(){
		return rangeSkills;
	}
	
	public GameObject[] GetMagicSkillsList(){
		return magicSkills;
	}

	public GameObject[] GetAllSkills(){
		int size = meleeSkills.Length + rangeSkills.Length + magicSkills.Length;
		GameObject[] allSkills = new GameObject[size];

		int index = 0;
		foreach (GameObject meleeskill in meleeSkills) {
			allSkills [index] = meleeskill;
		}
		foreach (GameObject rangeskill in rangeSkills) {
			allSkills [index] = rangeskill;
		}
		foreach (GameObject magicskill in magicSkills) {
			allSkills [index] = magicskill;
		}
		return allSkills;
	}
}
