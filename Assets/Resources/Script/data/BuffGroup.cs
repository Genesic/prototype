using UnityEngine;
using System.Collections;

public class BuffGroup : MonoBehaviour {

	public Buff[] buffData;
	
	public void init () {
		buffData = new Buff[transform.childCount];
		foreach(Transform child in transform){
			Buff buff = child.gameObject.GetComponent<Buff>();
			buffData[buff.id] = buff;			
		}
	}
	
	public BattleBuff getBuff(int id){
		Buff buff = buffData[id];
		BattleBuff battleBuff = new BattleBuff();
		battleBuff.id = buff.id;
		battleBuff.left_time = buff.buffTime;
		battleBuff.icon = buff.buffIcon;
				
		battleBuff.effects[BuffEnum.ATK] = buff.atkEffect;
		battleBuff.effects[BuffEnum.DEF] = buff.defEffect;
		battleBuff.effects[BuffEnum.MATK] = buff.matkEffect;
		battleBuff.effects[BuffEnum.MDEF] = buff.mdefEffect;		
		
		return battleBuff;
	}
}
