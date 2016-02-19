using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class textText : MonoBehaviour {
	
	public Text show;
	// Use this for initialization
	void Start () {
		StartCoroutine(show_text("玩家","多打一點字測試\abc顯示的速度\n還有順便看看能不能夠測到換行"));
	}

	public IEnumerator show_text(string title, string content){
		show.text = title + ":\n";
		yield return new WaitForSeconds(0.2F);
		foreach(char ch in content ){
			show.text += ch;
			if( ch.CompareTo('\a') == 0 )
				yield return new WaitForSeconds(0.2F);

			yield return new WaitForSeconds(0.01F);
		}
	}
	
}
