using System.Collections.Generic;
using UnityEngine;

public enum BuffEnum {
	ATK = 0,
	DEF = 1,
	MATK = 2,
	MDEF = 3,	
}

public class BuffEffect {
	public Dictionary<BuffEnum, int> value;
	public BuffEffect(){
		value = new Dictionary<BuffEnum, int>();
		foreach(BuffEnum ability in System.Enum.GetValues(typeof(BuffEnum))){
			value[ability] = 0;
		}
	}
} 

public class BattleBuff {
		
	public int id;	
	public int type;
	public float left_time;
	public int buff_value;	
	public Dictionary<BuffEnum, int> effects; 
	public Sprite icon;
	
	public BattleBuff(){
		effects = new Dictionary<BuffEnum, int>();
	}
	
	public void patchLeftTime(float patch){
		left_time += patch;
	}
	
	public bool close(){
		if( left_time <= 0)
			return true;
			
		return false;
	}
	
	public BuffEffect get_effect(PlayerStruct info){
		BuffEffect effect = new BuffEffect();
		effect.value[BuffEnum.ATK] = info.atk * effects[BuffEnum.ATK] / 100;
		effect.value[BuffEnum.DEF] = info.def * effects[BuffEnum.DEF] / 100;
		effect.value[BuffEnum.MATK] = info.matk * effects[BuffEnum.MATK] / 100;
		effect.value[BuffEnum.MDEF] = info.mdef * effects[BuffEnum.MDEF] / 100;
		
		return effect;
	}
}
