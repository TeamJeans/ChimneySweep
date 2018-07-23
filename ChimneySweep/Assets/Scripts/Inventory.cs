using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	[SerializeField]
	GameObject[] slots;
	[SerializeField]
	GameObject slotGlow;
	GameObject selectedSlot;

	[SerializeField]
	ChimneyTileTemplate[] tileStored;
	public ChimneyTileTemplate[] TileStored { get { return tileStored; } }

	bool noEmptySlots = false;
	bool slotSelected = false;
	int freeSpace;
	Vector2 mousePosition;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < tileStored.Length; i++)
		{
			slots[i].GetComponent<InventorySlot>().SlotNum = i;
			slots[i].GetComponent<SpriteRenderer>().sprite = tileStored[i].artwork;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		slotSelected = false;
		for (int i = 0; i < tileStored.Length; i++)
		{
			if (slots[i].GetComponent<InventorySlot>().Selected)
			{
				slotSelected = true;
				selectedSlot = slots[i];
				slotGlow.transform.position = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, slotGlow.transform.position.z);
			}
		}

		if (slotSelected)
		{
			slotGlow.GetComponent<SpriteRenderer>().enabled = true;
		}
		else
		{
			slotGlow.GetComponent<SpriteRenderer>().enabled = false;
		}

		if (slotSelected)
		{
			if (selectedSlot.GetComponent<InventorySlot>().MouseOver && Input.GetMouseButton(0))
			{
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				selectedSlot.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedSlot.transform.position.z);
			}
			if (Input.GetMouseButtonUp(0))
			{
				selectedSlot.transform.position = new Vector3(selectedSlot.GetComponent<InventorySlot>().OriginalPosition.x, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.y, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.z);
			}
		}
	}

	public bool IsThereSpace()
	{
		// Check all the stored tiles, if one of them is empty store the index and return a true value
		noEmptySlots = true;
		for (int i = tileStored.Length -1; i >= 0; i--)
		{
			Debug.Log(i);
			if (tileStored[i].catagory == ChimneyTileTemplate.Catagory.EMPTY)
			{
				noEmptySlots = false;
				freeSpace = i;
			}
		}
		return !noEmptySlots;
	}

	public void AddItem(ChimneyTileTemplate newItem)
	{
		tileStored[freeSpace] = newItem;
		slots[freeSpace].GetComponent<SpriteRenderer>().sprite = newItem.artwork;
	}
}
