using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public CharacterStatus characterStatus;
	public StrengthStatus strengthStatus;
    public MakeStatus makeStatus;

	void Start(){
	}

	void close_all(){
		characterStatus.gameObject.SetActive (false);
		strengthStatus.gameObject.SetActive (false);
        makeStatus.gameObject.SetActive(false);
	}

	public void openCharacterStatus(){
		close_all ();		           
		characterStatus.gameObject.SetActive(true);
		int cid = characterStatus.get_select_cid ();
		if (cid >= 0) {
			characterStatus.init (cid);
		} else {
			characterStatus.init (0);
		}
	}

	public void openStrengthStatus(){
		close_all ();		
		strengthStatus.gameObject.SetActive (true);
		ItemType equip_type = strengthStatus.get_select_type ();		
		strengthStatus.init (equip_type);
	}
    
    public void openMakeStatus(){
        close_all ();
        makeStatus.gameObject.SetActive(true);
        makeStatus.init_with_now_type();
    }

}
