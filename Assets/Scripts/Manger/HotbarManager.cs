using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour {

    // Hotbar attributes
    private int size = 6;
	private GameObject[] instantiatedSkills;
	private GameObject[] skills;

    // Player's iventory
	private GameObject player;

    void Awake(){
		instantiatedSkills = new GameObject[size];
		skills = new GameObject[size];
		transform.Find ("HotItem0").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(0);});
		transform.Find ("HotItem1").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(1);});
		transform.Find ("HotItem2").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(2);});
		transform.Find ("HotItem3").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(3);});
		transform.Find ("HotItem4").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(4);});
		transform.Find ("HotItem5").GetComponent<Button> ().onClick.AddListener (delegate {RemoveSkill(5);});
	}

    void Update(){
		player = GameObject.FindGameObjectWithTag ("Player").gameObject;

        if (Input.GetButtonDown("Hotslot1")){
			if (skills [0] != null) {
				instantiatedSkills[0].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
			}
        }
		if (Input.GetButtonDown("Hotslot2")){
			if (skills [1] != null) {
				instantiatedSkills[1].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
			}
        }
        if (Input.GetButtonDown("Hotslot3")){
//			Instantiate (Resources.Load ("NPC/NPC"), new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -0.1f), Quaternion.identity);
            if (skills[2] != null){
				instantiatedSkills[2].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
            }
        }
        if (Input.GetButtonDown("Hotslot4")){
            if (skills[3] != null){
				instantiatedSkills[3].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
            }
        }
        if (Input.GetButtonDown("Hotslot5")){
            if (skills[4] != null){
				instantiatedSkills[4].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
            }
        }
        if (Input.GetButtonDown("Hotslot6")){
            if (skills[5] != null){
				instantiatedSkills[5].GetComponent<Skill>().Cast(player.transform.position, player.GetComponent<Animator>().GetInteger("Direction"));
            }
        }
    }

    // Add skill to hotbar if there is room
    public void AddtoHotbar(GameObject skill){
        Transform temphotitem;
        bool attached = false;
        // Check if item already exist on the hotbar
		foreach(GameObject skillFromList in skills){
			if(skillFromList != null && skillFromList == skill){
                attached = true;
                break;
            }
        }
		// If skill isn't on hotbar yet then find the earliest empty spot for the skill to attach to
		if (!attached) {
			for(int i = 0; i < size; i++) {
				if (skills[i] == null) {
					skills [i] = skill;
					instantiatedSkills [i] = Instantiate (skill);
					// Set instantiated gameobject to correct skill level
					instantiatedSkills [i].GetComponent<Skill> ().setSkillLevel (skill.GetComponent<Skill> ().getSkillLevel ());
					temphotitem = transform.Find (string.Format ("HotItem{0}", i));
					temphotitem.GetChild (0).GetComponent<Image> ().sprite = skill.GetComponent<SpriteRenderer> ().sprite;
					break;
				}
			}
		}
    }

	// Remove Skill
	public void RemoveSkill(int i){
		// Destroy the instantiated skill
		Destroy (instantiatedSkills [i]);
		// Remove skill from list so another can take its place
		skills [i] = null;
		// Remove Sprite
		Transform temphotitem;
		temphotitem = transform.Find(string.Format(string.Format("HotItem{0}", i)));
		temphotitem.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("TransparentSprite");
	}
}
