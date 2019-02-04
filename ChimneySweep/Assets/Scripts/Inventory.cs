using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	[SerializeField]
	GameMaster gm;
	[SerializeField]
	TileManager tileManager;
	public TileManager InventoryTileManager { get { return tileManager; } }
	[SerializeField]
	Swipe swipeControls;
	public Swipe SwipeControls { get { return swipeControls; } }

	[SerializeField]
	GameObject[] slots;
	[SerializeField]
	GameObject[] tileBackgrounds;
	[SerializeField]
	GameObject slotGlow;
	GameObject selectedSlot;
	public GameObject SelectedSlot { get { return selectedSlot; } }

	[SerializeField]
	ChimneyTileTemplate emptyTileTemplate;

	[SerializeField]
	RemoveInventoryItem itemRemover;

	[SerializeField]
	Text[] itemValues;
	[SerializeField]
	Sprite[] catagoryTileBackground;

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
			if (selectedSlot.GetComponent<InventorySlot>().BeingDragged && selectedSlot.GetComponent<InventorySlot>().MouseOver)
			{
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				selectedSlot.transform.position = new Vector3(mousePosition.x, mousePosition.y, selectedSlot.transform.position.z);
			}
			if (Input.GetMouseButtonUp(0))
			{
				//Debug.Log("Mouse up");
				selectedSlot.GetComponent<InventorySlot>().BeingDragged = false;
				selectedSlot.transform.position = new Vector3(selectedSlot.GetComponent<InventorySlot>().OriginalPosition.x, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.y, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.z);
			}
		}

		// If the item has made contact with the bin icon and the player has took there finger off the screen the item in that slot will be deleted
		if (itemRemover.RemoveItem)
		{
			if (Input.GetMouseButtonUp(0))
			{
				itemRemover.RemoveItem = false;
				selectedSlot.GetComponent<SpriteRenderer>().sprite = emptyTileTemplate.artwork;
				tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum] = emptyTileTemplate;
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
			//Debug.Log(i);
			if (tileStored[i].catagory == ChimneyTileTemplate.Catagory.EMPTY)
			{
				noEmptySlots = false;
				freeSpace = i;
			}
		}
		return !noEmptySlots;
	}

	public void AddItem(ChimneyTileTemplate newItem, int itemValue, Color color, int chimneyTileIndex)
	{
		tileStored[freeSpace] = newItem;
		slots[freeSpace].GetComponent<SpriteRenderer>().sprite = newItem.artwork;
		slots[freeSpace].GetComponent<InventorySlot>().ItemValue = itemValue;
		slots[freeSpace].GetComponent<InventorySlot>().ChimneyTileTemplateIndex = chimneyTileIndex;
		itemValues[freeSpace].text = itemValue.ToString();

		// Give them the correct backgrounds
		switch (tileStored[freeSpace].catagory)
		{
			case ChimneyTileTemplate.Catagory.ARMOUR:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[0];
				break;
			case ChimneyTileTemplate.Catagory.WEAPON:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[1];
				break;
			case ChimneyTileTemplate.Catagory.POTION:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[2];
				break;
			case ChimneyTileTemplate.Catagory.SKIPTOOL:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[3];
				break;
			case ChimneyTileTemplate.Catagory.ENEMY:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[4];
				break;
			case ChimneyTileTemplate.Catagory.BOMB:
				tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite = catagoryTileBackground[5];
				break;
			default:
				break;
		}

		tileBackgrounds[freeSpace].transform.position = new Vector3(slots[freeSpace].transform.position.x, slots[freeSpace].transform.position.y, slots[freeSpace].transform.position.z);
		slots[freeSpace].GetComponent<InventorySlot>().TileBackgroundSprite = tileBackgrounds[freeSpace].GetComponent<SpriteRenderer>().sprite;
	}

	public void UseItem(bool chimneyTile)
	{
		if (!chimneyTile)
		{

			// Change what the item does depending on it's catagory.
			switch (tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum].catagory)
			{
				case ChimneyTileTemplate.Catagory.ARMOUR:
					UsingArmourItem(chimneyTile);
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:
					UsingWeaponItem();
					break;
				case ChimneyTileTemplate.Catagory.POTION:
					UsingPotionItem(chimneyTile);
					break;
				case ChimneyTileTemplate.Catagory.SKIPTOOL:
					UsingSkipToolItem();
					break;
				case ChimneyTileTemplate.Catagory.ENEMY:
					break;
				case ChimneyTileTemplate.Catagory.BOMB:
					UsingBombItem();
					break;
				case ChimneyTileTemplate.Catagory.MONEY:
					break;
				case ChimneyTileTemplate.Catagory.EMPTY:
					break;
				default:
					break;
			}

			// Get rid of the item
			selectedSlot.GetComponent<SpriteRenderer>().sprite = emptyTileTemplate.artwork;
			tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum] = emptyTileTemplate;
			selectedSlot.GetComponent<InventorySlot>().Selected = false;
			selectedSlot.transform.position = new Vector3(selectedSlot.GetComponent<InventorySlot>().OriginalPosition.x, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.y, selectedSlot.GetComponent<InventorySlot>().OriginalPosition.z);
		}
		else
		{
			// Change what the item does depending on it's catagory.
			switch (tileManager.ChimneyTileTemplateArray[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory)
			{
				case ChimneyTileTemplate.Catagory.ARMOUR:
					UsingArmourItem(chimneyTile);
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:
					UsingWeaponItem();
					break;
				case ChimneyTileTemplate.Catagory.POTION:
					UsingPotionItem(chimneyTile);
					break;
				case ChimneyTileTemplate.Catagory.SKIPTOOL:
					UsingSkipToolItem();
					break;
				case ChimneyTileTemplate.Catagory.ENEMY:
					break;
				case ChimneyTileTemplate.Catagory.BOMB:
					UsingBombItem();
					break;
				case ChimneyTileTemplate.Catagory.MONEY:
					break;
				case ChimneyTileTemplate.Catagory.EMPTY:
					break;
				default:
					break;
			}
		}
	}

	void UsingArmourItem(bool chimneyTile)
	{
		Debug.Log("Armour used");

		if (!chimneyTile)
		{
			if (gm.HasArmour)
			{
				if (gm.MaxArmourHitPoints < slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue)
				{
					gm.HasArmour = true;
					gm.CurrentArmourHitPoints = slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
					gm.MaxArmourHitPoints = slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
				}
				else
				{
					gm.CurrentArmourHitPoints += slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
				}
			}
			else
			{
				gm.HasArmour = true;
				gm.CurrentArmourHitPoints = slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
				gm.MaxArmourHitPoints = slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
			}
		}
		else
		{
			if (gm.HasArmour)
			{
				if (gm.MaxArmourHitPoints < tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue)
				{
					gm.HasArmour = true;
					gm.CurrentArmourHitPoints = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
					gm.MaxArmourHitPoints = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
				}
				else
				{
					gm.CurrentArmourHitPoints += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
				}
			}
			else
			{
				gm.HasArmour = true;
				gm.CurrentArmourHitPoints = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
				gm.MaxArmourHitPoints = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
			}
		}
	}

	void UsingWeaponItem()
	{
		Debug.Log("Weapon used");

		// Take the items value away from the current tiles value
		tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue -= slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;

		if (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue < 0)
		{
			tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue = 0;
		}
	}

	void UsingPotionItem(bool chimneyTile)
	{
		if (!chimneyTile)
		{
			switch (tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory)
			{
				case ChimneyTileTemplate.PotionsSubCatagory.HEALTH:
					UsingHealthPotionItem(chimneyTile);
					break;
				case ChimneyTileTemplate.PotionsSubCatagory.POISON:
					UsingPoisonPotionItem();
					break;
				case ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE:
					UsingClairvoyancePotionItem(chimneyTile);
					break;
				default:
					break;
			}
		}
		else
		{
			switch (tileManager.ChimneyTileTemplateArray[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory)
			{
				case ChimneyTileTemplate.PotionsSubCatagory.HEALTH:
					UsingHealthPotionItem(chimneyTile);
					break;
				case ChimneyTileTemplate.PotionsSubCatagory.POISON:
					UsingPoisonPotionItem();
					break;
				case ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE:
					UsingClairvoyancePotionItem(chimneyTile);
					break;
				default:
					break;
			}
		}
	}

	void UsingSkipToolItem()
	{
		Debug.Log("SkipTool used");

		// Skip ahead the number of tiles equal to that of the value on the item
		tileManager.SkipTiles(slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue);
	}

	void UsingBombItem()
	{
		Debug.Log("Bomb used");

		// Half the value of the current tile
		tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue /= 2;
	}

	void UsingHealthPotionItem(bool chimneyTile)
	{
		Debug.Log("HealthPotion used");
		if (!chimneyTile)
		{
			gm.CurrentHitPoints += slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue;
		}
		else
		{
			gm.CurrentHitPoints += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
		}
	}

	void UsingPoisonPotionItem()
	{
		Debug.Log("PoisonPotion used");

		// Instantly kills an enemy
		tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue = 0;
	}

	void UsingClairvoyancePotionItem(bool chimneyTile)
	{
		Debug.Log("ClairvoyancePotion used");

		// Shows what the next tiles are (the number of tiles it shows depends on the item's value)
		if (!chimneyTile)
		{
			tileManager.ShowTiles(slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue);
			//tileManager.StartCoroutine("revealingTiles", slots[selectedSlot.GetComponent<InventorySlot>().SlotNum].GetComponent<InventorySlot>().ItemValue);
		}
		else
		{
			tileManager.ShowTiles(tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue);
			//tileManager.StartCoroutine("revealingTiles", tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue);
		}
	}

	public ChimneyTileTemplate.Catagory GetCurrentlySelectedItemCatagory()
	{
		return tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum].catagory;
	}

	public ChimneyTileTemplate.PotionsSubCatagory GetCurrentlySelectedItemPotionCatagory()
	{
		return tileStored[selectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory;
	}
}
