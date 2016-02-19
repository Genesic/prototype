using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmStrength : MonoBehaviour {

	public StrengthStatus strengthStatus;
	public DataCenter dataCenter;

	public Text equip_name;
	public Text ori_money;
	public Text diff_money;
	public GameObject new_money;
	public Text lv;
	public Text max_hp;
	public Text max_mp;
	public Text atk;
	public Text def;
	public Text spd;
	public Text matk;
	public Text mdef;
	public Text skill;
	public Image head_pic;
	public GameObject up_ok_button;
	public GameObject down_ok_button;
	public GameObject up_button;
	public GameObject down_button;

	public Color32 normal_color = new Color32 (50, 50, 50, 255);
	
	public void init(ItemType equip_type, int seq){
		dataCenter = MainData.dataCenter;
		var equip = dataCenter.get_equip_by_seq (equip_type, seq);
		check_buttom_show (equip);
		show_diff (equip, equip);

		int cid = dataCenter.equipList [equip_type] [seq].equiper;
		Debug.Log ("cid:" + cid);
		if (cid < 0) {
			head_pic.color = Color.clear;
		} else {
			head_pic.color = Color.white;
			head_pic.sprite = dataCenter.characters.character_data[cid].head_pic;
		}
	}

	void check_buttom_show(EquipAbility equip){
		if (equip.lv == 1) {
			up_button.SetActive (true);
			up_button.GetComponent<Image> ().color = Color.white;
			down_button.SetActive (false);
		} else if (equip.lv >= equip.max_lv) {
			up_button.SetActive (false);
			down_button.SetActive (true);
			down_button.GetComponent<Image> ().color = Color.white;
		} else {
			up_button.SetActive (true);
			down_button.SetActive (true);
			up_button.GetComponent<Image> ().color = Color.white;
			down_button.GetComponent<Image> ().color = Color.white;
		}
	}

	void show_diff(EquipAbility before, EquipAbility after){
		ori_money.text = dataCenter.money.ToString();
		equip_name.text = after.name + " (max_lv:"+after.max_lv+")";

		if (before.lv == after.lv) {
			diff_money.text = "";
			new_money.SetActive (false);
		} else {
			new_money.SetActive (true);
			Text new_money_text = new_money.GetComponent<Text> ();

			if( after.lv - before.lv > 0 ){ // up
				new_money_text.text = (dataCenter.money - before.next_lv_money).ToString();
				new_money_text.color = Color.red;
				diff_money.text = "(-"+before.next_lv_money+")";
				diff_money.color = Color.red;
			} else{	// down
				new_money_text.text = (dataCenter.money + before.back_lv_money).ToString();
				new_money_text.color = Color.green;
				diff_money.text = "(+"+before.back_lv_money+")";
				diff_money.color = Color.green;
			}
		}

		int[] before_value = new int[]{
			before.lv, before.hp, before.mp, before.atk, before.def, before.matk, before.mdef
		};

		int[] after_value = new int[]{
			after.lv, after.hp, after.mp, after.atk, after.def, after.matk, after.mdef
		};

		Text[] show = new Text[]{
			lv, max_hp, max_mp, atk, def, matk, mdef
		};

		for (int i=0; i<before_value.Length; i++) {
			int diff = after_value[i] - before_value[i];
			if( diff > 0 ){
				show[i].color = Color.green;
				show[i].text = after_value[i] + " (+" + diff + ")";
			} else if( diff == 0 ){
				show[i].color = normal_color;
				show[i].text = after_value[i].ToString();
			} else if( diff < 0 ){
				show[i].color = Color.red;
				show[i].text = after_value[i] + " (" + diff + ")";
			}
		}

		float after_spd = after.add_act;
		float diff_spd = after.add_act - before.add_act;
		if( diff_spd > 0 ){
			spd.color = Color.green;
			spd.text = after_spd + " (+" + diff_spd + ")";
		} else if( diff_spd == 0 ){
			spd.color = normal_color;
			spd.text = after_spd.ToString();
		} else if( diff_spd < 0 ){
			spd.color = Color.red;
			spd.text = after_spd + " (" + diff_spd + ")";
		}

		int before_skill = -1;
		int after_skill = -1;
		if( before.skill_id.Length > 0)
			before_skill = before.skill_id[0];

		if (after.skill_id.Length > 0)
			after_skill = after.skill_id [0];

		skill.text = DataCenter.NONE_SKILL;
		if (after_skill >= 0) {
			string skill_name = dataCenter.get_skill_by_id(after_skill).skill_name;
			skill.text = skill_name;
		}

		if (before_skill != after_skill) {
			if( after_skill >= 0 )
				skill.color = Color.green;
			else
				skill.color = Color.red;
		} else {
			skill.color = normal_color;
		}
	}

	void open_up_ok_button(EquipAbility equip){
		down_ok_button.SetActive (false);

		// not enough money
		if (dataCenter.check_money_patch (-equip.next_lv_money) == false){
			up_ok_button.SetActive (false);
			return;
		}

		// already max lv
		if (equip.lv >= equip.max_lv) {
			up_ok_button.SetActive (false);
			return;
		}

		up_ok_button.SetActive (true);
	}

	public void up_equip(){

		ItemType equip_type = strengthStatus.get_select_type ();
		int seq = strengthStatus.get_select_seq ();
		// now data
		var now_equip = dataCenter.get_equip_by_seq (equip_type, seq);

		// after up data
		var up_equip = dataCenter.equip [equip_type].get_equip_ability (now_equip.id, now_equip.lv + 1);

		// show value
		show_diff (now_equip, up_equip);

		up_button.GetComponent<Image> ().color = strengthStatus.select_color;
		down_button.GetComponent<Image> ().color = Color.white;
		open_up_ok_button (now_equip);
	}

	void open_down_ok_button(EquipAbility equip ){
		up_ok_button.SetActive (false);

		// already max lv
		if (equip.lv == 1 ) {
			down_ok_button.SetActive (false);
			return;
		}
		down_ok_button.SetActive (true);
	}

	public void down_equip(){
		ItemType equip_type = strengthStatus.get_select_type ();
		int seq = strengthStatus.get_select_seq ();

		// now data
		var now_equip = dataCenter.get_equip_by_seq (equip_type, seq);
		
		// after up data
		var up_equip = dataCenter.equip [equip_type].get_equip_ability (now_equip.id, now_equip.lv - 1);

		// show value
		show_diff (now_equip, up_equip);

		up_button.GetComponent<Image> ().color = Color.white;
		down_button.GetComponent<Image> ().color = strengthStatus.select_color;
		open_down_ok_button (now_equip);
	}

	public void up_ok(){
		ItemType equip_type = strengthStatus.get_select_type ();
		int seq = strengthStatus.get_select_seq ();

		var equip = dataCenter.get_equip_by_seq (equip_type, seq);
		dataCenter.patch_equip_lv (equip_type, seq, 1);
		dataCenter.patch_money (-equip.next_lv_money);
		strengthStatus.selectScrollerController.init (equip_type);

		var after_equip = dataCenter.get_equip_by_seq (equip_type, seq);
		if (after_equip.lv < after_equip.max_lv) {
			up_equip ();
			check_buttom_show (after_equip);
			up_button.GetComponent<Image> ().color = Color.yellow;
		} else {
			show_diff(after_equip, after_equip);
			check_buttom_show (after_equip);
			open_up_ok_button(after_equip);
		}
	}

	public void down_ok(){
		ItemType equip_type = strengthStatus.get_select_type ();
		int seq = strengthStatus.get_select_seq ();

		var equip = dataCenter.get_equip_by_seq (equip_type, seq);
		dataCenter.patch_equip_lv (equip_type, seq, -1);
		dataCenter.patch_money (equip.back_lv_money);
		strengthStatus.selectScrollerController.init (equip_type);

		var after_equip = dataCenter.get_equip_by_seq (equip_type, seq);
		if (after_equip.lv > 1) {
			down_equip();
			check_buttom_show (after_equip);
			down_button.GetComponent<Image> ().color = Color.yellow;
		} else {
			show_diff(after_equip, after_equip);
			check_buttom_show (after_equip);
			open_down_ok_button(after_equip);
		}
	}

}
