using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PointUp : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler {
	public CharacterStatus characterStatus;
	private Vector2 pointDown;
    
    private int nextHash = Animator.StringToHash("next"); 
    private int upHash = Animator.StringToHash("up");
	
	public void OnPointerDown(PointerEventData eventData){
		pointDown = eventData.position;
	}

	public void OnPointerUp(PointerEventData eventData){
		float diff = eventData.position.x - pointDown.x;
		if (diff > 20F) {
            characterStatus.moveCharactor.SetTrigger(upHash);
			characterStatus.change_select_character(-1);
			//Debug.Log (" ->");
		} else if (diff < -20F) {
            characterStatus.moveCharactor.SetTrigger(nextHash);
			characterStatus.change_select_character(1);
			//Debug.Log (" <-");
		}
	}
	
	public void OnPointerClick(PointerEventData eventData){
	}
}
