using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum SkillPos{
	WEAPON = 0,
	ARMOR = 1,
	HANDGUARD = 2,
	SHOES = 3,
	SELF = 4,
	MAX_SELF = 5,
}

public class CharacterStatus : MonoBehaviour {

	public DataCenter dataCenter;
	public int character_id;

	public Image head_pic;
	public Text character_name;
	public Text lv;
	public Text exp;
	public Text hp;
	public Text max_hp;
	public Text mp;
	public Text max_mp;
	public Text atk;
	public Text def;
	public Text spd;
	public Text matk;
	public Text mdef;
	public Text[] skill;
	public Dictionary<SkillPos, Text> SkillText;

	public Text[] equip_text;

	public EquipScrollerController scrollerController;
	public GameObject selectEquipPanel;
	public Image[] equip_button;
	public GameObject selectPanel;
	public GameObject confirmPanel;

	private int select_cid = -1;
	private ItemType select_type = ItemType.NONE_TYPE;
	private int select_seq = -1;
	private Color select_color = Color.yellow;
    public Animator moveCharactor;

	public int get_select_seq(){
		return select_seq;
	}

	Color ori_color = new Color (0, 0, 0.392F, 1.0F);

	public int get_select_cid(){
		return select_cid;
	}

	public void init(int cid){
		dataCenter = MainData.dataCenter;		
		SkillText = new Dictionary<SkillPos, Text>();		
		foreach(SkillPos pos in System.Enum.GetValues(typeof(SkillPos))){
			SkillText[pos] = skill[(int)pos];			
		}
        
        change_select_character (cid);
	}

	public void change_select_character(int add){
		int idx = 0;
		if (dataCenter.status_idx + add >= dataCenter.character_team.Length) {
			idx = dataCenter.status_idx = 0;
		} else if (dataCenter.status_idx + add < 0) {
			idx = dataCenter.status_idx = dataCenter.character_team.Length - 1;
		} else {
			dataCenter.status_idx += add;
			idx = dataCenter.status_idx;
		}
		//Debug.Log ("add:"+add+" idx:" + idx + "len:"+dataCenter.character_team.Length);
		select_cid = dataCenter.character_team [idx];
		equip_cancel ();
		selectEquipPanel.SetActive (false);
		foreach (Image button in equip_button) {
			button.color = Color.white;
		}
	}
    
    public ItemType getEquipTypeBySkillPos(SkillPos skillPos){
        if( skillPos == SkillPos.WEAPON){
            return ItemType.WEAPON;
        } else if( skillPos == SkillPos.ARMOR ){
            return ItemType.ARMOR;
        } else if( skillPos == SkillPos.HANDGUARD ){
            return ItemType.HANDGUARD;
        } else if( skillPos == SkillPos.SHOES ){
            return ItemType.SHOES;
        }
        
        return ItemType.NONE_TYPE;
    }
	
	public void set_character_panel_status(){
		Character character = dataCenter.characters.character_data [select_cid];
		PlayerStruct info = dataCenter.characters.get_game_player (select_cid);

		head_pic.sprite = character.head_pic;
		character_name.text = character.character_name;
		lv.text = "Lv." + character.lv;
		exp.text = "Exp: " + character.exp + "/" + dataCenter.characters.get_next_lv_exp( character.lv_id, character.lv );
		hp.text = info.hp.ToString();
		max_hp.text = info.max_hp.ToString();
		max_hp.color = ori_color;
		mp.text = info.mp.ToString();
		max_mp.text = info.max_mp.ToString ();
		max_mp.color = ori_color;
		atk.text = info.atk.ToString();
		atk.color = ori_color;
		def.text = info.def.ToString();
		def.color = ori_color;
		spd.text = info.add_act.ToString ();
		spd.color = ori_color;
		matk.text = info.matk.ToString ();
		matk.color = ori_color;
		mdef.text = info.mdef.ToString ();
		mdef.color = ori_color;

		foreach(SkillPos pos in System.Enum.GetValues(typeof(SkillPos))){
			SkillText[pos].text = DataCenter.NONE_SKILL;
			SkillText[pos].color = ori_color;
			ItemType itemType = getEquipTypeBySkillPos(pos);
			if( itemType != ItemType.NONE_TYPE ){
				EquipAbility equip_info = dataCenter.get_character_equip (select_cid, itemType);
				if( equip_info != null ){
					equip_text[(int)itemType].text = equip_info.name + "Lv."+equip_info.lv;

					foreach(int sid in equip_info.skill_id){
						var skill_obj = dataCenter.get_skill_by_id(sid);
						skill[(int)itemType].text = skill_obj.skill_name;
					}
				}
				else
					equip_text[(int)itemType].text = DataCenter.NONE_EQUIP;					
			} else if (pos == SkillPos.SELF ) {
				var skill_obj = dataCenter.get_skill_by_id(character.self_skill);
				SkillText[pos].text = skill_obj.skill_name;
				SkillText[pos].color = ori_color;			
			}
		}				
	}

    public void openEquipPanel (ItemType equip_type ){
		set_character_panel_status ();
		for (ItemType i=ItemType.WEAPON; i<ItemType.EQUIP_NUM; i++) {
			if( i == equip_type ){
				equip_button [(int)i].color = select_color;
			} else {
				equip_button [(int)i].color = Color.white;
			}
		}
		confirmPanel.SetActive (false);
		selectPanel.SetActive (true);
		scrollerController.init (equip_type, character_id);        
    }

	public void select_equip_type (int equip_type ){
        openEquipPanel((ItemType)equip_type); 
	}

	public void equip_ok(){
		if (select_cid < 0)
			return;

		if (select_type < 0)
			return;

		//Debug.Log ("seq:"+select_seq+" cid:"+select_cid);
		dataCenter.set_character_equip (select_cid, select_type, select_seq);
		set_character_panel_status ();
		confirmPanel.SetActive (false);

		//var equip = dataCenter.get_character_equip (select_cid, select_type);
		//Debug.Log (equip);
	}

	public void equip_cancel(){
		select_type = ItemType.NONE_TYPE;
		select_seq = -1;

		//Debug.Log ("cancel");
		set_character_panel_status ();
		confirmPanel.SetActive (false);
	}

	public void show_equip_diff_value (ItemType equip_type, int seq){		
		// Debug.Log ("cid:" + cid + " type:" + equip_type + " seq:" + seq);
		PlayerStruct player = dataCenter.characters.get_game_player (select_cid);
		EquipAbility before = dataCenter.get_character_equip(select_cid, equip_type);
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
		
		int[] after_value;
		if (after != null) {
			after_value = new int[]{
				after.hp, after.mp, after.atk, after.def, after.matk, after.mdef
			};
		} else {
			after_value = new int[]{ 0, 0, 0, 0, 0, 0 };
		}
		
		Text[] ability_text = new Text[]{
			max_hp, max_mp, atk, def, matk, mdef
		};

		Color ori_color = new Color (0, 0, 0.392F, 1.0F);
		for (int i=0; i<before_value.Length; i++) {
			int ori = player_value[i];
			int before_num = before_value[i];
			int after_num = after_value[i];
			Text ability = ability_text[i];
			if( after_num == before_num ){
				ability.color = ori_color;
				ability.text =ori.ToString();
				continue;
			}
			if (before_num > after_num ) {
				ability.color = Color.red;
				ability.text = (ori + after_num - before_num)  + " (" + (after_num - before_num) + ")";
			} else {
				ability.color = Color.green;
				ability.text = (ori + after_num - before_num)  + " (+" + (after_num - before_num) + ")";
			}
		}

		show_skill_diff (equip_type, before, after);

		select_type = equip_type;
		select_seq = seq;
	}

	void skill_diff_text( ItemType equip_type, Color new_color, int sid ){
		skill [(int)equip_type].color = new_color;
		if (sid < 0) {
			skill [(int)equip_type].text = DataCenter.NONE_SKILL;
		} else {
			var skill_obj = dataCenter.get_skill_by_id (sid);
			skill [(int)equip_type].text = skill_obj.skill_name;
		}
	}

	void show_skill_diff(ItemType equip_type, EquipAbility before, EquipAbility after){
		int before_skill = -1;
		int after_skill = -1;

		if( before != null && before.skill_id.Length > 0 )
			before_skill = before.skill_id[0];

		if (after != null && after.skill_id.Length > 0)
			after_skill = after.skill_id [0];

		if (before_skill != after_skill) {
			if( after_skill >= 0 )
				skill_diff_text ( equip_type, Color.green, after_skill );
			else if( before_skill >= 0 )
				skill_diff_text ( equip_type, Color.red, after_skill );
		}
	}
}
