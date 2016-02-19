using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class item_result : MonoBehaviour {

	int get_money;
	int now_money;
	Item[] items;

	public Text money_text;
	public Text now_money_text;
	public Text[] items_text;

	private DataCenter dataCenter;
	private battle_result battleResult;
	public void init(int money, Item[] get_items){
		dataCenter = MainData.dataCenter;
		battleResult = gameObject.GetComponentInParent<battle_result> ();
		items = get_items;
		get_money = money;
		now_money = dataCenter.money;
		update_money (get_money, now_money);

		foreach (Text item in items_text) {
			item.text = "";
		}
	}

	public void start_show(){
		StartCoroutine (show_result ());
	}

	public IEnumerator show_result (){
		yield return new WaitForSeconds(0.5F);

		int show_money = get_money;
		int patch = show_money / 6;
		while (get_money > 0) {
			if( get_money - patch > 0 ){
				now_money += patch;
				get_money -= patch;
			} else {
				now_money += get_money;
				get_money = 0;
			}
			update_money(get_money, now_money);

			yield return new WaitForSeconds(0.05F);
		}

		yield return new WaitForSeconds(0.4f);
		int idx = 0;
		foreach (Item item in items) {
			string item_name = dataCenter.get_name_by_itemid(item.item_type, item.item_id, item.item_lv);
			items_text[idx].text = item_name + " x"+item.item_num;
			idx++;
			yield return new WaitForSeconds(0.2f);
		}

		yield return new WaitForSeconds(1f);
		battleResult.set_state (battle_result.RESULT_STATE_ITEM_END);
	}

	void update_money(int money, int now_money){
		money_text.text = money.ToString ();
		now_money_text.text = now_money.ToString ();
	}
}
