using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGroup : MonoBehaviour {

	public Enemy[] enemy_data;

	// Use this for initialization
	public void init () {
		enemy_data = new Enemy[transform.childCount];
		foreach (Transform child in transform) {
			Enemy enemy = child.gameObject.GetComponent<Enemy>();
			enemy_data[enemy.id] = enemy;
		}
	}

	public PlayerStruct get_enemy_data(int id){
		//PlayerStruct enemy = new PlayerStruct ();
		Enemy enemy = enemy_data [id];

		PlayerStruct data = new PlayerStruct ();
		data.hp = enemy.hp;
		data.max_hp = enemy.max_hp;
		data.mp = enemy.mp;
		data.max_mp = enemy.max_mp;
		data.add_act = enemy.add_act;
		data.max_act = enemy.max_act;
		data.atk = enemy.atk;
		data.def = enemy.def;
		data.cure = enemy.cure;
		data.matk = enemy.matk;
		data.mdef = enemy.mdef;
		data.head_pic = enemy.head_pic;

		data.skill_group = new int[enemy.skill_group.Length];
		for (int i=0; i<enemy.skill_group.Length; i++) {
			data.skill_group[i] = enemy.skill_group[i];
		}

		return data;
	}

	public Item[] get_drop_item(int id){
		Enemy enemy = enemy_data [id];
		List<Item> dropItem = new List<Item> ();

		foreach (DropItem drop in enemy.drops) {
			Item item = drop.get_item();
			if( item != null )
				dropItem.Add(item);
		}

		return dropItem.ToArray ();
	}
}
