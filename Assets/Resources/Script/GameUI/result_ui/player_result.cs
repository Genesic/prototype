using UnityEngine;
using System.Collections;

public class player_result : MonoBehaviour {
	public bool start_result = false;

	public int[] player_cids;
	public int[] exp_get;
	public int[] exp_now;
	public int[] exp_max;
	public int[] lv_now;
	
	public RectTransform[] exp_bar;
	public RectTransform[] exp_max_bar;
	
	public BattleResultPlayer[] player_show;
	
	int[] exp_add;
	private battle_result battleResult;
	
	public void init(int[] cids, bool[] live, int patch_exp){
		battleResult = gameObject.GetComponentInParent<battle_result> ();

		player_cids = cids;
		player_show = new BattleResultPlayer[transform.childCount];
		exp_get = new int[live.Length];
		exp_max = new int[live.Length];
		exp_add = new int[live.Length];
		exp_now = new int[live.Length];
		lv_now = new int[live.Length];

		CharacterGroup characterGroup = MainData.dataCenter.characters;
		
		int idx = 0;
		foreach (Transform child in transform) {
			if( idx >= cids.Length ){
				child.gameObject.SetActive(false);
				continue;
			}
			
			int cid = cids[idx];
			if( cid >= 0 ){
				BattleResultPlayer player = child.gameObject.GetComponent<BattleResultPlayer>();
				
				var character = MainData.dataCenter.characters.character_data[cid];
				player.head_pic.sprite = character.head_pic;
				player.lv.text = "Lv. "+character.lv;
				
				int exp = character.exp;
				int max_exp = characterGroup.get_next_lv_exp(character.lv_id, character.lv);
				lv_now[idx] = character.lv;
				exp_now[idx] = exp;
				exp_max[idx] = max_exp;
				exp_get[idx] = ( live[idx] )? patch_exp : 0 ;
				player_show[idx] = player;
				
				// draw text and bar
				update_exp_num(idx, exp, max_exp);
				
				if( live[idx] )
					exp_add[idx] = ( patch_exp/30 < 1)? 1 : patch_exp/30;
			}
			idx++;
		}
	}

	public void start_show(){
		StartCoroutine (show_result ());
	}

	public IEnumerator show_result(){
		yield return new WaitForSeconds(0.6F);
		bool is_over = false;

		while (!is_over) {
			bool check = true;
			for (int idx = 0; idx < exp_get.Length; idx++) {
				if (exp_get [idx] > 0) {
					int patch = (exp_get [idx] > exp_add [idx]) ? exp_add [idx] : exp_get [idx];
					exp_get [idx] -= patch;
					exp_now [idx] += patch;
					if (exp_now [idx] >= exp_max [idx]) {
						int add = exp_now [idx] - exp_max [idx];
						int cid = player_cids [idx];
						lv_now [idx]++;
						var character = MainData.dataCenter.characters.character_data [cid];
						exp_max [idx] = MainData.dataCenter.characters.get_next_lv_exp (character.lv_id, lv_now [idx]);
						exp_now [idx] = add;
						player_show [idx].lv.text = "Lv. " + lv_now [idx];
					}
					update_exp_num (idx, exp_now [idx], exp_max [idx]);
					check = false;				
				}
			}
			if( check )
				is_over = true;

			yield return new WaitForSeconds(0.01F);
		}

		battleResult.set_state (battle_result.RESULT_STATE_PLAYER_END);
	}

	void update_exp_num (int idx, int exp, int max_exp) {
		player_show [idx].get_exp.text = exp_get [idx].ToString ();
		player_show[idx].now_exp.text = exp.ToString() + " / " + max_exp.ToString();
		
		float percent = (float)exp / (float)max_exp;
		float ori_width = player_show[idx].exp_bar_max.rectTransform.rect.width;
		float ori_height = player_show[idx].exp_bar_max.rectTransform.rect.height;
		player_show[idx].exp_bar.rectTransform.sizeDelta = new Vector2 (ori_width * percent, ori_height);
	}
}
