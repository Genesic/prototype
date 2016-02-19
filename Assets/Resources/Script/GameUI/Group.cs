using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Group : MonoBehaviour {

	public GameManager GameMgr;
	public bool is_monster;
	public Group enemy_group;
	public ActStoper act_stoper;
	public Player[] player_group;
	private int[] player_target;

	public enum line_bar { 
		HP_LINE = 1,
		MAX_HP_LINE,
		MP_LINE,
		MAX_MP_LINE,
		ACT_LINE,
		MAX_ACT_LINE,
	}

	public RectTransform[] hp_line;
	public RectTransform[] max_hp_line;
	public RectTransform[] mp_line;
	public RectTransform[] max_mp_line;
	public RectTransform[] act_line;
	public RectTransform[] max_act_line;
	public RectTransform[] head_pic;

	public void init(){        
        // init group
        if( is_monster ){
            player_group = new Player[transform.childCount];
            int idx = 0;
            foreach( Transform child in transform ){
                player_group[idx++] = child.gameObject.GetComponent<Player>();             
            }
        }
                
		// init player target
		player_target = new int[player_group.Length];
		for (int i = 0; i<player_target.Length; i++)
			player_target [i] = 0;

		
		// init line bar
		hp_line = new RectTransform[player_group.Length];
		max_hp_line = new RectTransform[player_group.Length];
		mp_line = new RectTransform[player_group.Length];
		max_mp_line = new RectTransform[player_group.Length];
		act_line = new RectTransform[player_group.Length];
		max_act_line = new RectTransform[player_group.Length];
		head_pic = new RectTransform[player_group.Length];
		
		for (int i = 0; i<player_group.Length; i++) {            
			Player player = player_group [i];            
			LineBar line_bar = player.gameObject.GetComponent<LineBar> ();
			hp_line [i] = line_bar.hp_line;
			max_hp_line [i] = line_bar.max_hp_line;
			mp_line [i] = line_bar.mp_line;
			max_mp_line [i] = line_bar.max_mp_line;
			act_line [i] = line_bar.act_line;
			max_act_line [i] = line_bar.max_act_line;
			head_pic [i] = line_bar.head_pic;
		}

	}
		
	public Player[] get_enemy_group(){
		return enemy_group.get_team_group ();
	}

	public Player[] get_team_group(){
		return player_group;
	}

	public Player get_target(Player player){
		for (int i=0; i<player_group.Length; i++) {
			if( player_group[i] == player ){
				int target_idx = player_target[i];
				Player[] enemy = get_enemy_group();
				if ( enemy[target_idx].is_alive() ){
					return enemy[target_idx];
				} else {
					for(int new_target_idx=0; new_target_idx<enemy.Length ; new_target_idx++ ){
						if( enemy[new_target_idx].is_alive() ){
							player_target[i] = new_target_idx;
							return enemy[new_target_idx];
						}
					}
				}
			}
		}

		return null;
	}

	public void set_target(Player player, int idx){
		for (int i=0; i<player_group.Length; i++) {
			if( player_group[i] == player ){
				Player[] enemy = get_enemy_group();
				if( enemy[idx] ){
					player_target[i] = idx;
					return;
				}
			}
		}		
	}

	public RectTransform get_head_pic(Player player){
		for (int loc=0; loc<player_group.Length; loc++) {
			if( player_group[loc] == player )
				return head_pic[loc];
		}

		return null;
	}

	RectTransform get_line_bar_by_type( line_bar type, int loc ){
		switch(type){
		case line_bar.MAX_HP_LINE:
			return max_hp_line[loc];
		case line_bar.HP_LINE:
			return hp_line[loc];
		case line_bar.MAX_MP_LINE:
			return max_mp_line[loc];
		case line_bar.MP_LINE:
			return mp_line[loc];
		case line_bar.MAX_ACT_LINE:
			return max_act_line[loc];
		case line_bar.ACT_LINE:
			return act_line[loc];
		default:
			return null;
		}
	}
	
	public void update_line_bar(line_bar type, line_bar max_type, Player player, float percent){
		RectTransform line = null;
		RectTransform max_line = null;
		for(int loc=0 ; loc<player_group.Length ; loc++ ){
			if( player_group[loc] == player ){
				line = get_line_bar_by_type(type, loc);
				max_line = get_line_bar_by_type(max_type, loc);
			}
		}

		if (line && max_line) {
			if (percent >= 1)
				percent = 1;
			float ori_width = max_line.rect.width;
			float ori_height = max_line.rect.height;
			line.sizeDelta = new Vector2 (ori_width * percent, ori_height);
		}
	}

	public void check_win(){
		bool is_game_over = true;
		foreach (Player player in player_group) {
			if( player.is_alive() )
				is_game_over = false;
		}

		if (is_game_over) {
			act_stoper.set_game_over(true);
			GameMgr.game_over_anime();
		}
	}

	public bool[] get_lives(){
		bool[] lives = new bool[player_group.Length];
		for (int i=0; i<lives.Length; i++) {
			lives[i] = player_group[i].is_alive();
		}

		return lives;
	}
}
