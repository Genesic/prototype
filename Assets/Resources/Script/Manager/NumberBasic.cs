using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum NumberColor {
	WHITE = 0,
	GREEN = 1,
	RED = 2,
	BLUE = 3,
}
public class NumberBasic : IPool
{		
    private RectTransform mTs = null;
	public NumberManager NumMgr;
	
    void Awake()
    {
		mTs = gameObject.GetComponent<RectTransform> ();
    }

    public void SetParam(string id)
    {
        mID = id;
    }

	public void SetPosition(Vector3 position)
	{
		mTs.position = position;
	}

	public void SetNumber(NumberColor color, int number )
	{
		string num_string = number.ToString ();
		int i = 0;
		if (number > 0) {
			foreach (Transform child in transform) {
				int num = int.Parse (num_string [i].ToString ());
				Sprite pic = NumMgr.while_number [num];
/*
			if( color.CompareTo("white") == 0){
				pic = NumMgr.while_number[num];
			} else if( color.CompareTo("green") == 0 ){
				pic = NumMgr.green_number[num];
			} else if( color.CompareTo("red") == 0){
				pic = NumMgr.red_number[num];
			}
			pic = NumMgr.while_number[num];
*/			
				child.gameObject.GetComponent<Image> ().sprite = pic;
				if (color == NumberColor.GREEN) {
					child.gameObject.GetComponent<Image> ().color = Color.green;
				} else if ( color == NumberColor.RED ) {
					child.gameObject.GetComponent<Image> ().color = Color.red;
				} else if (color == NumberColor.BLUE ) {
					child.gameObject.GetComponent<Image> ().color = Color.blue;
				} else {
					child.gameObject.GetComponent<Image> ().color = Color.white;
				}
				i++;
			}
		}
	}

	int actHash = Animator.StringToHash("act");
	public override void SetEnable ()
	{
		base.SetEnable ();
		gameObject.GetComponent<Animator> ().SetTrigger(actHash);
		StartCoroutine(CalculateLifePeriod());
	}

	private IEnumerator CalculateLifePeriod()
	{
		yield return new WaitForSeconds(0.5f);

		NumberManager.Retrieve(this);
	}
}
