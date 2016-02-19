using UnityEngine;
using System.Collections;

public class MakeScrollerData {
    public string resName;
    public ItemType resType;
    public int resId;
    public int resNum;
    
    public MakeScrollerData(string name, ItemType type, int itemId, int num){
        resName = name;
        resType = type;
        resId = itemId;
        resNum = num;
    }

}
