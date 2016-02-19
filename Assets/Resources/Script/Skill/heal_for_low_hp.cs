using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class heal_for_low_hp : Skill {
	
	public override bool use_skill( Player caster, Group casterGroup, List<Player> targetGroup ){							
		int heal_hp = caster.info.cure + extra;
		patch = heal_hp;
		StartCoroutine(cast_anime (caster, targetGroup[0], casterGroup, heal_hp, false));
			
		return true;
	}
	
	int patch;
	public override void patch_after_anime(){
		targetGroup[0].patch_hp (patch);
	}
}
