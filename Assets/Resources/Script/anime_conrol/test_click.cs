using UnityEngine;
using System.Collections;

public class test_click : MonoBehaviour {

	public Animator anime;
	int actHash = Animator.StringToHash("act");

	void Start(){
		Debug.Log (anime);
	}

	public void test(){
		Debug.Log ("click");
		anime.SetTrigger (actHash);
	}
}
