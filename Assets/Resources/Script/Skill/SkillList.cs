using UnityEngine;
using System.Collections;

public class SkillList : MonoBehaviour {
	public Skill[] skill_list;
	public AnimeList anime_list;

	public void init(){
		skill_list = new Skill[transform.childCount];
		foreach (Transform child in transform) {
			var skill = child.gameObject.GetComponent<Skill>();
			skill_list[skill.id] = skill;
		}
	}

	public Skill get_skill_by_id(int skill_id){
		return skill_list[skill_id];
	}	
}
