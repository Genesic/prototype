using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class battle_result : MonoBehaviour {

	public const int RESULT_STATE_START = 0;
	public const int RESULT_STATE_PLAYER_START = 1;
	public const int RESULT_STATE_PLAYER_END = 2;
	public const int RESULT_STATE_ITEM_START = 3;
	public const int RESULT_STATE_ITEM_END = 4;

	public int state;
	public GameObject playerResultObj;
	public GameObject itemResultObj;

	private player_result playerResult;
	private item_result itemResult;

	public int startHash = Animator.StringToHash("start");
	public int overHash = Animator.StringToHash("over");
	// Use this for initialization
	void Start () {
		/*
		int[] cids = new int[2] {0, 1};
		bool[] live = new bool[2] { true, true };
		int get_money = 150;
		Item[] items = new Item[4];
		items [0] = new Item (DataCenter.HANDGUARD, 0, 1, 5);
		items [1] = new Item (DataCenter.MATERIAL, 0, 3, 0);
		items [2] = new Item (DataCenter.MATERIAL, 1, 5, 0);
		items [3] = new Item (DataCenter.MATERIAL, 2, 4, 0);
		init (cids, live, 40, get_money, items);
		*/
	}

	public void init(int[] cids, bool[] live, int total_exp, int money, Item[] items ){
		state = RESULT_STATE_START;
		playerResult = playerResultObj.GetComponent<player_result> ();
		itemResult = itemResultObj.GetComponent<item_result> ();
		int num = 0;
		foreach (bool is_live in live) {
			if( is_live )
				num++;
		}
		int patch_exp = total_exp / num;
		playerResult.init (cids, live, patch_exp);
		itemResult.init (money, items);

		MainData.dataCenter.patch_money (money);
		MainData.dataCenter.patch_items (items);
		for (int i=0; i<cids.Length; i++) {
			if( cids[i] >= 0 && live[i] )
				MainData.dataCenter.characters.patch_exp(cids[i], patch_exp );
		}
		set_state (RESULT_STATE_PLAYER_START);
	}

	public void set_state(int new_state){
		state = new_state;

		switch (new_state) {
		case RESULT_STATE_PLAYER_START:
			Animator playerAnime = playerResultObj.GetComponent<Animator> ();
			playerAnime.SetTrigger (startHash);
			break;

		case RESULT_STATE_ITEM_START:
			Animator itemAnime = itemResultObj.GetComponent<Animator> ();
			itemAnime.SetTrigger(startHash);
			itemResult.start_show();
			break;

		case RESULT_STATE_ITEM_END:
			MainData.ins.status_scene();
			break;
		}
	}
}
