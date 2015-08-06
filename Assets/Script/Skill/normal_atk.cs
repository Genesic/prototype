using UnityEngine;
using System.Collections;

public class normal_atk : Skill {
	
	public override bool use_skill( Player caster, Player[] targets ){
		int mp = caster.get_mp ();
		Player target = targets [0];
		if (mp > cost) {
			int atk = caster.get_atk ();
			int def = target.get_def ();
			int damage = atk - def;
			caster.patch_mp(-cost);
			target.patch_hp (-damage);

			return true;
		}

		return false;
	}
}
