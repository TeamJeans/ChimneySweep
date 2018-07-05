using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScrollControl : MonoBehaviour {

	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	ScrollRect scrollRect;
	[SerializeField]
	TileManager tileManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(tileManager.TileDragMode)
		{
			scrollRect.enabled = false;
		}

		if (swipeControls.SwipeDown || swipeControls.SwipeUp)
		{
			scrollRect.enabled = true;
		}
	}
}
