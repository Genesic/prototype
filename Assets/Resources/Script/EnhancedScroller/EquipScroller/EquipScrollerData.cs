using UnityEngine;
using System.Collections;

public class EquipScrollerData {
	public string equipName;
	public int character_id;
	public ItemType equip_type;
	public int equip_id;

	public EquipScrollerData(string name, ItemType type, int id, int cid){
		equip_id = id;
		equip_type = type;
		equipName = name;
		character_id = cid;
	}
}
