using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConfirmEquip : MonoBehaviour {
	//public DataCenter dataCenter;
	public GameObject[] confirmEquip;

	public void set_confirm(ItemType confirmType, int seq){
		for (int i= (int)ItemType.WEAPON; i< (int)ItemType.EQUIP_NUM; i++) {
			if( i != (int)confirmType ){
				confirmEquip[i].SetActive(false);
			} else {
				confirmEquip[i].SetActive(true);
				var equip = MainData.dataCenter.get_equip_by_seq(confirmType, seq);
				if( equip == null ){
					confirmEquip[i].GetComponentInChildren<Text>().text = DataCenter.NONE_EQUIP;
				} else {
					confirmEquip[i].GetComponentInChildren<Text>().text = equip.show_name;
				}
			}
		}
	}
}
