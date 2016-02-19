using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterGroup : MonoBehaviour {

	public DataCenter dataCenter;
	public Character[] character_data;

	public const int LV_MAX = 20;

	// Use this for initialization
	public void init () {
		dataCenter = MainData.dataCenter;
		character_data = new Character[transform.childCount];
		foreach (Transform child in transform) {
			Character character = child.gameObject.GetComponent<Character>();
			character_data[character.id] = character;
		}
	}

	public void patch_exp(int cid, int exp){
		Character character = character_data [cid];

		while (exp > 0 && character.lv < LV_MAX) {
			int max_exp = get_next_lv_exp (character.lv_id, character.lv);
			if (character.exp + exp >= max_exp) {
				int add = character.exp + exp - max_exp;
				character.lv += 1;
				character.exp = add;
				exp -= (max_exp - character.exp);
			} else {
				character.exp += exp;
				exp = 0;
			}
		}
	}

	public void update_battle_data(int cid, int hp, int mp){
		MainData.dataCenter.characters.character_data [cid].hp = hp;
		MainData.dataCenter.characters.character_data [cid].mp = mp;
	}
		
	public PlayerStruct get_game_player(int cid){
		Character character = character_data [cid];
		PlayerStruct player = new PlayerStruct ();
		player.id = cid;		

		// character
		Ability character_ability = get_base_ability (character.ability_id, character.lv);
		player.ch_name = character.character_name;
		player.head_pic = character.head_pic;
		player.hp = character.hp;		
		// Debug.Log ("len:" + character.add_ability.Length + "idx:"+EquipGroup.HP);
		player.max_hp = character_ability.max_hp + character.add_ability[EquipGroup.HP];
		player.max_mp = character_ability.max_mp + character.add_ability[EquipGroup.MP];
		player.atk = character_ability.atk + character.add_ability[EquipGroup.ATK];
		player.def = character_ability.def + character.add_ability[EquipGroup.DEF];
		player.matk = character_ability.matk + character.add_ability[EquipGroup.MATK];
		player.mdef = character_ability.mdef + character.add_ability[EquipGroup.MDEF];
		player.mp2 = character.mp2;
		player.add_act = character.spd;
		player.max_act = 5;
		
        var skills = new List<int>();
        skills.Add(character.self_skill);
		for (ItemType i=ItemType.WEAPON; i < ItemType.EQUIP_NUM; i++) {
			//EquipAbility equip = null;

			EquipAbility equip = dataCenter.get_character_equip(character.id, i );
			if( equip != null){
				player.max_hp += equip.hp;
				player.max_mp += equip.mp;
				player.mp2 += equip.mp2;
				player.atk += equip.atk;
				player.def += equip.def;
				player.matk += equip.matk;
				player.mdef += equip.mdef;
				player.add_act += equip.add_act;
				player.max_act += equip.max_act;								
                for(int j=0 ; j<equip.skill_id.Length ; j++){
                    skills.Add(equip.skill_id[j]);
                }
			}
		}
        
        // mp每次戰鬥時都是全滿的
        player.mp = player.max_mp;
        
		player.skill_group = new int[skills.Count];
        for(int idx = 0 ; idx < skills.Count ; idx++){
            player.skill_group[idx] = skills[idx];
        }
        
		return player;
	}

	public Ability get_base_ability(int ability_id, int lv){
		Ability data = new Ability ();
		switch (ability_id) {
		case 1: // atk type
			data.max_hp = 130 + lv * 11;
			data.max_mp = 120 + lv * 5;
			data.atk = 32 + lv * 4;
			data.def = 18 + lv;
			data.matk = 25 + lv;
			data.mdef = 13 + lv / 2;			 
			break;
		case 2: // def type
			data.max_hp = 180 + lv * 20;
			data.max_mp = 100 + lv * 5;
			data.atk = 15 + lv;
			data.def = 35 + lv * 3;
			data.matk = 15 + lv / 2;
			data.mdef = 32 + lv * 2;
			break;
		case 3: // matk type
			data.max_hp = 90 + lv * 12;
			data.max_mp = 180 + lv * 13;
			data.atk = 18 + lv;
			data.def = 23 + lv;
			data.matk = 40 + lv*3;
			data.mdef = 15 + lv*2;
			break;
		default: // average type
			data.max_hp = 130 + lv * 15;
			data.max_mp = 120 + lv * 8;
			data.atk = 25 + lv*2;
			data.def = 23 + lv*2;
			data.matk = 19 + lv;
			data.mdef = 25 + lv;
			break;
		}

		return data;
	}

	public int get_next_lv_exp(int lv_id, int lv ){
		int [] exp = new int[LV_MAX];
		switch (lv_id) {
		case 1: //
			exp = new int[]{0, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210 };
			return exp[lv];
		case 2:
			exp = new int[]{0, 25, 32, 38, 45, 55, 70, 75, 80, 90, 110, 130, 150, 170, 175, 180, 185, 190, 193, 195, 200 };
			return exp[lv];
		default:
			exp = new int[]{0, 15, 23, 32, 40, 48, 57, 62, 73, 80, 89, 100, 110, 125, 140, 160, 180, 185, 190, 195, 200 };
			return exp[lv];
		}
	}
}
