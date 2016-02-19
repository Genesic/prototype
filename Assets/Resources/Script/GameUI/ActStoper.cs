using UnityEngine;
using System.Collections;

public class ActStoper : MonoBehaviour {

	public bool in_use_skill;
	public bool game_over;

	void Start(){
		in_use_skill = true;
	}

	public bool check_stoper(){
		if (game_over) {
			return game_over;
		} else {
			return in_use_skill;
		}
	}

	public void set_game_over(bool flag){
		game_over = flag;
	}

	public void set_stoper(bool flag){
		in_use_skill = flag;
	}
}
