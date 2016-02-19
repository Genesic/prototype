using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;

public class EquipCellView : EnhancedScrollerCellView {
	public Text equipNameText;
	public int character_id;
	public ItemType equip_type;
	public int seq;
	public GameObject mainSelectPanel;
	public GameObject confirmPanel;
	public CharacterStatus characterStatus;

	public void SetData(EquipScrollerData data){
		equipNameText.text = data.equipName;
		equip_type = data.equip_type;
		seq = data.equip_id;
		character_id = data.character_id;
	}


	public void select_equip(){
		confirmPanel.SetActive (true);
		confirmPanel.GetComponent<ConfirmEquip> ().set_confirm (equip_type, seq);
		mainSelectPanel.SetActive (false);
		characterStatus.show_equip_diff_value(equip_type, seq);
	}
}
