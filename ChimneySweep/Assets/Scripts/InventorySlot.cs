using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {

	[SerializeField]
	bool selected;
	public bool Selected { get { return selected; } set { selected = value; } }

	int slotNum;
	public int SlotNum { get { return slotNum; } set { slotNum = value; } }

	Vector3 originalPosition;
	public Vector3 OriginalPosition { get { return originalPosition; } }

	[SerializeField]
	bool mouseOver = false;
	public bool MouseOver { get { return mouseOver; } }

	[SerializeField]
	Inventory inventory;
	Swipe swipeControls;

	int itemValue;
	public int ItemValue { get { return itemValue; } set { itemValue = value; } }

	string catagoryName;
	public string CatagoryName { get { return catagoryName; } set { catagoryName = value; } }
	string catagoryDescription;
	public string CatagoryDescription { get { return catagoryDescription; } set { catagoryDescription = value; } }

	int chimneyTileTemplateIndex;
	public int ChimneyTileTemplateIndex { get { return chimneyTileTemplateIndex; } set { chimneyTileTemplateIndex = value; } }

	Sprite tileBackgroundSprite;
	public Sprite TileBackgroundSprite{get{ return tileBackgroundSprite; } set { tileBackgroundSprite = value; } }

	void Start()
	{
		originalPosition = transform.position;
		swipeControls = inventory.SwipeControls;
	}

	void OnMouseOver()
	{
		if (!mouseOver)
		{
			mouseOver = true;
		}
	}

	void OnMouseExit()
	{
		if (mouseOver)
		{
			mouseOver = false;
		}
	}

	void Update()
	{
		if (!mouseOver)
		{
			selected = false;
		}

		// Check if the user has dragged the tile
		if (inventory.TileStored[slotNum].catagory != ChimneyTileTemplate.Catagory.EMPTY && swipeControls.SwipeUp && mouseOver)
		{
			Debug.Log("SELECTED INVENTORY TILE");
			selected = true;
		}
		Debug.Log(selected);

		inventory.InventoryTileManager.ShowInventoryTileDescription(this);
	}
}
