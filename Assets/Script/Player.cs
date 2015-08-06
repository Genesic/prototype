using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[SerializeField]
	protected int id;
	[SerializeField]
	protected int hp;
	[SerializeField]
	protected int max_hp;
	[SerializeField]
	protected int mp;
	[SerializeField]
	protected int max_mp;
	[SerializeField]
	protected float act;
	[SerializeField]
	protected float max_act;

	[SerializeField]
	protected int atk;
	[SerializeField]
	protected int def;
	[SerializeField]
	protected int matk;
	[SerializeField]
	protected int mdef;

	protected Player player;

	public int skill_idx;
	public Skill[] skill_group;

	public Group group;
	public Player[] target;

	public RectTransform hp_line;
	public RectTransform max_hp_line;
	public RectTransform mp_line;
	public RectTransform max_mp_line;
	public RectTransform act_line;
	public RectTransform max_act_line;
	
	void Start (){
		player = gameObject.GetComponent<Player> ();
	}

	private void update_act(){
		float percent = act / max_act;
		if (percent >= 1)
			player.set_act (0);

		if (max_act_line != null) {
			float ori_width = max_act_line.rect.width;
			float ori_height = max_act_line.rect.height;
			if (percent >= 1) {
				act_line.sizeDelta = new Vector2 (ori_width, ori_height);
			} else {
				act_line.sizeDelta = new Vector2 (ori_width * percent, ori_height);
			}
		}
	}

	private void update_hp(){
		float percent = (float)hp / (float)max_hp;
		if (max_hp_line != null) {
			if (percent > 1)
				percent = 1;
			float ori_width = max_hp_line.rect.width;
			float ori_height = max_hp_line.rect.height;
			hp_line.sizeDelta = new Vector2 (ori_width * percent, ori_height);
		}
	}

	private void update_mp(){
		float percent = (float)mp / (float)max_mp;
		if (max_mp_line != null) {
			if (percent > 1)
				percent = 1;
			float ori_width = max_mp_line.rect.width;
			float ori_height = max_mp_line.rect.height;
			mp_line.sizeDelta = new Vector2 (ori_width * percent, ori_height);
		}
	}

	void FixedUpdate () {
		player.patch_act (Time.deltaTime);
		update_act ();
		update_hp ();
		update_mp ();
	}
	
	public int get_atk(){
		return atk;
	}

	public int get_def(){
		return def;
	}

	public int get_matk(){
		return matk;
	}

	public int get_mdef(){
		return mdef;
	}

	public int get_max_hp(){
		return max_hp;
	}

	public int get_hp(){ 
		return hp; 
	}

	public int patch_hp(int patch){
		hp += patch;
		if ( hp > max_hp)
			hp = max_hp;

		if (hp < 0)
			hp = 0;

		return hp;
	}

	public int get_max_mp(){
		return max_mp;
	}

	public int get_mp(){
		return mp;
	}

	public int patch_mp( int patch){
		mp += patch;
		if (mp > max_mp)
			mp = max_mp;

		if (mp < 0 )
			mp = 0;

		return mp;
	}

	public float get_max_act(){
		return max_act;
	}
	
	public float get_act(){
		return act;
	}

	public void set_act(float set){
		act = set;
	}
	
	public float patch_act( float patch){
		act += patch;
		if (act > max_act)
			act = max_act;
		
		if (act < 0 )
			act = 0;

		return act;
	}

	public void set_target( int idx ){
		Player[] enemy_group = group.get_enemy_group (player);
		target = new Player[enemy_group.Length];
		for (int i=0; i<enemy_group.Length; i++) {
			if( i == 0 )
				target[i] = enemy_group[idx];
			else if( i == idx )
				target[i] = enemy_group[0];
			else
				target[i] = enemy_group[i];
		}
	}

	public void start_act(){
		Skill use_skill = skill_group [skill_idx];
		use_skill.use_skill (player, target);
	}
}
