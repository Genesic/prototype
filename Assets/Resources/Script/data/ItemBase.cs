using UnityEngine;
using System.Collections;

public class ItemBase : MonoBehaviour {
    public int id;
    public string itemName;
    public ItemType itemType { get; private set;}
    public int price;    
    public void setItemType(ItemType type){
        itemType = type;
    }    
}
