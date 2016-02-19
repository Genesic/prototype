using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeGroup : MonoBehaviour {
    public MakePlan[] makeGroup;
    public void init(){
        makeGroup = new MakePlan[transform.childCount];
        int idx = 0;
        foreach(Transform child in transform){            
            makeGroup[idx++] = child.gameObject.GetComponent<MakePlan>();
        }
    }
    
    // 根據成品種類取得所有該類可製作道具
    public List<MakePlan> getMakePlans(ItemType makeType){        
        List<MakePlan> makePlans = new List<MakePlan>();
        if( makeType == ItemType.NONE_TYPE ){            
            foreach(var plan in makeGroup){
                makePlans.Add(plan);
            }
        } else {
            foreach(MakePlan plan in makeGroup){                
                if( plan.resItem.itemType == makeType ){
                    makePlans.Add(plan);
                }
            }
        }
        
        return makePlans;
    }
    
    public MakePlan getMakePlan(ItemType makeType, int resId){
        foreach(var makePlan in makeGroup){
            if( makePlan.resItem.itemType == makeType && makePlan.resItem.id == resId){
                return makePlan;
            }
        }
        
        return null;
    }
    
    // 根據成品取得製作材料
    public List<MakeElement> getMakeElement(ItemType makeType, int resId){
        MakePlan plan = getMakePlan(makeType, resId);
        if( plan == null )
            return null;
            
        return plan.getMakeElement();
    }

    // 計算身上材料可製作的成品數量    
    public int CalcCanMakeNum(ItemType makeType, int resId){
        var elementList = getMakeElement(makeType, resId);
        int min_num = -1;
        foreach(var element in elementList ){
            var item = MainData.dataCenter.get_item_by_id(element.type, element.itemId);
            int count = item.itemNum / element.itemNum;
            if( min_num < 0 || count < min_num)
                min_num = count;            
        }
        
        return min_num;
    }
}
