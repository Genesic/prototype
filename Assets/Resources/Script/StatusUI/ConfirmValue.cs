using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConfirmValue : MonoBehaviour {

	public DataCenter dataCenter;
	public CharacterStatus characterStatus;

	public Text hp;
	public Text mp;
	public Text atk;
	public Text def;
	public Text spd;
	public Text matk;
	public Text mdef;

	public void show_equip_diff_value (int cid, ItemType equip_type, int seq){
		PlayerStruct player = dataCenter.characters.get_game_player (cid);
		EquipAbility before = dataCenter.get_character_equip(cid, equip_type);
		EquipAbility after = dataCenter.get_equip_by_seq(equip_type, seq);

		int[] player_value = new int[] {
			player.max_hp, player.max_mp, player.atk, player.def, player.matk, player.mdef
		};

		int[] before_value;
		if (before != null) {
			before_value = new int[]{
				before.hp, before.mp, before.atk, before.def, before.matk, before.mdef
			};
		} else {
			before_value = new int[]{ 0, 0, 0, 0, 0, 0 };
		}

		int[] after_value = new int[]{
			after.hp, after.mp, after.atk, after.def, after.matk, after.mdef
		};

		Text[] ability_text = new Text[]{
			hp, mp, atk, def, matk, mdef
		};

		// int value
		for (int i=0; i<before_value.Length; i++) {
			int ori = player_value[i];
			int before_num = before_value[i];
			int after_num = after_value[i];
			if( after_num == before_num )
				continue;

			Text ability = ability_text[i];
			ability.text = (ori + after_num - before_num)  + " (" + (after_num - before_num) + ")";
			if (before_num > after_num ) {
				ability.color = Color.red;
			} else {
				ability.color = Color.green;
			}
		}

		// float value
		float ori_spd = player.add_act;
		float before_spd = before.add_act;
		float after_spd = after.add_act;
		if (after_spd != before_spd) {
			spd.text = (ori_spd + after_spd - before_spd) + " ("+ (after_spd - before_spd) + ")";
			if( before_spd > after_spd ){
				spd.color = Color.red;
			} else {
				spd.color = Color.green;
			}
		}
	}
}
