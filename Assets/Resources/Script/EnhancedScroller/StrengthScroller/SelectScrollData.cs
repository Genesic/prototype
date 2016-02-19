using UnityEngine;
using System.Collections;

public class SelectScrollData {
	public string equipName;
	public ItemType equip_type;
	public int equip_seq;

	public SelectScrollData(string name, ItemType type, int seq){
		equip_seq = seq;
		equip_type = type;
		equipName = name;
	}
}
