using UnityEngine;
using System.Collections;

public class ActBar : MonoBehaviour {

	public Canvas canvas;
	public GameObject act_line;
	public Player player;

	private float ori_width;
	private float ori_height;

	void Start (){
		RectTransform rt = act_line.GetComponent<RectTransform> ();
		ori_width = rt.rect.width;
		ori_height = rt.rect.height;
	}

	void FixedUpdate () {
		float now_act = player.patch_act (Time.deltaTime);
		float max_act = player.get_max_act ();
		float percent = now_act / max_act;
		if ( percent >= 1) {
			act_line.GetComponent<RectTransform> ().sizeDelta = new Vector2 (ori_width, ori_height);
			player.set_act (0);
		} else {
			act_line.GetComponent<RectTransform> ().sizeDelta = new Vector2 (ori_width * percent, ori_height);
		}
	}
}
