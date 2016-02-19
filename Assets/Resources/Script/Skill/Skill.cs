using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {

	public enum TargetType {
		SELF = 0,
		SELF_GROUP = 1,
		ENEMY = 2,
		ENEMY_GROUP = 3,
		SELF_LOWEST_HP = 4,
	}
	
	public enum ShowNameType{
		NONE = 0,
		ATK = 1,
		MATK = 2,
	}
		
	public int id;
	public int cost;
	public string skill_name;
	public string anime_id;
	public int extra;
	public int hit_rate;
	public int num_range;

	public float show_bar_time = 0.3f;
	public float shake_time = 0.3F;
	//public float number_time = 1F;
	public float cast_time = 0.5f;
	public float skill_time = 0.15f;
	public NumberColor numberColor = NumberColor.WHITE;
	public int buffId;
	public ShowNameType show_name_type;
	
	public TargetType targetType;

	public GameManager gameMgr;

	private Hashtable element_map;
	private float[,] addition;

	public const int ATR_NORMAL = 0;
	public const int ATR_FIRE = 1;
	public const int ATR_WIND = 2;
	public const int ATR_WATER = 3;
	public const int ATR_HOLY = 4;
	public const int ATR_DARK = 5;

	public static float calc_element_addition (string cast_name, string target_name){
		Hashtable element_map = new Hashtable ();
		element_map ["NORMAL"] = ATR_NORMAL;
		element_map ["FIRE"] = ATR_FIRE;
		element_map ["WIND"] = ATR_WIND;
		element_map ["WATER"] = ATR_WATER;
		element_map ["HOLY"] = ATR_HOLY;
		element_map ["DARK"] = ATR_DARK;
		int max = 6;

		float [,] addition = new float[max,max];
		for (int i=0; i<max ; i++ )
			for( int j=0; j<max ; j++ )
				addition[i, j] = 1.0f;

		addition[ATR_FIRE, ATR_WIND] = 1.5f;
		addition[ATR_FIRE, ATR_WATER] = 0.5f;
		addition[ATR_WIND, ATR_WATER] = 1.5f;
		addition[ATR_WIND, ATR_FIRE] = 0.5f;
		addition[ATR_WATER, ATR_FIRE] = 1.5f;
		addition[ATR_WATER, ATR_WIND] = 0.5f;
		addition[ATR_HOLY, ATR_DARK] = 1.5f;
		addition[ATR_DARK, ATR_HOLY] = 1.5f;

		int cast = ( element_map.ContainsKey(cast_name) )? (int)element_map [cast_name] : ATR_NORMAL;
		int target = ( element_map.ContainsKey(target_name) )? (int)element_map [target_name] : ATR_NORMAL;
		return addition[cast, target];
	}

	public bool check_mp (Player caster){
		return (caster.info.mp > cost) ? true : false;
	}
	public string get_skill_show_name(Player caster){
		string show_name = skill_name;
		int buffValue = 0;
		int total = 0;
				
		// 判斷要顯示的技能數值是哪種
        if (show_name_type == ShowNameType.ATK ){
			show_name += "\n(ATK:";			
			buffValue = caster.get_buff_effect().value[BuffEnum.ATK];
			total = caster.info.atk + caster.get_buff_effect().value[BuffEnum.ATK];
		} else if(show_name_type == ShowNameType.MATK ) {
			show_name += "\n(MATK:";
			buffValue = caster.get_buff_effect().value[BuffEnum.MATK];
			total = caster.info.matk + caster.get_buff_effect().value[BuffEnum.MATK];			
		} else {
			//沒有要顯示數值的直接回傳技能名稱
			return show_name;
		}
		
		
		//判斷要顯示的技能數值是什麼顏色
		if( buffValue > 0){
			show_name += "<color=green>+" + total + "</color>)";	
		} else if( buffValue == 0 ){
			show_name += "" + total + ")";
		} else {
			show_name += "<color=red>-" + total + "</color>)";
		}
		
		return show_name;
    }

	public int calc_damage(Player caster, Player target){
		BuffEffect casterBuff = caster.get_buff_effect();
        int atk = caster.info.atk + casterBuff.value[BuffEnum.ATK];
		if( target == null ){
			return atk + extra;
		}		
		BuffEffect targetBuff = target.get_buff_effect();				
		int def = target.info.def + targetBuff.value[BuffEnum.DEF];		
		int check = atk - def;
		if( check < 0)
			check = 0;			
		
		int damage = check + extra;		 
		int add = Random.Range (-damage*num_range/100, damage*num_range/100);
		damage += add;
        if( damage > 0 )
		  return damage;
        else 
          return 0;
	}

	public int calc_magic_damage(Player caster, Player target){
		BuffEffect casterBuff = caster.get_buff_effect();
        int atk = caster.info.matk + casterBuff.value[BuffEnum.MATK];
        if( target == null ){
            return atk + extra;
        }
		BuffEffect targetBuff = target.get_buff_effect();        
        int def = target.info.mdef + targetBuff.value[BuffEnum.MDEF];
        int check = atk - def;
        if(check < 0)
            check = 0;
            
		int damage = check + extra;
		int add = Random.Range (-damage*num_range/100, damage*num_range/100);
		damage += add;
        if(damage > 0)
		  return damage;
        else
          return 0;
	}

	public void skill_init( float s_time){
		skill_time = s_time;
	}
		
	protected List<Player> targetGroup;
	public void start_skill(Player caster, Group casterGroup){
		// 檢查mp是否足夠
		if( !check_mp(caster) )
			return;
		
		targetGroup = new List<Player>();
		// 設定target
		if( targetType == TargetType.SELF ){			
			targetGroup.Add(caster);
		} else if( targetType == TargetType.SELF_GROUP ){			
			foreach( Player player in casterGroup.player_group){
				targetGroup.Add(player);
			}
		} else if( targetType == TargetType.ENEMY ){
			Player target = casterGroup.get_target (caster);
			targetGroup.Add(target);
		} else if( targetType == TargetType.ENEMY_GROUP ){
			Group enemyGroup = casterGroup.get_target (caster).group;
			foreach( Player enemy in enemyGroup.player_group){
				targetGroup.Add(enemy);
			}
		} else if( targetType == TargetType.SELF_LOWEST_HP ){
			int min = -1;
			Player target = null;
			foreach (Player player in casterGroup.player_group) {
				if( player.is_alive() ){
					if( min < 0 || player.info.hp < min ){
						target = player;
						min = target.info.hp;
					}
				}
			}
			
			targetGroup.Add(target);
		}
		
		use_skill(caster, casterGroup, targetGroup);
	}
	
	public virtual bool use_skill ( Player caster, Group casterGroup, List<Player> targetGroup ){
		return true;
	}

	public virtual void patch_after_anime (){}
	
	public void buff_after_anime(Player target){
		if( buffId > 0)
			target.add_buff(buffId);			
	}

	// create number anime
	public void createNumber(Player target, int damage, NumberColor color){
		int size = damage.ToString ().Length;
		string id;
		if (damage > 0) {
			id = size + "num";
		} else {
			id = "miss";
		}
		var number = gameMgr.NumberMgr.Obtain (id);
		number.SetNumber (color, damage);
		Vector2 position = target.gameObject.GetComponent<RectTransform>().position;
		if (target.is_monster) {
			position = target.gameObject.GetComponent<RectTransform> ().position;
		} else {
			position = target.original_head;
		}
		
		number.SetPosition (position);
		number.SetEnable ();
	}

	private int actHash = Animator.StringToHash("act");

	//施放動畫
	public IEnumerator cast_anime(Player caster, Player target, Group caster_group, int damage, bool is_atk){
		caster_group.act_stoper.set_stoper(true);
		Animator anime = caster.gameObject.GetComponent<LineBar> ().anime;
		if (anime) {
			anime.SetTrigger (actHash);
			yield return new WaitForSeconds (cast_time);
		}

		// 技能動畫
		var skill_anime = gameMgr.SkillAnimeMgr.Obtain (anime_id);
		RectTransform animeRt = skill_anime.GetComponent<RectTransform> ();
		if (target.is_monster) {
			// 動畫施放在怪物上
			animeRt.position = target.gameObject.GetComponent<RectTransform>().position;
		} else {
			// 動畫施放在玩家上
			animeRt.position = target.original_head;
		}	
		skill_anime.SetOverTime (skill_time);
		skill_anime.SetEnable ();
		yield return new WaitForSeconds (skill_time);

		// 結束動畫(數字與震動)
		if( damage >= 0)
			createNumber (target, damage, numberColor);
		if (is_atk)
			target.set_shake_head (shake_time);

		patch_after_anime ();
		buff_after_anime(target);
		yield return new WaitForSeconds (shake_time);
		
		// yield return new WaitForSeconds (show_bar_time);
		caster_group.act_stoper.set_stoper(false);
	}
}
