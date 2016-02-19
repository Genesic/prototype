using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;

public class MakeCellView : EnhancedScrollerCellView {

    public Text makeTextName;
    private MakeScrollerData res;
    public MakeStatus makeStatus;
    int canMakeNum;
	public void SetData(MakeScrollerData data){        
        res = data;
        canMakeNum = MainData.dataCenter.makePlans.CalcCanMakeNum(res.resType, res.resId);
        // int nowNum = MainData.dataCenter.get_item_by_id(res.resType, res.resId).itemNum;
        makeTextName.text = data.resName+"("+canMakeNum+")";
	}
    
    public void selectMake(){
        makeStatus.openConfirmMake(res.resType, res.resId);           
    }

    void Update(){                
        if (canMakeNum > 0) {
            makeTextName.color = Color.black;            
        } else {                        
            makeTextName.color = Color.grey;            
        }
    }
}
