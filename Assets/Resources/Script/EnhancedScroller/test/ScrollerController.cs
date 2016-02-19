using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate {
	private List<ScrollerData> _data;

	public EnhancedScroller myScroller;
	public AnimalCellView animalCellViewPrefab;

	void Start(){
		_data = new List<ScrollerData> ();

		_data.Add (new ScrollerData("Lion", 1));
		_data.Add (new ScrollerData("Bear", 2));
		_data.Add (new ScrollerData("Eagle", 3));
		_data.Add (new ScrollerData("Dolphin", 4));
		_data.Add (new ScrollerData("Ant", 5));
		_data.Add (new ScrollerData("Cat", 6));
		_data.Add (new ScrollerData("Sparrow", 7));
		_data.Add (new ScrollerData("Dog", 8));

		myScroller.Delegate = this;
		myScroller.ReloadData ();
	}

	public int GetNumberOfCells(EnhancedScroller scroller){
		return _data.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex){
		return 30f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		AnimalCellView cellView = scroller.GetCellView (animalCellViewPrefab) as AnimalCellView;
		cellView.SetData (_data [dataIndex]);
		return cellView;
	}

	public EnhancedScrollerCellView InstantiateCellView(EnhancedScroller scroller){
		string path = string.Format ("Prefab/EnhancedScroller/AnimalCellButton");
		var ScrollerCellView = Instantiate ( Resources.Load<AnimalCellView> (path) ) as AnimalCellView;
		return ScrollerCellView;
	}
}
