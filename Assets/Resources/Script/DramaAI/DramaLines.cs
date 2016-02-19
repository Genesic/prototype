using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DramaLines : MonoBehaviour {

	public int lines_idx;	
	public Text show;
	public string[] lines;
	private Coroutine now_co;			
	
	public void start_show_text(){		
		if( lines_idx >= lines.Length )
			return;
			
		if( now_co != null )
			StopCoroutine(now_co);
			
		show.text = "";
		string show_line = lines[lines_idx];
		string[] res = show_line.Split(':');				
		if( res.Length > 1){			
			now_co = StartCoroutine(show_text_with_title(res[0],res[1]));		
		} else {			
			now_co = StartCoroutine(show_text(res[0]));
		}		
		lines_idx++;
	}
	
	public IEnumerator show_text(string content){
		foreach(char ch in content ){
			show.text += ch;
			if( ch.CompareTo('\a') == 0 )
				yield return new WaitForSeconds(0.2F);

			yield return new WaitForSeconds(0.01F);
		}
	}
	
	public IEnumerator show_text_with_title(string title, string content){
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
