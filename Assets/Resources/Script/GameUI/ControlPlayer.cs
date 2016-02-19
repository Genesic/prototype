using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlPlayer : MonoBehaviour {
	
	public Group group;
	public GameObject[] skill_button;
	public int control;
	public SkillList skill_list;

	// private float ori_width;
	
	public void init(){
		// ori_width = group.player_group [0].gameObject.GetComponent<RectTransform> ().rect.width;
		reset_skill_line ();
	}

	public void reset_skill_line(){
		int[] skill_group = group.player_group [control].info.skill_group;
		for (int i=0; i<skill_group.Length; i++) {
			GameObject skill = skill_button[i];
			skill.SetActive(true);
			var colors = skill.GetComponent<Button>().colors;
			if( i == group.player_group[control].skill_idx ){
				colors.normalColor = new Color32(255, 255, 0, 255);
				colors.highlightedColor = new Color32(255, 255, 100, 255);
			} else {
				colors.normalColor = Color.white;
				colors.highlightedColor = Color.white;
			}
			skill.GetComponent<Button>().colors = colors;
			int skill_id = skill_group[i];
			Skill skill_obj = skill_list.get_skill_by_id(skill_id);
			skill.GetComponentInChildren<Text>().text = skill_obj.skill_name;
		}

		for (int i=skill_group.Length; i<skill_button.Length; i++)
			skill_button [i].SetActive (false);
	}

	public void SetControl(int new_control){
		control = new_control;
		reset_skill_line ();
		Player controller = group.player_group[control];

		// change controller size
		/*
		foreach (Player player in group.player_group) {
			RectTransform playerRT = player.gameObject.GetComponent<RectTransform>();
			if( player == controller ){
				playerRT.sizeDelta = new Vector2( playerRT.rect.width +20, playerRT.rect.height );
			} else {
				playerRT.sizeDelta = new Vector2( ori_width, playerRT.rect.height );
			}
		}
		*/

		// change controller color
		//Player controller = group.player_group[control];
		foreach (Player player in group.player_group) {
			var colors = player.gameObject.GetComponent<Button>().colors;
			if( player == controller ){
				colors.normalColor = new Color32(255, 255, 0, 255);
				colors.highlightedColor = new Color32(255, 255, 100, 255);
			} else {
				colors.normalColor = Color.white;
				colors.highlightedColor = Color.white;
			}
			player.gameObject.GetComponent<Button>().colors = colors;
		}
	}
	
	public void SetTarget(int index){
		Player controller = group.player_group[control];
		controller.set_target (index);
	}

	public void SetSkill(int index){
		Player controller = group.player_group[control];
		controller.set_use_skill (index);		
		reset_skill_line ();
	}
}
