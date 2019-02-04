using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour {

	bool selected = false;
	public bool Selected { get { return selected; } set { selected = value; } }

	bool beingDragged = false;
	public bool BeingDragged { get { return beingDragged; } set { beingDragged = value; } }

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

	float elapsedTimeForTileBeingHeldDown = 0f;
	public float ElapsedTimeForTileBeingHeldDown { get { return elapsedTimeForTileBeingHeldDown; } set { elapsedTimeForTileBeingHeldDown = value; } }
	[SerializeField]
	float lengthOfTimeTileNeedsToBeHeldDown = 1f;
	public float LengthOfTimeTileNeedsToBeHeldDown { get { return lengthOfTimeTileNeedsToBeHeldDown; } }

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
		mouseOver = true;
	}

	void OnMouseExit()
	{
		mouseOver = false;
	}

	void Update()
	{
		// Check if the user has tapped the inventory item
		if (inventory.TileStored[slotNum].catagory != ChimneyTileTemplate.Catagory.EMPTY && mouseOver && Input.GetMouseButtonDown(0))
		{
			Debug.Log("SELECTED INVENTORY TILE");
			selected = true;
		}
		//Debug.Log(selected);

		if (selected && swipeControls.SwipeUp)
		{
			beingDragged = true;
		}

		if (!mouseOver && Input.GetMouseButtonDown(0))
		{
			selected = false;
			beingDragged = false;
		}

		inventory.InventoryTileManager.ShowInventoryTileDescription(this);
	}
}
