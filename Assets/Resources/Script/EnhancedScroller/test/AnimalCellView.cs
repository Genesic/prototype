using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class AnimalCellView : EnhancedScrollerCellView {

	public Text animalNameText;
	public int id;

	public void SetData(ScrollerData data){
		animalNameText.text = data.animalName;
		id = data.id;
	}

	public void show_id(){
		Debug.Log (id);
	}
}
