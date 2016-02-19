using UnityEngine;
using System.Collections;

public class EquipAbility {
	public string name;
	public string show_name;
	public int id;
	public int lv;
	public int max_lv;
	public int hp;
	public int mp;
	public int mp2;
	public int atk;
	public int def;
	public int matk;
	public int mdef;
	public int cure;
	public float add_act;
	public float max_act;
	public int[] skill_id;

	public int next_lv_money;
	public int back_lv_money;
}

public class EquipGroup : MonoBehaviour {

	public Equip[] equip_data;
    public ItemType equipType;

	public const int HP = 0;
	public const int MP = 1;
	public const int ATK = 2;
	public const int DEF = 3;
	public const int MATK = 4;
	public const int MDEF = 5;
	public const int ADD_ACT = 6;
	public const int MAX_ACT = 7;

	// Use this for initialization
	/*
	void Awake () {
		equip_data = new Equip[transform.childCount];
		foreach (Transform child in transform) {
			Equip equip = child.gameObject.GetComponent<Equip>();
			equip_data[equip.id] = equip;
		}
	}*/
	public void init () {
		equip_data = new Equip[transform.childCount];
		foreach (Transform child in transform) {
			Equip equip = child.gameObject.GetComponent<Equip>();
            equip.setItemType(equipType);                   
			equip_data[equip.id] = equip;
		}
	}


	public EquipAbility get_equip_ability(int id, int lv){
		Equip equip = equip_data [id];

		EquipAbility data = new EquipAbility ();
		data.id = id;
		data.lv = lv;
		data.name = equip.itemName;
		data.show_name = equip.itemName + "Lv." + lv;
		data.hp = ( equip.hp + equip.lv_add[HP] * lv )*equip.item_lv;
		data.mp = equip.mp + equip.lv_add [MP] * lv;
		data.atk = equip.atk + equip.lv_add [ATK] * lv;
		data.def = equip.def + equip.lv_add [DEF] * lv;
		data.matk = equip.matk + equip.lv_add [MATK] * lv;
		data.mdef = equip.mdef + equip.lv_add [MDEF] * lv;
		data.add_act = equip.add_act + equip.lv_add[ADD_ACT];
		data.max_act = equip.max_act + equip.lv_add[MAX_ACT];
		data.max_lv = equip.get_max_lv ();
		data.next_lv_money = equip.get_strength_cost (lv);
		data.back_lv_money = (int)(equip.get_strength_cost (lv - 1) * 0.5);
		data.mp2 = equip.mp2;

		data.skill_id = new int[equip.skill_id.Length];
		for (int i=0; i<data.skill_id.Length; i++) {
			data.skill_id[i] = equip.skill_id[i];
		}

		return data;
	}
}
