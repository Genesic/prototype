using UnityEngine;
using System.Collections;

public class MainData : MonoBehaviour {

	public static DataCenter dataCenter;
	public static MainData ins;
		
	// Use this for initialization
	void Awake () {
		if (dataCenter == null) {
			dataCenter = gameObject.GetComponent<DataCenter>();
			ins = this;
			GameObject.DontDestroyOnLoad (gameObject);
		} else if (dataCenter != gameObject.GetComponent<DataCenter>() ) {
			Destroy(gameObject);
			//Debug.Log ("hello");
			//Debug.Log ("ins:"+ins.GetInstanceID()+" dataCenter:"+dataCenter.GetInstanceID() );
		}
	}

	public void battle_scene(){
		//Debug.Log ("ins:"+ins.GetInstanceID()+" dataCenter:"+dataCenter.GetInstanceID() );
		Application.LoadLevel (1);
	}

	public void status_scene(){
		Application.LoadLevel (0);
	}
}