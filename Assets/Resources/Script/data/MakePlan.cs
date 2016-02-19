using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct MakeElement {
    public ItemType type;
    public int itemId;
    public int itemNum;
}
public class MakePlan : MonoBehaviour {    
    public int needSkill;
    public float needTime;
    public const int ELEMENT_NUM = 3;
    public ItemBase resItem;
    public int resNum = 1;
    public ItemBase[] elementItem;
    public int[] numList;    
 
    public List<MakeElement> getMakeElement(){        
        List<MakeElement> elementList = new List<MakeElement>();
        for(int i=0; i<elementItem.Length; i++){
            if( numList[i] > 0 ){
                MakeElement element = new MakeElement();
                element.type = elementItem[i].itemType;
                element.itemId = elementItem[i].id;
                element.itemNum = numList[i];
                elementList.Add(element);
            }
        }
        
        return elementList; 
    }
}
