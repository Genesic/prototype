using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LineBar : MonoBehaviour {

	public RectTransform hp_line;
	public RectTransform max_hp_line;
	public RectTransform mp_line;
	public RectTransform max_mp_line;
	public RectTransform act_line;
	public RectTransform max_act_line;
	public RectTransform head_pic;

	public Animator anime;
	public Text ch_name;
	public Text hp_text;
	public Text mp_text;
	
	public Text use_skill;
	public Image[] buff_img;
	public Text[] buff_time;
}
