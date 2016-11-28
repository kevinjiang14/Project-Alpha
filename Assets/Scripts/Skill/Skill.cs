using UnityEngine;
using System.Collections;

public interface Skill {

	string GetSkillName();

	// Cast skill
	void Cast (Vector3 position, int direction);

	// Level up skill
	void LevelUp ();

	int getSkillLevel();

	void setSkillLevel(int level);
}
