using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class normal_atk : Skill {
				
	public override bool use_skill( Player caster, Group casterGroup, List<Player> targetGroup ){
						
		int damage = calc_damage(caster, targetGroup[0]);;
		int prob = Random.Range(0, 100);
		if( prob < hit_rate ){
			// hit		
			patch = -damage;
			StartCoroutine(cast_anime (caster, targetGroup[0], casterGroup, damage, true));
		} else {
			// miss
			StartCoroutine(cast_anime (caster, targetGroup[0], casterGroup, -1, true));
		}

		return true;
	}	
	int patch;
	public override void patch_after_anime(){
		targetGroup[0].patch_hp (patch);
	}
}
