using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;

public class SelectCellView : EnhancedScrollerCellView {
	public Text selectNameText;
	public ItemType equip_type;
	public int seq;
	//public GameObject strengthPanel;
	public StrengthStatus strengthStatus;

	public void SetData(SelectScrollData data){
		selectNameText.text = data.equipName;
		equip_type = data.equip_type;
		seq = data.equip_seq;
	}

	public void select_equip(){
		strengthStatus.show_select_equip (seq); 
	}

	void Update(){
		if (strengthStatus.get_select_seq () == seq) {
			gameObject.GetComponent<Image>().color = Color.yellow;
		} else {
			gameObject.GetComponent<Image>().color = Color.white;
		}
	}
}
