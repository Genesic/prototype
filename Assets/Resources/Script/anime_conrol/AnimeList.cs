using UnityEngine;
using System.Collections;

public class AnimeList : MonoBehaviour {
	public GameObject[] anime_list;

	public GameObject get_anime_by_id( int anime_id ){
		return anime_list[anime_id];
	}
}
