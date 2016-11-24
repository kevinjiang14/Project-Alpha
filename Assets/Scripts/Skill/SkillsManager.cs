using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour {

	private SkillHolder playerSkills;
	private Player playerRef;

	// Skill slots
	public GameObject skillSlot;

	// Use this for initialization
	void OnEnable () {
		playerSkills = GameObject.FindGameObjectWithTag ("Player").GetComponent<SkillHolder> ();
		playerRef = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		AddRangeSkills ();
		AddButtonListeners ();
	}

	void Update(){
		transform.Find ("SkillPoints").GetComponent<Text> ().text = " Skill Points: " + playerRef.getSkillPoints ();
	}

	public void AddMeleeSkills(){
		ClearSkillList ();

		GameObject[] meleeSkills = playerSkills.GetMeleeSkillsList();
		foreach (GameObject skill in meleeSkills) {
			GameObject slot = AddSlot();
			SkillSlotManager ssManager = slot.GetComponent<SkillSlotManager> ();
			ssManager.AddSkill (skill);
		}
	}

	public void AddRangeSkills(){
		ClearSkillList ();

		GameObject[] rangeSkills = playerSkills.GetRangeSkillsList();
		foreach (GameObject skill in rangeSkills) {
			GameObject slot = AddSlot();
			SkillSlotManager ssManager = slot.GetComponent<SkillSlotManager> ();
			ssManager.AddSkill (skill);
		}
	}

	// 
	public void AddMagicSkills(){
		ClearSkillList ();

		GameObject[] magicSkills = playerSkills.GetMagicSkillsList();
		foreach (GameObject skill in magicSkills) {
			GameObject slot = AddSlot();
			SkillSlotManager ssManager = slot.GetComponent<SkillSlotManager> ();
			ssManager.AddSkill (skill);
		}
	}

	// Add skill slots to menu
	public GameObject AddSlot(){
		GameObject slot = Instantiate (skillSlot);
		slot.transform.SetParent (transform.Find ("SkillSlots").Find ("Layout"));
		return slot;
	}

	// Clear the current skill list being displayed
	public void ClearSkillList(){
		Transform layout = transform.Find ("SkillSlots").Find ("Layout");

		if (transform.childCount > 1) {
			Transform[] slots = layout.gameObject.GetComponentsInChildren<Transform> ();
			foreach (Transform slot in slots) {
				if (slot != slots [0]) {
					Destroy (slot.gameObject);
				}
			}
		}
	}

	// Add the on click listeners to switch skill set
	public void AddButtonListeners(){
		transform.Find ("MeleeSkills").GetComponent<Button> ().onClick.AddListener (AddMeleeSkills);
		transform.Find ("RangeSkills").GetComponent<Button> ().onClick.AddListener (AddRangeSkills);
		transform.Find ("MagicSkills").GetComponent<Button> ().onClick.AddListener (AddMagicSkills);
	}
}
