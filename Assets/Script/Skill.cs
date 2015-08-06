using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour {

	public int cost;

	public virtual bool use_skill ( Player caster, Player[] targets ){
		return true;
	}
}
