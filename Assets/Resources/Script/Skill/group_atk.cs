using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class group_atk : Skill {
				
	public override bool use_skill( Player caster, Group casterGroup, List<Player> targetGroup ){				
		int idx = 0;
		patchs = new int[targetGroup.Count];
		foreach (Player target in targetGroup) {
			if (target.is_alive ()) {
				int damage = calc_damage(caster, target);
				int prob = Random.Range (0, 100);
				if (prob < hit_rate) {
					// hit	
					patchs[idx++] = -damage;						
					StartCoroutine (cast_anime (caster, target, casterGroup, damage, true));
				} else {
					// miss
					StartCoroutine (cast_anime (caster, target, casterGroup, -1, true));
				}
			}
		}

		return true;
	}
	int[] patchs;
	public override void patch_after_anime(){
		int i=0;
		foreach(Player target in targetGroup ){
			target.patch_hp(patchs[i++]);
		}		
	}
}
