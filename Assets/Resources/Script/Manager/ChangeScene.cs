using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	public void battle_scene(){
		MainData.dataCenter.enemy_ids = new int[3] {0, 0, 0};
		MainData.dataCenter.enemy_group_name = "slime_group";
		MainData.ins.battle_scene ();
	}
	public void battle_scene2(){
		MainData.dataCenter.enemy_ids = new int[4] {1, 0, 1, 0};
		MainData.dataCenter.enemy_group_name = "monster_group2";
		MainData.ins.battle_scene ();
	}

	public void status_scene(){
		MainData.ins.status_scene ();
	}
}
