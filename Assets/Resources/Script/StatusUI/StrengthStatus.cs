using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrengthStatus : MonoBehaviour {

	public Color select_color = Color.yellow;
	public const int IS_STRENGTH = 0;
	public const int IS_MAKE = 1;	
	private ItemType select_type = ItemType.WEAPON;
	private int select_seq = -1;

	public SelectScrollerController selectScrollerController;
	public ConfirmStrength confirmStrength;

	public Image[] type_buttom;
	public Image[] func_bottum;

	public void init(ItemType equip_type){		
		select_equip_type ((int)equip_type);
		select_seq = -1;
	}
	
	public ItemType get_select_type(){
		return select_type;
	}

	public int get_select_seq(){
		return select_seq;
	}

	void change_button_color(){
		for(int i=0; i<type_buttom.Length ; i++ ){
			if( i == (int)select_type ){
				type_buttom [i].color = select_color;
			} else {
				type_buttom[i].color = Color.white;
			}
		}
	}

	public void select_equip_type(int equipType){
        ItemType type = (ItemType)equipType;
		selectScrollerController.init (type);		
		select_type = type;
		change_button_color ();
		confirmStrength.gameObject.SetActive (false);
	}

	public void show_select_equip(int seq){
		select_seq = seq;
		confirmStrength.gameObject.SetActive (true);
		confirmStrength.init (select_type, seq);
	}
}
