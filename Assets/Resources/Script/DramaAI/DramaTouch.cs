using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DramaTouch : MonoBehaviour, IPointerClickHandler {

	public DramaLines dramaLines;
	public void OnPointerClick(PointerEventData eventData){
		dramaLines.start_show_text();
	}

}
