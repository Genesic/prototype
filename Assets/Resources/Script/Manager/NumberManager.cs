using UnityEngine;
using System.Collections;

public class NumberManager : ObjectPools<NumberManager, NumberBasic> {

	public Sprite[] while_number;
	public Sprite[] green_number;
	public Sprite[] red_number;

	protected override Transform ContainerTs { get { return this.transform; } }

	protected override void Awake()
	{
		base.Awake();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
	
	public static void Retrieve(NumberBasic obj)
	{
		Instance.InsRetrieve(obj);
	}
	
	public override NumberBasic CreateNew(string id)
	{
		var path = string.Format("Prefab/numbers/{0}", id);
		var prefab = Resources.Load<GameObject>(path);
		var numGo = Instantiate(prefab) as GameObject;
		var numTs = numGo.transform;
		numTs.SetParent(ContainerTs);

		NumberBasic setting = numGo.GetComponent<NumberBasic> ();
		setting.SetParam (id);
		setting.NumMgr = this;

		return setting;
	}
}
