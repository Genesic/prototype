using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

public class SelectScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {
	private List<SelectScrollData> _data;
	public EnhancedScroller myScroller;
	public SelectCellView selectCellViewPrefab;
	public DataCenter dataCenter;
	public StrengthStatus strengthStatus;

	public void init(ItemType equip_type){
		dataCenter = MainData.dataCenter;
		List<EquipList> equip_list = dataCenter.equipList [equip_type];
		_data = new List<SelectScrollData> ();
		
		for (int i=0; i<equip_list.Count; i++) {
			var equip = dataCenter.get_equip_by_seq(equip_type, i);
			string equip_name = equip.show_name;
			if( equip_list[i].equiper >= 0 )
				equip_name = equip_name + " (E)";

			_data.Add(new SelectScrollData(equip_name, equip_type, i) );
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
		SelectCellView cellView = scroller.GetCellView (selectCellViewPrefab) as SelectCellView;
		cellView.SetData (_data [dataIdx]);
		return cellView;
	}
	
	public EnhancedScrollerCellView InstantiateCellView(EnhancedScroller scroller){
		var ScrollerCellView = Instantiate ( selectCellViewPrefab ) as SelectCellView;
		ScrollerCellView.strengthStatus = strengthStatus;
		return ScrollerCellView;
	}
}
