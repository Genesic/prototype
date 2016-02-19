using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public int id;

	public int hp;
	public int max_hp;
	public int mp;
	public int max_mp;
	public float add_act;
	public float max_act;
	
	public int atk;
	public int def;
	public int cure;
	public int matk;
	public int mdef;
	
	public int[] skill_group;
	public Sprite head_pic;

	public int exp;
	public int money;
	public DropItem[] drops;

	void Start(){
		drops = new DropItem[transform.childCount];
		int i = 0;
		foreach (Transform child in transform) {
			DropItem dropItem = child.gameObject.GetComponent<DropItem>();
			drops[i++] = dropItem;
		}
	}
}
