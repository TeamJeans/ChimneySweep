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

	void Start()
	{
		originalPosition = transform.position;
	}

	void OnMouseOver()
	{
		//Debug.Log(inventory.TileStored[slotNum].catagory);

		if (!mouseOver)
		{
			mouseOver = true;
		}
		// Check if the user has tapped
		if (Input.GetMouseButtonDown(0) && inventory.TileStored[slotNum].catagory != ChimneyTileTemplate.Catagory.EMPTY)
		{
			selected = true;
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
		if (!mouseOver && Input.GetMouseButtonDown(0))
		{
			selected = false;
		}
	}
}
