using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public struct PlayerStruct {
	public int id;
	public string ch_name;
	public int hp;
	public int max_hp;
	public int mp;
	public int max_mp;
	public int mp2;
	public float add_act;
	public float max_act;
	
	public int atk;
	public int def;
	public int cure;
	public int matk;
	public int mdef;

	public int[] skill_group;
	public Sprite head_pic;
}


public class Player : MonoBehaviour {

	public PlayerStruct info;
	public List<BattleBuff> buffData = new List<BattleBuff>();
	//public int hp;
	//public int max_hp;
	//public int mp;
	//public int max_mp;
	public float act;
	//public float add_act = 0;
	//public float max_act;
	
	//public int atk;
	//public int def;
	//public int cure;
	//public int matk;
	//public int mdef;

	public string attribute;

	[SerializeField]
	protected bool alive;
	public Group group;
	public bool is_monster { get; private set;}

	public int skill_idx;
	//public int[] skill_group;
	public SkillList skill_list;
	
	public Sprite head_pic = null;

	public Vector2 original_head { get; private set; }

	[SerializeField]
	private int ori_hp;
	[SerializeField]
	private int ori_mp;		 

	public void init (){
		alive = true;
		group = gameObject.GetComponentInParent<Group> ();
		original_head = group.get_head_pic (this).position;

		is_monster = (group.is_monster) ? true : false;
		ori_hp = info.hp;
		ori_mp = info.mp;
		head_pic = info.head_pic;
		
		//初始化使用技能
		int use = MainData.dataCenter.characters.character_data[info.id].use_skill;
		set_use_skill(use);
		
		//初始化buff狀態		

		if( !is_monster )
			gameObject.GetComponent<LineBar>().ch_name.text = info.ch_name;
	}
	
	public bool set_alive_status(){
		if (info.hp > 0)
			alive = true;
		else {
			alive = false;
			if( group.is_monster ){
				StartCoroutine(monster_dead ());
//				gameObject.SetActive(false);
//				group.check_win();
			}
		}

		return alive;
	}

	IEnumerator monster_dead(){
		group.act_stoper.set_stoper (true);
		var dead_anime = group.GameMgr.SkillAnimeMgr.Obtain ("dead_anime");
		float anime_time = 1F;
		dead_anime.SetOverTime (anime_time);
		dead_anime.SetEnable ();
		dead_anime.SetPosition (original_head);
		Image head = dead_anime.transform.GetChild (0).GetComponent<Image>();
		head.sprite = gameObject.GetComponent<Image>().sprite;
		gameObject.GetComponent<Image> ().color = Color.clear;
		yield return new WaitForSeconds (anime_time);
		group.act_stoper.set_stoper (false);
		gameObject.SetActive (false);
		group.check_win();
	}


	public bool is_alive(){
		return alive;
	}
	
	float mp2_timer = 0;
	private void add_mp2(){
		if (group.act_stoper.check_stoper ())
			return;

		mp2_timer += Time.deltaTime;    
		if( mp2_timer > 2.0){
			mp2_timer = 0;
			patch_mp(info.mp2);
		}
	}

	private void update_act(){
		if (group.act_stoper.check_stoper ())
			return;

		//patch_act (Time.deltaTime);
		patch_act (info.add_act*Time.deltaTime);
		float percent = act / info.max_act;

		if (percent >= 1) {
			act = 0;
			start_act ();
		}

		group.update_line_bar (Group.line_bar.ACT_LINE, Group.line_bar.MAX_ACT_LINE, this, percent);
	}

	private void update_hp(){
		if (ori_hp != info.hp) {
			float ori_percent = (float)ori_hp / (float)info.max_hp;
			float percent = (float)info.hp / (float)info.max_hp;
			float diff = (ori_percent - percent) / 5;
			float new_percent = ori_percent - diff;

			int patch = (int)(diff * info.max_hp);
			if (patch != 0)
				ori_hp -= patch;
			else
				ori_hp = info.hp;
				
			group.update_line_bar (Group.line_bar.HP_LINE, Group.line_bar.MAX_HP_LINE, this, new_percent);
			
			if( !is_monster )
				gameObject.GetComponent<LineBar>().hp_text.text = ori_hp +"/"+info.max_hp;

		} else {
			float percent = (float)info.hp / (float)info.max_hp;
			group.update_line_bar (Group.line_bar.HP_LINE, Group.line_bar.MAX_HP_LINE, this, percent);
			
			if( !is_monster )
				gameObject.GetComponent<LineBar>().hp_text.text = info.hp +"/"+info.max_hp;
		}
	}

	private void update_mp(){
		if (ori_mp != info.mp) {
			float ori_percent = (float)ori_mp / (float)info.max_mp;
			float percent = (float)info.mp / (float)info.max_mp;
			float diff = (ori_percent - percent) / 5;
			float new_percent = ori_percent - diff;

			int patch = (int)(diff * info.max_mp);
			if (patch != 0)
				ori_mp -= patch;
			else
				ori_mp = info.mp;

			group.update_line_bar (Group.line_bar.MP_LINE, Group.line_bar.MAX_MP_LINE, this, new_percent);
			
			if( !is_monster )
				gameObject.GetComponent<LineBar>().mp_text.text = ori_mp+"/"+info.max_mp;			
		} else {
			float percent = (float)info.mp / (float)info.max_mp;
			group.update_line_bar (Group.line_bar.MP_LINE, Group.line_bar.MAX_MP_LINE, this, percent);
			
			if( !is_monster )
				gameObject.GetComponent<LineBar>().mp_text.text = info.mp+"/"+info.max_mp;
		}
	}
	
	void FixedUpdate () {
		if (group) {
			update_act ();
			update_hp ();
			update_mp ();
			update_buff();
			update_buff_info();			
			shake_head ();
			
			// 處理2秒回魔
			add_mp2();
		}
	}

	public int patch_hp(int patch){
		ori_hp = info.hp;
		info.hp += patch;
		if ( info.hp > info.max_hp)
			info.hp = info.max_hp;

		if (info.hp < 0) {
			info.hp = 0;
			set_alive_status();
		}

		return info.hp;
	}

	public int patch_mp( int patch){
		ori_mp = info.mp;
		info.mp += patch;
		if (info.mp > info.max_mp)
			info.mp = info.max_mp;

		if (info.mp < 0 )
			info.mp = 0;

		return info.mp;
	}
	
	public float patch_act( float patch){
		if ( !this.actable() )
			return 0;

		act += patch;
		if (act > info.max_act)
			act = info.max_act;
		
		if (act < 0 )
			act = 0;

		return act;
	}

	public bool actable(){
		return alive;
	}

	public void set_target( int idx ){
		group.set_target (this, idx);
	}
	
	public void start_act(){
		int use_skill_id = info.skill_group [skill_idx];
		Skill use_skill = skill_list.get_skill_by_id (use_skill_id);
		use_skill.start_skill (this, group);
	}

	public void set_use_skill(int idx){
		//Debug.Log ("set_use:"+idx);
		skill_idx = idx;
		if( !is_monster ){
			MainData.dataCenter.characters.character_data[info.id].use_skill = idx;		
			update_use_skill_text();
		}
	}
	
	public void update_use_skill_text(){
		int use_skill_id = info.skill_group [skill_idx];
		Skill use_skill = skill_list.get_skill_by_id (use_skill_id);		
		gameObject.GetComponent<LineBar>().use_skill.text = use_skill.get_skill_show_name(this);		
	}

	float shake = 0f;
	float shakeAmount = 8f;
	float decreaseFactor = 3f;
	public void set_shake_head(float shake_time){
		shake = shake_time;
	}
	
	public void shake_head(){
		RectTransform head_pic = group.get_head_pic (this);
		if (shake > 0) {
			head_pic.position = original_head + Random.insideUnitCircle * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
			head_pic.position = original_head;
		}
	}
	
	public BuffEffect get_buff_effect(){
		BuffEffect totalEffect = new BuffEffect();		
		foreach(BattleBuff buff in buffData){
			BuffEffect effect = buff.get_effect(info);
			foreach(BuffEnum ability in System.Enum.GetValues(typeof(BuffEnum))){
				totalEffect.value[ability] += effect.value[ability];				
			}
		}
		
		return totalEffect;
	}
	
	public void add_buff(int id){
		BattleBuff battleBuff = MainData.dataCenter.buffs.getBuff(id);		
				
		bool already = false;
		foreach(BattleBuff buff in buffData ){
			if( buff.id == id ){
				buff.left_time = battleBuff.left_time;
				already = true;
			}			
		}
		
		if( !already )
			buffData.Add(battleBuff);
			
		if( !is_monster )
			update_use_skill_text();
	}
	
	void update_buff(){
		foreach(BattleBuff buff in buffData){
			buff.patchLeftTime(-Time.deltaTime);
			if( buff.close() )
				buffData.Remove(buff);						
		}
		
		if( !is_monster )
			update_use_skill_text();			
	}
	
	// 更新buff狀態顯示
	void update_buff_info(){
		Image[] buffIcon = gameObject.GetComponent<LineBar>().buff_img;
		Text[] buffTime = gameObject.GetComponent<LineBar>().buff_time;		
		int len = buffIcon.Length;
		for(int i=0; i<len ; i++){			
			buffIcon[i].color = Color.clear;
			buffTime[i].text = "";
		}
		
		for(int j=0; j<buffData.Count ; j++){			
			buffIcon[j].sprite = buffData[j].icon;
			buffIcon[j].color = Color.white;			
			buffTime[j].text = string.Format("{0:0}",buffData[j].left_time);			
		}		
	}
}
