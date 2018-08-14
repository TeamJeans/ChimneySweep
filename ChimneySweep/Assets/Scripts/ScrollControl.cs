using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScrollControl : MonoBehaviour {

	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	ScrollRect scrollRect;
	public ScrollRect ScrollRect { get { return scrollRect; } set { scrollRect = value; } }
	[SerializeField]
	TileManager tileManager;

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
