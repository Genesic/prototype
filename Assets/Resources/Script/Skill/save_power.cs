using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class save_power : Skill {
			
	public override bool use_skill( Player caster, Group casterGroup, List<Player> targetGroup ){						
		StartCoroutine(cast_anime (caster, caster, casterGroup, cost, false));
		return true;
	}
		
	public override void patch_after_anime(){		
		targetGroup[0].patch_mp (cost);
	}
}
