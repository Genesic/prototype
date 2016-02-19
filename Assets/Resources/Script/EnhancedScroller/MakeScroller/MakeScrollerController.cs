using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

public class MakeScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {

    private List<MakeScrollerData> _data;
    public EnhancedScroller myScroller;
    public MakeCellView makeCellViewPrefab;
    public DataCenter dataCenter;
    public MakeStatus makeStatus;    
    
	public void init(ItemType equipType){        
		dataCenter = MainData.dataCenter;
		_data = new List<MakeScrollerData> ();
        
        List<MakePlan> makePlans = dataCenter.makePlans.getMakePlans(equipType);
        foreach(MakePlan makePlan in makePlans){
            var item = makePlan.resItem;
            int resId = item.id;
            int makeNum = dataCenter.makePlans.CalcCanMakeNum(item.itemType, resId);
            string resName = dataCenter.get_item_by_id(item.itemType, resId).itemName;
            _data.Add(new MakeScrollerData(resName, item.itemType , resId, makeNum));
        }
		
		myScroller.Delegate = this;
		myScroller.ReloadData ();
        
        // 更新選擇道具種類按鈕顏色
        makeStatus.updateButtonColor();
	}
	public int GetNumberOfCells(EnhancedScroller scroller){
		return _data.Count;
	}

	public float GetCellViewSize(EnhancedScroller scoller, int dataIdx){
		return 60f;
	}
	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIdx, int cellIdx){
		MakeCellView cellView = scroller.GetCellView (makeCellViewPrefab) as MakeCellView;
		cellView.SetData (_data [dataIdx]);
		return cellView;
	}
	
	public EnhancedScrollerCellView InstantiateCellView(EnhancedScroller scroller){
		var ScrollerCellView = Instantiate ( makeCellViewPrefab ) as MakeCellView;
        ScrollerCellView.makeStatus = makeStatus;
		return ScrollerCellView;
	}
	
}
