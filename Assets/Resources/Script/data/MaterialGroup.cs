using UnityEngine;
using System.Collections;

public class MaterialGroup : MonoBehaviour {
	public static ItemType type = ItemType.MATERIAL;
	public MaterialItem[] material_data;

	public void init(){
		material_data = new MaterialItem[transform.childCount];

		foreach (Transform child in transform) {
			MaterialItem material = child.gameObject.GetComponent<MaterialItem>();
            material.setItemType(type);
			material_data[material.id] = material;
		}
	}
}
