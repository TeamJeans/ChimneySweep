using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	[SerializeField]
	GameObject[] slots;
	[SerializeField]
	GameObject[] tileBackgrounds;
	[SerializeField]
	GameObject slotGlow;
	GameObject selectedSlot;

	[SerializeField]
	ChimneyTileTemplate emptyTileTemplate;

	[SerializeField]
	RemoveInventoryItem itemRemover;

	[SerializeField]
	Text[] itemValues;

	[SerializeField]
	ChimneyTileTemplate[] tileStored;
	public ChimneyTileTemplate[] TileStored { get { return tileStored; } }

	bool noEmptySlots = true;
	bool slotSelected = false;
	int freeSpace;
	Vector2 mousePosition;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < tileStored.Length; i++)
		{
			slots[i].GetComponent<InventorySlot>().SlotNum = i;
			//slots[i].GetComponent<SpriteRenderer>().sprite = tileStored[i].artwork;
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
				tileBackgrounds[i].transform.position = new Vector3(slots[i].transform.position.x, slots[i].transform.position.y, slots[i].transform.position.z);
			}

			// Check which slots have items in them
			if (tileStored[i].catagory != ChimneyTileTemplate.Catagory.EMPTY)
			{
				tileBackgrounds[i].SetActive(true);
			}
			else
			{
				tileBackgrounds[i].SetActive(false);
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

		// If the item has made contact with the bin icon and the player has took there finger off the screen the item in that slot will be deleted
		if (itemRemover.RemoveItem)
		{
			if (Input.GetMouseButtonUp(0))
			{
				itemRemover.RemoveItem = false;
				for (int i = 0; i < slots.Length; i++)
				{
					selectedSlot.GetComponent<SpriteRenderer>().sprite = emptyTileTemplate.artwork;
					tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum] = emptyTileTemplate;
				}
				selectedSlot.GetComponent<InventorySlot>().Selected = false;
			}
		}

		// If an item in the inventory is selected then show the inventory options
		if (slotSelected)
		{
			itemRemover.gameObject.SetActive(true);
		}
		else
		{
			itemRemover.gameObject.SetActive(false);
		}
	}

	public bool IsThereSpace()
	{
		// Check all the stored tiles, if one of them is empty store the index and return a true value
		noEmptySlots = true;

		int i = -1;
		while(noEmptySlots && i < tileStored.Length - 1)
		{
			i++;
			Debug.Log(i);
			if (tileStored[i].catagory == ChimneyTileTemplate.Catagory.EMPTY)
			{
				noEmptySlots = false;
				freeSpace = i;
			}
		}
		return !noEmptySlots;
	}

	public void AddItem(ChimneyTileTemplate newItem, int itemValue, Color color)
	{
		tileStored[freeSpace] = newItem;
		slots[freeSpace].GetComponent<SpriteRenderer>().sprite = newItem.artwork;
		itemValues[freeSpace].text = itemValue.ToString();
		itemValues[freeSpace].color = color;
		tileBackgrounds[freeSpace].transform.position = new Vector3(slots[freeSpace].transform.position.x, slots[freeSpace].transform.position.y, slots[freeSpace].transform.position.z);
	}
}
