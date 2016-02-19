using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillAnimeBasic : IPool
{		
    private RectTransform mTs = null;
	private float over_time;

    void Awake()
    {
		mTs = gameObject.GetComponent<RectTransform> ();
    }

    public void SetParam(string id)
    {
        mID = id;
    }

	public void SetPosition(Vector3 position)
	{
		mTs.position = position;
	}

	public void SetOverTime(float s_time ){
		over_time = s_time;
	}
	
	int actHash = Animator.StringToHash("act");
	public override void SetEnable ()
	{
		// float skill_time = 0.5f;
		base.SetEnable ();
		gameObject.GetComponent<Animator> ().SetTrigger(actHash);
		StartCoroutine(CalculateLifePeriod(over_time));
	}

	private IEnumerator CalculateLifePeriod(float over_time)
	{
		yield return new WaitForSeconds(over_time);
		
		SkillAnimeManager.Retrieve(this);
	}
}
