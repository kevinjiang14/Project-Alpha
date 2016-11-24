using UnityEngine;
using System.Collections;

public interface Skill {

	string GetSkillName();

	void Cast (Vector3 position, int direction);

	void LevelUp ();

	int getSkillLevel();

	void setSkillLevel(int level);
}
