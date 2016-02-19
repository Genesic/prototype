using UnityEngine;
using System.Collections;

public class SkillAnimeManager : ObjectPools<SkillAnimeManager, SkillAnimeBasic> {
	
	protected override Transform ContainerTs { get { return this.transform; } }

	protected override void Awake()
	{
		base.Awake();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
	
	public static void Retrieve(SkillAnimeBasic obj)
	{
		Instance.InsRetrieve(obj);
	}
	
	public override SkillAnimeBasic CreateNew(string id)
	{
		var path = string.Format("Prefab/skill/{0}", id);
		var prefab = Resources.Load<GameObject>(path);
		var skillGo = Instantiate(prefab) as GameObject;
		var skillTs = skillGo.transform;
		skillTs.SetParent(ContainerTs);

		SkillAnimeBasic setting = skillGo.GetComponent<SkillAnimeBasic> ();
		setting.SetParam (id);

		return setting;
	}
}
