using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MakeStatus : MonoBehaviour {
    public ConfirmMake confirmMake;
    public MakeScrollerController makeScrollerController;
    public ItemType nowType = ItemType.NONE_TYPE;
    public Image[] buttonImage;
    
    Image getImageByNowType(ItemType type){
        Dictionary <ItemType, int > type2Button = new Dictionary<ItemType, int>{
            {ItemType.NONE_TYPE, 0},
            {ItemType.MATERIAL, 1},
            {ItemType.WEAPON, 2},
            {ItemType.ARMOR, 3},
            {ItemType.HANDGUARD, 4},
            {ItemType.SHOES, 5},
        };
        
        if( type2Button.ContainsKey(type)) 
            return buttonImage[type2Button[type]];
            
        return null;
    }
    
    public void updateButtonColor(){
        Image use = getImageByNowType(nowType);
        foreach(Image button in buttonImage)
            button.color = Color.white;
        use.color = Color.yellow;
    }
    
    public void openConfirmMake(ItemType makeType, int resId){
        confirmMake.gameObject.SetActive(true);
        confirmMake.setMake(makeType, resId);
    }
    
    public void init_with_allplan(){
        nowType = ItemType.NONE_TYPE;
        confirmMake.gameObject.SetActive(false);
        makeScrollerController.init(ItemType.NONE_TYPE);        
    }
    
    public void init_with_type(int makeType){
        nowType = (ItemType)makeType;
        confirmMake.gameObject.SetActive(false);
        makeScrollerController.init(nowType);        
    }
    
    public void init_with_now_type(){        
        confirmMake.gameObject.SetActive(false);
        makeScrollerController.init(nowType);        
    }
}
