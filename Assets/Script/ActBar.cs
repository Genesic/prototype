using UnityEngine;
using System.Collections;

public class ActBar : MonoBehaviour {

	public float now_act;
	public float max_act;
	public Canvas canvas;

	public GameObject act_line;

	private float ori_width;
	private float ori_height;

	void Start (){
		now_act = 0;

		RectTransform rt = act_line.GetComponent<RectTransform> ();
		ori_width = rt.rect.width;
		ori_height = rt.rect.height;
	}

	void FixedUpdate () {
		now_act += Time.deltaTime;
		float percent = now_act / max_act;
		act_line.GetComponent<RectTransform>().sizeDelta = new Vector2 (ori_width * percent,  ori_height );

		if (now_act > max_act) {
			now_act = 0;
		}
	}
}
