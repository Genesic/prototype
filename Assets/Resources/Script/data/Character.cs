using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability {
	public int max_hp;
	public int max_mp;	
	public int atk;
	public int def;
	public int matk;
	public int mdef;	
}

public class Character : MonoBehaviour {
	public string character_name;
	public int id;
	public int lv;
	public int hp;
	public int mp;
	public int mp2;
	public float spd;
	public int[] add_ability = new int[7];
	public int exp;
	public int lv_id;
	public int ability_id;
	public int self_skill;
	public int use_skill;
    public EquipType[] canEquipWeapon;
    public EquipType[] canEquipArmor;
	
	public Sprite head_pic;	

	void Awake(){
		//Debug.Log ("id:"+id+" add_ability:"+add_ability.Length);
		if (add_ability.Length == 0)
			add_ability = new int[7];
	}
}
