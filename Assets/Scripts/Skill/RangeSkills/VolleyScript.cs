using UnityEngine;
using System.Collections;

public class VolleyScript : MonoBehaviour, Skill {

	public string skillName;
	public float cooldown;

	private float timer;
	private bool learned = true;
	private int skillLevel = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
	}

	public string GetSkillName(){
		return skillName;
	}

	public void Cast(Vector3 position, Quaternion rotation){
		if (timer >= cooldown && learned) {
			Quaternion newRotation = GetRotation ();
			Vector3 newPosition = UpdatePosition (position);
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

	public Quaternion GetRotation(){
		Animator playerAnimation = GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<Animator> ();
		Quaternion rotation = Quaternion.Euler(0, 0, 0);
		if (playerAnimation.GetInteger ("Direction") == 0) {
			rotation = Quaternion.Euler(0, 0, 90);
		} else if (playerAnimation.GetInteger ("Direction") == 1) {
			rotation = Quaternion.Euler(0, 0, 0);
		} else if (playerAnimation.GetInteger ("Direction") == 2) {
			rotation = Quaternion.Euler(0, 0, -90);
		} else if (playerAnimation.GetInteger ("Direction") == 3) {
			rotation = Quaternion.Euler(0, 0, 180);
		}
		return rotation;
	}

	public Vector3 UpdatePosition(Vector3 originPos){
		Animator playerAnimation = GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<Animator> ();
		Vector3 projectilePos = originPos;
		if (playerAnimation.GetInteger ("Direction") == 0) {
			projectilePos.y -= 0.6f;
		} else if (playerAnimation.GetInteger ("Direction") == 1) {
			projectilePos.x -= 0.6f;
		} else if (playerAnimation.GetInteger ("Direction") == 2) {
			projectilePos.y += 0.6f;
		} else if (playerAnimation.GetInteger ("Direction") == 3) {
			projectilePos.x += 0.6f;
		}
		return projectilePos;
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
