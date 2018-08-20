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
		if(tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().MouseOver || tileManager.TileDragMode)
		{
			scrollRect.enabled = false;
		}
		else
		{
			scrollRect.enabled = true;
		}
	}
}
