using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MakingItemPanel : MonoBehaviour {
    public Text makeItemName;
    public RectTransform maxLine;
    public RectTransform nowLine;
    public RectTransform elementPanel;
    public Text[] elements;
    public MakePlan usePlan;    
    public GameObject cancelButton;
    public GameObject closeGroup;
    public GameObject fullScreen;
    public MakeStatus makeStatusPanel;
    float ori_height;
    float ori_width;
    float add_height;
    [SerializeField]
    private float timer;
    float timer_max;
    bool cancelFlag = false;
    public bool isMakeAll;
    
    private DataCenter dataCenter;
    void Awake(){
        dataCenter = MainData.dataCenter;
        add_height = elements[0].gameObject.GetComponent<RectTransform>().rect.height;
        ori_height = elementPanel.rect.height;
        ori_width = elementPanel.rect.width;        
    }
    
    void Update(){
        if( timer > 0){            
            timer -= Time.deltaTime;
            if( timer < 0)
                timer = 0;
                
            updateLine();
            
            // 製作時間到了的話進行完成處理
            if( timer <= 0 )
                makeClose(cancelFlag);                                  
        }
    }
    
    void makeClose(bool isCancel){
        // 進度條歸零
        clearLine();
                
        //開啟結束按鈕並關閉取消按鈕
        cancelButton.SetActive(false);
        closeGroup.SetActive(true);
        
        // 不是因為被取消而結束的話進行其他判斷
        if( !isCancel ){
            makePlanItem();
            updateMakeNum(usePlan);
            
            // 如果是選擇製作全部的話就繼續製作(無法製作的話init會失敗)
            if( isMakeAll )
                init(usePlan, isMakeAll);
        }
    }
    
    public void test_button(){
        var plan = dataCenter.makePlans.getMakePlan(ItemType.MATERIAL, 4);
        init(plan, true);
    }
    
    // 更新製作進度條
    void updateLine(){
        float percent = (timer_max - timer) / timer_max;        
        nowLine.sizeDelta = new Vector2 (maxLine.rect.width * percent , maxLine.rect.height );
    }
    
    //歸零進度條
    void clearLine(){
        nowLine.sizeDelta = new Vector2 (0 , maxLine.rect.height );
    }
    
    public void makePlanItem(){
        var getItem = usePlan.resItem;        
        dataCenter.patch_item(getItem.itemType, getItem.id, 1, usePlan.resNum);
        List<MakeElement> makeElement = usePlan.getMakeElement();
        foreach(var element in makeElement ){            
            PatchRes res = dataCenter.patch_item(element.type, element.itemId, 1, -element.itemNum);
            if( res != PatchRes.SUCCESS ){
                isMakeAll = false;
                return;                   
            }                
        }
    }

    // 更新製作中的道具數量    
    public bool updateMakeNum(MakePlan makePlan){
        var getItem = makePlan.resItem;
        ItemData makeItem = dataCenter.get_item_by_id(getItem.itemType, getItem.id);
        makeItemName.text = makeItem.itemName + "("+makeItem.itemNum+")";
        List<MakeElement> makeElement = makePlan.getMakeElement();
        elementPanel.sizeDelta = new Vector2(ori_width, ori_height + makeElement.Count*add_height);
        
        bool checkFlag = true;
        foreach(Text element in elements ){
            element.color = Color.clear;
        }
        
        for(int i=0; i< makeElement.Count ; i++){
            var eItem = makeElement[i];
            ItemData info = dataCenter.get_item_by_id(eItem.type, eItem.itemId);
            elements[i].text = info.itemName + " ("+info.itemNum+"/"+eItem.itemNum+")";
            elements[i].color = Color.black;
            if( info.itemNum < eItem.itemNum )
                checkFlag = false;            
        }
        
        return checkFlag;
    }

    public void init(MakePlan makePlan, bool makeAll){        
        usePlan = makePlan;
        isMakeAll = makeAll;
        cancelFlag = false;                        
        bool checkFlag = updateMakeNum(makePlan);                                  
        if( checkFlag ) {
            timer_max = timer = makePlan.needTime;
            //開啟取消按鈕並關閉結束按鈕
            cancelButton.SetActive(true);
            closeGroup.SetActive(false);
        } else
            isMakeAll = false;
                           
    }
    
    // 結束按鈕
    public void closePanel(){        
        fullScreen.SetActive(false);
        makeStatusPanel.init_with_now_type();
    }
    
    // 再來一次按鈕
    public void reMake(){
        init(usePlan, false);
    }
    
    // 取消按鈕
    public void cancelMake(){
        timer = 0;
        cancelFlag = true;
        isMakeAll = false;
        makeClose(cancelFlag);
    }
}
