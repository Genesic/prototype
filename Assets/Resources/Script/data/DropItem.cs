using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
	public ItemType item_type;
	public int item_id;
	public int item_max_lv;
	public int item_min_lv;
	public int item_max_num;
	public int item_min_num;

	public int prob;

	public Item get_item(){
		int check = Random.Range(0, 100);
		if (check > prob)
			return null;

		int item_num = 1;
		if (item_type >= ItemType.EQUIP_NUM)
			item_num = Random.Range (item_min_num, item_max_num);

		int item_lv = Random.Range (item_min_lv, item_max_lv);
		Item item = new Item (item_type, item_id, item_num,item_lv );

		return item;
	}
}
