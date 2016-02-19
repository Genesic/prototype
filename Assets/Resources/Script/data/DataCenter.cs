using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipList {
	public int id;
	public int lv;
	public int equiper;

	public EquipList(int item_id, int item_lv){
		equiper = -1;
		id = item_id;
		lv = item_lv;
	}

	public bool set_equiper(int character_id ){
		if (equiper < 0) {
			equiper = character_id;
			return true;
		}

		return false;
	}

	public void unequip( int charater_id ){
		if (equiper == charater_id )
			equiper = -1;
	}
}

public class Item {
	public ItemType item_type;
	public int item_id;
	public int item_num;
	public int item_lv;

	public Item(ItemType type, int id, int num, int lv){
		item_type = type;
		item_id = id;
		item_num = num;
		item_lv = lv;
	}
}

// 裝備道具的統一格式
public struct ItemData{
    public int itemId;
    public ItemType itemType;    
    public string itemName;
    public int itemNum;
    public int itemLv;
}

public enum ItemType {
    NONE_TYPE = -1, // 代表沒有這個TYPE
    WEAPON,
    ARMOR,
    HANDGUARD,
    SHOES,
    EQUIP_NUM, // 裝備道具的種類數量
    MATERIAL,  // 素材
}

public enum EquipType {
    NON_TYPE = 0,
    SWORD = 1,
    WAND = 2, //魔杖
    HEAVY_ARMOR = 3,
    LIGHT_ARMOR = 4,
}
public enum PatchRes{
    COST_EQUIP_NEED_SEQ = -4, // 要扣掉裝備需要seq才能扣
    ITEM_NOT_ENOUGH = -3,
    WRONG_TYPE = -2,
    ITEM_FULL = -1,
    SUCCESS = 1,
}
public class DataCenter : MonoBehaviour {

    public EquipGroup[] equipGroupObj;
	public Dictionary<ItemType, EquipGroup > equip = new Dictionary<ItemType, EquipGroup>();
	public const int WEAPON = 0;
	public const int ARMOR = 1;
	public const int HANDGUARD = 2;
	public const int SHOES = 3;
	public const int EQUIP_MAX = 4;
	public const int MATERIAL = 10;
	public const string NONE_EQUIP = "(none)";
	public const string NONE_SKILL = "(none)";
	
	public CharacterGroup characters;
	public EnemyGroup enemys;
	public SkillList skills;
	public MaterialGroup materials;
	public BuffGroup buffs;
    public int makeSkill;
    public MakeGroup makePlans;

	public int money;

	public int status_idx = 0;
	public int[] character_team;	
	//public List< List<EquipList> > equip_list = new List< List<EquipList> > {new List<EquipList>() , new List<EquipList>(), new List<EquipList>(), new List<EquipList>()};
    public Dictionary<ItemType, List<EquipList> > equipList = new Dictionary<ItemType, List<EquipList> >();
	public List<bool[]> equip_get = new List<bool[]> ();
	public const int ITEM_LIMIT = 100;
	public Item[] ItemList = new Item[100];
	public Dictionary<int, Item> materialList = new Dictionary<int, Item>();	 

	public int[] enemy_ids;
	public string enemy_group_name;

	void Awake(){		
		//init characters
		characters.init ();

		//init enemys
		enemys.init ();

		//init skills
		skills.init ();

		//init materials
		materials.init ();
		
		//init buffs
		buffs.init();
        
        //init makePlans;
        makePlans.init();

		//init equip_data
        foreach(EquipGroup data in equipGroupObj){
            data.init();            
            equip.Add(data.equipType, data );
        }
                                                   
        //init equipList        
        for(ItemType i = ItemType.WEAPON; i < ItemType.EQUIP_NUM ; i++ ){
            List<EquipList> list = new List<EquipList>();
            equipList.Add(i, list);                        
        }

		//gameObject.transform.FindChild("Character")
		EquipList sword = new EquipList(0, 5);
		sword.set_equiper (0);
		equipList [ItemType.WEAPON].Add (sword);

		EquipList sword2 = new EquipList(0, 1);
//		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword2);

		EquipList sword3 = new EquipList(0, 4);
		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword3);

		EquipList sword4 = new EquipList(0, 10);
		//		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword4);

		EquipList sword5 = new EquipList(0, 8);
		//		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword5);

		EquipList sword6 = new EquipList(0, 6);
		//		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword6);

		EquipList sword7 = new EquipList(0, 3);
		//		sword2.set_equiper (1);
		equipList [ItemType.WEAPON].Add (sword7);

		EquipList armor = new EquipList(0, 2);
		armor.set_equiper (0);
		equipList [ItemType.ARMOR].Add (armor);

		EquipList armor2 = new EquipList(0, 1);
		armor2.set_equiper (1);
		equipList [ItemType.ARMOR].Add (armor2);
        
        patch_item(ItemType.MATERIAL, 0, 1, 20);
        patch_item(ItemType.MATERIAL, 3, 1, 10);
	}

	public EquipAbility get_character_equip(int character_id, ItemType equip_type){        
		foreach (EquipList equip_base in equipList[equip_type]) {
			if( equip_base != null && equip_base.equiper == character_id ){                            
				return equip[equip_type].get_equip_ability(equip_base.id, equip_base.lv );
			}
		}
		
		return null;
	}

	public EquipAbility get_equip_by_seq(ItemType equip_type, int seq){
		if (seq < 0)
			return null;

		var info = equipList[equip_type][seq];
		return equip [equip_type].get_equip_ability (info.id, info.lv);
	}

	public Skill get_skill_by_id( int skill_id ){
		return skills.skill_list[skill_id];
	}

	public void set_character_equip(int cid, ItemType equip_type, int seq){
		foreach (var equip in equipList[equip_type]) {
			equip.unequip(cid);
		}

		if( seq >= 0 )
			equipList [equip_type] [seq].set_equiper (cid);
	}

	public bool check_money_patch(int patch){
		if (money + patch < 0)
			return false;

		return true;
	}

	public void patch_money(int patch){
		money += patch;
	}

	public void patch_equip_lv(ItemType equip_type, int seq, int patch){
		equipList [equip_type] [seq].lv += patch;
	}

    // 根據動態資料取得道具名稱
	public string get_name_by_itemid(ItemType item_type, int item_id, int item_lv){
		if (item_type >= ItemType.WEAPON && item_type <= ItemType.EQUIP_NUM) {
			var info = equip [item_type].get_equip_ability (item_id, item_lv);
			return info.show_name;
		} else if (item_type == ItemType.MATERIAL) {
			return materials.material_data[item_id].itemName;
		}

		return "";
	}
    
    // 取得道具資料
    public ItemData get_item_by_id(ItemType itemType, int itemId){
        ItemData itemData = new ItemData();
        itemData.itemType = itemType;
        itemData.itemId = itemId;
        itemData.itemNum = 0;       

        if (itemType >= ItemType.WEAPON && itemType < ItemType.EQUIP_NUM) {
            var equipData = equip[itemType].equip_data[itemId];
            itemData.itemName = equipData.itemName;
            foreach(var data in equipList[itemType]){
                if( data.id == itemId){
                    itemData.itemLv = data.lv;
                    itemData.itemNum += 1; 
                }
            }
        } else if(itemType == ItemType.MATERIAL){
            itemData.itemName = materials.material_data[itemId].itemName;
            if(materialList.ContainsKey(itemId) )
                itemData.itemNum =  materialList[itemId].item_num;
        }
        
        return itemData;
    }

	public void test_patch_item(){
		patch_item (ItemType.HANDGUARD, 0, 3, 1);
	}

	public PatchRes patch_items(Item[] items){
		foreach (Item item in items) {
			PatchRes result = patch_item( item.item_type, item.item_id, item.item_lv, item.item_num );
			if( (int)result < 0 )
				Debug.Log ( "type:"+item.item_type+" id:"+item.item_id+" res:"+result );
		}

		return PatchRes.SUCCESS;
	}

	public PatchRes patch_item(ItemType item_type, int item_id, int item_lv, int num){
		if (item_type >= ItemType.WEAPON && item_type <= ItemType.EQUIP_NUM) {
            if( num > 0){
			    if (equipList [item_type].Count >= ITEM_LIMIT)
				return PatchRes.ITEM_FULL;
							
			    EquipList equip_data = new EquipList (item_id, item_lv);			
			    equipList [item_type].Add (equip_data);
			    return PatchRes.SUCCESS;
            } else {
                return PatchRes.COST_EQUIP_NEED_SEQ;
            }
		} else if (item_type == ItemType.MATERIAL) {
			if( materialList.ContainsKey( item_id ) ){
				Item material = materialList[item_id] as Item;
                if( material.item_num + num < 0 ){
                    return PatchRes.ITEM_NOT_ENOUGH;
                } else if( material.item_num + num == 0 ){
                    materialList.Remove(item_id);
                } else {
				    material.item_num += num;
                }
			} else {
                if( num <= 0 )
                    return PatchRes.ITEM_NOT_ENOUGH;
                Item item = new Item(item_type, item_id, num, item_lv);
				materialList.Add( item_id, item );
			}

			return PatchRes.SUCCESS;
		}

		return PatchRes.WRONG_TYPE;
	}
}
