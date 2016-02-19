using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class add_atk_buff : Skill {	
	public override bool use_skill( Player caster, Group casterGroup, List<Player> targetGroup ){		
		StartCoroutine(cast_anime (caster, caster, casterGroup, -1, false));
		return true;
	}
}
