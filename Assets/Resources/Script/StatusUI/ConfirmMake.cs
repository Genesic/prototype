using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ConfirmMake : MonoBehaviour {

    public Text[] rawElement;
    public Text[] rawNum;
    public Text resName;
    
    public Image diff;
    public RectTransform panelRect;
    public GameObject fullScreen;
    public MakingItemPanel makingPanel;
    MakePlan usePlan;
    
    public void clearMake(){
        for(int i=0; i<rawElement.Length; i++){
            rawElement[i].text = "";
            rawNum[i].text = "";
            resName.text = "";
        }
    }
    
    float ori_height;
    float ori_width;
    void Awake(){
        ori_height = panelRect.rect.height;
        ori_width = panelRect.rect.width;
    }
    
    public void setSize(int elementNum){
        float add_size = rawElement[0].gameObject.GetComponent<RectTransform>().rect.height;
        panelRect.sizeDelta = new Vector2(ori_width, ori_height + (elementNum-1)*add_size);
    }
    
    public void setMake( ItemType makeType, int resId ){
         DataCenter dataCenter = MainData.dataCenter;
         usePlan = MainData.dataCenter.makePlans.getMakePlan(makeType, resId);        
         List<MakeElement> makeElement = usePlan.getMakeElement();
         clearMake();
         
         // 設定材料視窗的大小
         setSize(makeElement.Count);
                  
         // 設定材料視窗顯示的資料
         for(int i=0; i<makeElement.Count && i<rawElement.Length ; i++ ){
             int itemId = makeElement[i].itemId;
             ItemType itemType = makeElement[i].type;
             ItemData itemData = dataCenter.get_item_by_id(itemType, itemId);
             rawElement[i].text = itemData.itemName;
             rawNum[i].text = itemData.itemNum+"/"+makeElement[i].itemNum;
             rawNum[i].color = (itemData.itemNum < makeElement[i].itemNum )? Color.red : Color.black;
         }
                  
         resName.text = dataCenter.get_name_by_itemid(makeType, resId, 1);
         usePlan = MainData.dataCenter.makePlans.getMakePlan(makeType, resId);
    }
    
    public void startMake(bool isMakeAll){
        fullScreen.SetActive(true);
        makingPanel.init(usePlan, isMakeAll);
    }
}
