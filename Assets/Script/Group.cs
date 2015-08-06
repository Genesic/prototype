using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour {

	public Player[] player_group;
	public Player[] enemy_group;

	public Player[] get_enemy_group(Player player){
		foreach (Player check in player_group) {
			if( check == player )
				return enemy_group;
		}

		foreach (Player check in enemy_group) {
			if( check == player )
				return player_group;
		}

		return null;
	}

	public Player[] get_team_group(Player player){
		foreach (Player check in player_group) {
			if( check == player )
				return player_group;
		}

		foreach (Player check in enemy_group) {
			if( check == player )
				return enemy_group;
		}

		return null;
	}
}
