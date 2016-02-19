using UnityEngine;
using System.Collections;

public class Equip : ItemBase {	
	//public string item_name;
    public EquipType equipType;
	public int item_lv;
	public int hp;
	public int mp;
	public int mp2;
	public int atk;
	public int def;
	public int matk;
	public int mdef;
	public int strength_cost;
	public float add_act;
	public float max_act;
	public int[] lv_add;
	public int[] skill_id;

	public int[] need_item;
	public int[] need_num;

	public int get_strength_cost(int lv){
		return (item_lv * 10 + strength_cost )*lv;
	}

	public int get_max_lv(){
		return item_lv * 5;
	}
}
