using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

public class EquipScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {
	private List<EquipScrollerData> _data;
	public EnhancedScroller myScroller;
	public EquipCellView equipCellViewPrefab;
	public DataCenter dataCenter;
	public GameObject mainSelectPanel;
	public GameObject confirmPanel;
	public CharacterStatus characterStatus;

	public void init(ItemType equip_type, int character_id ){
		dataCenter = MainData.dataCenter;
		List<EquipList> equip_list = dataCenter.equipList [equip_type];
		_data = new List<EquipScrollerData> ();

		_data.Add(new EquipScrollerData(DataCenter.NONE_EQUIP, equip_type, -1, character_id) );

		for (int i=0; i<equip_list.Count; i++) {
			if( equip_list[i].equiper < 0 ){
				var equip = dataCenter.get_equip_by_seq(equip_type, i);
				_data.Add(new EquipScrollerData(equip.show_name, equip_type, i, character_id) );
			}
		}

		myScroller.Delegate = this;
		myScroller.ReloadData ();
	}

	public int GetNumberOfCells(EnhancedScroller scroller){
		return _data.Count;
	}

	public float GetCellViewSize(EnhancedScroller scoller, int dataIdx){
		return 60f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIdx, int cellIdx){
		EquipCellView cellView = scroller.GetCellView (equipCellViewPrefab) as EquipCellView;
		cellView.SetData (_data [dataIdx]);
		return cellView;
	}

	public EnhancedScrollerCellView InstantiateCellView(EnhancedScroller scroller){
		var ScrollerCellView = Instantiate ( equipCellViewPrefab ) as EquipCellView;
		ScrollerCellView.mainSelectPanel = mainSelectPanel;
		ScrollerCellView.confirmPanel = confirmPanel;
		ScrollerCellView.characterStatus = characterStatus;
		return ScrollerCellView;
	}
}
