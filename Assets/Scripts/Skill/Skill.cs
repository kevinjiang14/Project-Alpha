using UnityEngine;
using System.Collections;

public interface Skill {

	string GetSkillName();

	void Cast (Vector3 position, Quaternion rotation);

	void LearnSkill ();

	void LevelUp ();

	int getSkillLevel();
}
