using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public NumberManager NumberMgr { get; private set; }
	public SkillAnimeManager SkillAnimeMgr { get; private set; }

	public Transform canvas;

	public Animator[] enemy_anime;
	public ActStoper act_stoper;

	public DataCenter data_center;
	public int[] cids;
	public int drop_exp;
	public int drop_money;
	public List<Item> drop_item = new List<Item>();
	private Group playerGroup;

	// public GameObject game_start_anime_obj;
	void Start(){
		data_center = MainData.dataCenter;
		// Debug.Log ("BattleStart equiper:"+data_center.equip_list[0][3].equiper );
		BattleStart ();
	}

	public void BattleStart(){

		// ActStoper
		var path = string.Format ("Prefab/BattleUI/ActStoper");
		var ActStoper = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		act_stoper = ActStoper.GetComponent<ActStoper> ();

		// BackGround
		path = string.Format ("Prefab/BattleUI/Background");
		var Background = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		Background.transform.SetParent(canvas, false);
		
		// Background setting
		//RectTransform BackgroundRT = Background.GetComponent<RectTransform> ();
		//BackgroundRT.localPosition = new Vector3 (4, 194, 0);
		//BackgroundRT.localScale = new Vector3 (1, 1, 1);
		
		// Skill
		var Skill = data_center.skills.gameObject;
		SkillList skill_list = data_center.skills;
		foreach (Transform child in Skill.transform) {
			child.gameObject.GetComponent<Skill>().gameMgr = this;
		}
		
		// Skillline
		path = string.Format ("Prefab/BattleUI/SkillLine");
		var SkillLine = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		SkillLine.transform.SetParent(canvas, false);
		
		// SkillLine setting
		RectTransform SkillLineRT = SkillLine.GetComponent<RectTransform> ();
		//SkillLineRT.localPosition = new Vector3 (400, -410, 0);
		SkillLineRT.localScale = new Vector3 (0.6667F, 0.6667F, 0.6667F);
		
		// PlayerGroup
		path = string.Format ("Prefab/BattleUI/PlayGroup");
		var PlayerGroup = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		PlayerGroup.transform.SetParent (canvas);
		
		// PlayerGroup setting
		PlayerGroup.transform.localPosition = new Vector3 (0, 0, 0);
		PlayerGroup.transform.localScale = new Vector3 (0.6667F, 0.6667F, 0.6667F);
		Group group = PlayerGroup.gameObject.GetComponent<Group> ();
		playerGroup = group;
		group.act_stoper = act_stoper;
		group.GameMgr = this;
		
		// ControlPlayer setting
		ControlPlayer control = PlayerGroup.gameObject.GetComponent<ControlPlayer> ();
		control.skill_list = skill_list;
		int i = 0;
		foreach (Transform skill_transform in SkillLine.transform)
			control.skill_button [i++] = skill_transform.gameObject;
		
		int k = 0;
		foreach (Transform child in SkillLine.transform) {
			int tmp = k;
			Button skill_button = child.gameObject.GetComponent<Button>();
			skill_button.onClick.AddListener( () => { control.SetSkill(tmp); });
			k++;
		}
		
		//init PlayerGroup
		int idx = 0;
		cids = new int[data_center.character_team.Length];
		group.player_group = new Player[data_center.character_team.Length];
		foreach(Transform child in PlayerGroup.transform){
			if( idx >= data_center.character_team.Length ){
				child.gameObject.SetActive(false);
			} else {
				Player toPlayer = child.gameObject.AddComponent<Player>();
				int cid = data_center.character_team[idx];
				PlayerStruct battler = data_center.characters.get_game_player(cid);
				toPlayer.info = battler;
				cids[idx] = cid;
		
				child.FindChild("Head_pic").GetComponent<Image>().sprite = toPlayer.info.head_pic;
				child.FindChild("Head").GetComponent<Image>().sprite = toPlayer.info.head_pic;
				group.player_group [idx] = toPlayer;
				idx++;
			}
		}
		group.init ();
		foreach (Player player in group.player_group) {
			player.skill_list = skill_list;
			player.init ();
		}
		
		// EnemyGroup
		string group_id = MainData.dataCenter.enemy_group_name;
		path = string.Format ("Prefab/BattleUI/MonsterGroup/{0}",group_id);
		var EnemyGroup = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		EnemyGroup.transform.SetParent (canvas);
		
		// 初始化敵人群組
		EnemyGroup.transform.localPosition = new Vector3 (198, 171, 0);
		EnemyGroup.transform.localScale = new Vector3 (0.6667F, 0.6667F, 0.6667F);
		Group enemy_group = EnemyGroup.gameObject.GetComponent<Group> ();
		enemy_group.act_stoper = act_stoper;
		group.enemy_group = enemy_group;
		enemy_group.enemy_group = group;
		enemy_group.GameMgr = this;
		
		enemy_group.init ();

		// 初始化敵人群組裡的敵人	
		k = 0;
		int[] enemy_ids = MainData.dataCenter.enemy_ids;
		foreach (Player enemy in enemy_group.player_group) {
			int enemy_id = enemy_ids[k];
			enemy.info = data_center.enemys.get_enemy_data(enemy_id); // slime
			enemy.skill_list = skill_list;
            enemy.gameObject.GetComponent<Image>().sprite = enemy.info.head_pic;
            enemy.transform.FindChild("Head").GetComponent<Image>().sprite = enemy.info.head_pic;
			enemy.init ();
			int tmp = k;
			Button enemy_button = enemy.gameObject.GetComponent<Button>();
			enemy_button.onClick.AddListener( () => { control.SetTarget(tmp); });
			k++;
		}

		// 計算掉落物
		calc_drop (enemy_ids);
		
		control.init ();
		control.SetControl (0);

		// 取得每個敵人的顯示動畫
		enemy_anime = new Animator[enemy_group.player_group.Length];
		int anime_idx = 0;
		foreach (Transform child in EnemyGroup.transform) {
			Animator anime = child.gameObject.GetComponentInChildren<Animator>();
			enemy_anime[anime_idx++] = anime;
		}

		// SkillAnimeManager
		path = string.Format ("Prefab/skill/SkillAnimeManager");
		var SkillAnimeManager = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		SkillAnimeManager.transform.SetParent(canvas);
		SkillAnimeMgr = SkillAnimeManager.GetComponent<SkillAnimeManager>();

		// Number Manager
		path = string.Format ("Prefab/numbers/NumberManager");
		var NumberManager = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		NumberManager.transform.SetParent(canvas);
		NumberMgr = NumberManager.GetComponent<NumberManager>();

		// game_start
		path = string.Format ("Prefab/BattleUI/anime_prefab/game_start");
		var game_start = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		game_start.transform.SetParent(canvas, false);
		game_start_anime = game_start.GetComponent<Animator> ();

		// game_over_win
		path = string.Format ("Prefab/BattleUI/anime_prefab/game_over_win");
		var game_over_win = Instantiate ( Resources.Load<GameObject> (path) ) as GameObject;
		game_over_win.transform.SetParent(canvas, false);
		game_over_win_anime = game_over_win.GetComponent<Animator> ();
		game_over_win.SetActive (false);

		// result anime
		path = string.Format ("Prefab/BattleUI/battle_result");
		var battleResultObj = Instantiate (Resources.Load<GameObject> (path)) as GameObject;
		battleResult = battleResultObj.GetComponent<battle_result> ();
		battleResult.transform.SetParent (canvas, false);
		battleResultObj.SetActive (false);

		// 播放開始動畫
		game_start_anime_show ();
	}

	public Animator game_over_win_anime;
	public battle_result battleResult;
	int actHash = Animator.StringToHash("act");
//	int overHash = Animator.StringToHash("over");
	int showHash = Animator.StringToHash("show");
	public void game_over_anime(){
		game_over_win_anime.gameObject.SetActive (true);

		StartCoroutine (game_over_anime_counter ());
	}

	IEnumerator game_over_anime_counter(){
		yield return new WaitForSeconds(1f);
		game_over_win_anime.SetTrigger (actHash);

		yield return new WaitForSeconds(3f);
		bool[] lives = playerGroup.get_lives ();

		// update hp mp
		foreach (Player player in playerGroup.player_group) {
			MainData.dataCenter.characters.update_battle_data( player.info.id, player.info.hp, player.info.mp );
		}

		battleResult.gameObject.SetActive (true);
		battleResult.init (cids, lives, drop_exp, drop_money, drop_item.ToArray ());

		// yield return new WaitForSeconds(5f);
		//game_over_win_anime.SetTrigger (overHash);
	}
/*
	IEnumerator game_over_close(){
		yield return new WaitForSeconds(3f);
		game_over_win_anime.SetTrigger (overHash);
	}
*/
	public Animator game_start_anime;
	int startHash = Animator.StringToHash("start");
	int warningHash = Animator.StringToHash("warning");
	public void game_start_anime_show(){
		StartCoroutine (game_start_show ());
	}

	IEnumerator game_start_show(){
		yield return new WaitForSeconds(1.0f);

		// 淡入動畫
		game_start_anime.SetTrigger (startHash);
		yield return new WaitForSeconds(1.0f);

		// warning動畫
		game_start_anime.SetTrigger (warningHash);
		yield return new WaitForSeconds(3f);

		for (int i=0; i<enemy_anime.Length; i++) {
			yield return new WaitForSeconds(0.4f);
			enemy_anime[i].SetTrigger(showHash);
		}

		yield return new WaitForSeconds(1f);
		act_stoper.set_stoper (false);
	}

	void calc_drop(int[] enemy_ids){
		foreach (int enemy_id in enemy_ids) {
			drop_exp += data_center.enemys.enemy_data[enemy_id].exp;
			drop_money += data_center.enemys.enemy_data[enemy_id].money;
			Item[] items = data_center.enemys.get_drop_item(enemy_id);
			foreach(Item item in items ){
				if( item.item_type >= ItemType.EQUIP_NUM ){
					// 一般道具統計總數
					bool already = false;
					foreach(Item drop in drop_item){
						if( item.item_type == drop.item_type && item.item_id == drop.item_id ){
							drop.item_num += item.item_num;
							already = true;
						}
					}
					if( !already )
						drop_item.Add(item);
				} else {
					// 裝備直接給予
					drop_item.Add (item);
				}
			}
		}
	}
}
