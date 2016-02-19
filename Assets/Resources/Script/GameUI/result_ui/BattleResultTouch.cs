using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class BattleResultTouch : MonoBehaviour,  IPointerClickHandler {

	public battle_result battleResult;

	public void OnPointerClick(PointerEventData eventData){
		if (battleResult.state == battle_result.RESULT_STATE_PLAYER_END) {
			Animator player_result_over_anime = battleResult.playerResultObj.GetComponent<Animator>();
			player_result_over_anime.SetTrigger( battleResult.overHash );
		} else if( battleResult.state == battle_result.RESULT_STATE_ITEM_END ) {
			Animator item_result_over_anime = battleResult.itemResultObj.GetComponent<Animator>();
			item_result_over_anime.SetTrigger( battleResult.overHash );
		}
	}
}