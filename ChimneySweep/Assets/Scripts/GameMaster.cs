using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	public CanvasScaler chimneyCanvasScaler;

	[SerializeField]
	TileSwipeLeft tileSwipeLeft;
	[SerializeField]
	TileSwipeRight tileSwipeRight;
	[SerializeField]
	TileManager tileManager;
	[SerializeField]
	GameObject endDayMenu;
	[SerializeField]
	GameObject gameOverMenu;
	[SerializeField]
	CameraControl cameraControl;
	[SerializeField]
	Inventory inventory;
	[SerializeField]
	InventoryItemTrigger inventoryItemTrigger;
	[SerializeField]
	RemoveInventoryItem removeInventoryItem;

	// UI related variables
	[SerializeField]
	GameObject sellTextBackground;
	[SerializeField]
	GameObject leaveChimneyTextBackground;
	[SerializeField]
	GameObject sellRightTextBackground;
	[SerializeField]
	GameObject useTextBackground;
	[SerializeField]
	GameObject fightTextBackground;
	[SerializeField]
	GameObject storeTextBackground;

	[SerializeField]
	int maxHitPoints = 10;
	[SerializeField]
	int currentHitPoints = 10;
	public int CurrentHitPoints { get { return currentHitPoints; } set { currentHitPoints = value; } }

	[SerializeField]
	int maxArmourHitPoints = 0;
	public int MaxArmourHitPoints { get { return maxArmourHitPoints; } set { maxArmourHitPoints = value; } }
	[SerializeField]
	int currentArmourHitPoints = 0;
	public int CurrentArmourHitPoints { get { return currentArmourHitPoints; } set { currentArmourHitPoints = value; } }

	[SerializeField]
	int currentMoney = 0;

	[SerializeField]
	bool hasArmour = false;
	public bool HasArmour { get { return hasArmour; } set { hasArmour = value; } }

	[SerializeField]
	Text moneyText;
	[SerializeField]
	Color moneyTextChangeColour;
	[SerializeField]
	Text hitPointsText;
	[SerializeField]
	Color hitPointsTextChangeColour;
	[SerializeField]
	Text armourHitPointsText;
	[SerializeField]
	Color armourHitPointsTextChangeColour;

	[SerializeField]
	GameObject armourIconObject;

	void Awake()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}
	}

	public delegate void EndDayMenuCallBack(bool active);
	public EndDayMenuCallBack onToggleEndDayMenu;
	
	// Update is called once per frame
	void Update ()
	{
		//Update the UI
		moneyText.text = "\u00A3" + currentMoney;
		hitPointsText.text = currentHitPoints + "/" + maxHitPoints;
		armourHitPointsText.text = currentArmourHitPoints + "/" + maxArmourHitPoints;

		if (currentHitPoints > maxHitPoints)
		{
			currentHitPoints = maxHitPoints;
		}

		if (tileSwipeLeft.CollisionWithEnemyTile && !endDayMenu.activeSelf)
		{
			tileSwipeRight.CollisionWithTile = false;
			if (Input.GetMouseButtonUp(0))
			{
				ToggleEndDayMenu();
				tileSwipeLeft.CollisionWithEnemyTile = false;
			}
		}

		// Handles swiping tiles to the left
		LeftSwipingHandler();

		// Handles swiping tiles to the right
		RightSwipingHandler();

		// Check if an item is being used
		CheckIfUsingItem();

		// Deal with armour ui
		UpdateArmourUI();

		// Activate gameovermenu if hit points are at 0
		if (currentHitPoints <= 0 && gameOverMenu.activeSelf != true)
		{
			gameOverMenu.SetActive(!gameOverMenu.activeSelf);
			onToggleEndDayMenu.Invoke(gameOverMenu.activeSelf);
		}
	}

	public void ToggleEndDayMenu()
	{
		endDayMenu.SetActive(!endDayMenu.activeSelf);
		onToggleEndDayMenu.Invoke(endDayMenu.activeSelf);
	}

	public void ChangeSceneToDayOverStats()
	{
		SceneManager.LoadScene("DayOverStatsScene");
	}

	public void ChangeSceneToCalendar()
	{
		SceneManager.LoadScene("CalendarScene");
	}

	public void ChangeSceneToMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	void LeftSwipingHandler()
	{
		if (tileSwipeLeft.CollisionWithStorableTile && !endDayMenu.activeSelf)
		{
			tileSwipeRight.CollisionWithTile = false;
			if (Input.GetMouseButtonUp(0))
			{
				// Give the player a sum of money equal to that of the tile value
				currentMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;

				// Move to the next tile in the queue
				tileManager.MoveToNextTile();

				// Change where the camera goes to when the next tile is selected
				cameraControl.SetDesiredCamPos();

				tileSwipeLeft.CollisionWithStorableTile = false;

				// Change UI elements back
				moneyText.text = "\u00A3" + currentMoney;
				sellTextBackground.SetActive(false);
				moneyText.color = Color.white;
			}
			else
			{
				// Display what the new values will be
				// Display what swiping it to the left does (Sell or Leave chimney)
				sellTextBackground.SetActive(true);
				// Change money text to gold colour and change the value to how much it would be if the player did this
				int tempMoney = currentMoney + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
				moneyText.text = "\u00A3" + tempMoney;
				moneyText.color = moneyTextChangeColour;
			}
		}
		else if (tileSwipeLeft.CollisionWithEnemyTile && !endDayMenu.activeSelf)
		{
			if (Input.GetMouseButtonUp(0))
			{
				// Change UI elements back
				leaveChimneyTextBackground.SetActive(false);
			}
			// Display what the new values will be
			// Display what swiping it to the left does (Sell or Leave chimney)
			leaveChimneyTextBackground.SetActive(true);
		}
		else if (!tileSwipeLeft.CollisionWithStorableTile && !tileSwipeLeft.CollisionWithEnemyTile && !tileSwipeRight.CollisionWithTile && !inventoryItemTrigger.UseItem)
		{
			// Change UI elements back
			sellTextBackground.SetActive(false);
			leaveChimneyTextBackground.SetActive(false);
			moneyText.color = Color.white;
		}
	}

	void RightSwipingHandler()
	{
		if (tileSwipeRight.CollisionWithTile)
		{
			tileSwipeLeft.CollisionWithEnemyTile = false;
			if (Input.GetMouseButtonUp(0))
			{
				Debug.Log("Current TileNumber: " + tileManager.CurrentTileNumber);
				Debug.Log("No of Tiles: " + tileManager.tileObjects.Length);
				if (tileManager.CurrentTileNumber + 1 != tileManager.tileObjects.Length)
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable)
					{
						inventory.AddItem(tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum], tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue, tileManager.currentTileValueText.color);
					}
					else if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ARMOUR || (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.POTION  && (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE || tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)))
					{
						inventory.UseItem(true);
					}
					else if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory != ChimneyTileTemplate.Catagory.ENEMY)
					{
						currentMoney += tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
					}

					// If the tile was an enemy find out it's value and take it away from the player's hit points
					if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
					{
						if (currentArmourHitPoints <= 0)
						{
							currentHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
						}
						else if (currentArmourHitPoints >= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue)
						{
							currentArmourHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
						}
						else if (currentArmourHitPoints < tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue)
						{
							int remainder = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue - currentArmourHitPoints;
							currentArmourHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
							currentHitPoints -= remainder;
						}

						if (currentHitPoints < 0)
						{
							currentHitPoints = 0;
						}

						currentMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;

					}

					// Move to the next tile in the queue
					tileManager.MoveToNextTile();

					// Change where the camera goes to when the next tile is selected
					cameraControl.SetDesiredCamPos();
				}
				else
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable)
					{
						inventory.AddItem(tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum], tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue, tileManager.currentTileValueText.color);
					}
					else if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ARMOUR || (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.POTION && (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE || tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)))
					{
						inventory.UseItem(true);
					}
					else if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory != ChimneyTileTemplate.Catagory.ENEMY)
					{
						currentMoney += tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
					}

					// If the tile was an enemy find out it's value and take it away from the player's hit points
					if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
					{
						if (currentArmourHitPoints <= 0)
						{
							currentHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
						}
						else if (currentArmourHitPoints >= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue)
						{
							currentArmourHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
						}
						else if (currentArmourHitPoints < tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue)
						{
							int remainder = tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue - currentArmourHitPoints;
							currentArmourHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
							currentHitPoints -= remainder;
						}

						if (currentHitPoints < 0)
						{
							currentHitPoints = 0;
						}

						currentMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;

					}

					// Move to the day stats scene
					ChangeSceneToDayOverStats();
				}
				tileSwipeRight.CollisionWithTile = false;
			}
			else
			{
				if (inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable)
				{
					// Show the "Store" text on the ui
					storeTextBackground.SetActive(true);
				}
				else if (!inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory != ChimneyTileTemplate.Catagory.ARMOUR && (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory != ChimneyTileTemplate.Catagory.POTION))
				{
					sellRightTextBackground.SetActive(true);
					moneyText.color = moneyTextChangeColour;
					int tempMoney = currentMoney + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
					moneyText.text = "\u00A3" + tempMoney;
				}
				else
				{
					switch (tileManager.chimneyTileTemplate[tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory)
					{
						case ChimneyTileTemplate.Catagory.ENEMY:
							fightTextBackground.SetActive(true);
							if (!hasArmour)
							{
								int tempHitPoints = currentHitPoints - tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								if (tempHitPoints < 0)
									tempHitPoints = 0;
								hitPointsText.color = hitPointsTextChangeColour;
								hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
							}
							else
							{
								if (tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue <= currentArmourHitPoints)
								{
									int tempArmourHitPoints = currentArmourHitPoints - tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
									if (tempArmourHitPoints < 0)
										tempArmourHitPoints = 0;
									armourHitPointsText.color = armourHitPointsTextChangeColour;
									armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
								}
								else
								{
									int tempArmourHitPoints = currentArmourHitPoints - tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
									if (tempArmourHitPoints < 0)
										tempArmourHitPoints = 0;
									armourHitPointsText.color = armourHitPointsTextChangeColour;
									armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
									int tempHitPoints = currentHitPoints - (tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue - currentArmourHitPoints);
									if (tempHitPoints < 0)
										tempHitPoints = 0;
									hitPointsText.color = hitPointsTextChangeColour;
									hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
								}
							}
							break;
						case ChimneyTileTemplate.Catagory.POTION:
							useTextBackground.SetActive(true);
							if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)
							{
								int tempHitPoints = currentHitPoints + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								if (tempHitPoints > maxHitPoints)
									tempHitPoints = maxHitPoints;
								hitPointsText.color = hitPointsTextChangeColour;
								hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
							}
							else if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE)
							{
								// If we want to do anything for clairvoyance potions, add the code here
							}
							else
							{
								useTextBackground.SetActive(false);
								sellRightTextBackground.SetActive(true);
								moneyText.color = moneyTextChangeColour;
								int tempMoney = currentMoney + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								moneyText.text = "\u00A3" + tempMoney;
							}
						break;
						case ChimneyTileTemplate.Catagory.ARMOUR:
							useTextBackground.SetActive(true);
							armourIconObject.SetActive(true);
							armourHitPointsText.gameObject.SetActive(true);
							if (tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue >= maxArmourHitPoints)
							{
								armourHitPointsText.color = armourHitPointsTextChangeColour;
								armourHitPointsText.text = tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue + "/" + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
							}
							else
							{
								int tempArmourHitPoints = currentArmourHitPoints + tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								if (tempArmourHitPoints > maxArmourHitPoints)
									tempArmourHitPoints = maxArmourHitPoints;
								armourHitPointsText.color = armourHitPointsTextChangeColour;
								armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
							}
							break;
						default:
						break;
					}
				}
			}
		}
		else if (!tileSwipeRight.CollisionWithTile && !tileSwipeLeft.CollisionWithStorableTile && !inventoryItemTrigger.UseItem)
		{
			// Change UI elements back
			sellRightTextBackground.SetActive(false);
			useTextBackground.SetActive(false);
			fightTextBackground.SetActive(false);
			storeTextBackground.SetActive(false);
			moneyText.color = Color.white;
			moneyText.text = "\u00A3" + currentMoney;
			hitPointsText.color = Color.white;
			hitPointsText.text = currentHitPoints + "/" + maxHitPoints;
			armourHitPointsText.color = Color.white;
			armourHitPointsText.text = currentArmourHitPoints + "/" + maxArmourHitPoints;
			if (!hasArmour)
			{
				armourIconObject.SetActive(false);
				armourHitPointsText.gameObject.SetActive(false);
			}
		}
	}

	void CheckIfUsingItem()
	{
		if (inventoryItemTrigger.UseItem && !removeInventoryItem.RemoveItem && Input.GetMouseButtonUp(0))
		{
			if ((inventory.GetCurrentlySelectedItemCatagory() == ChimneyTileTemplate.Catagory.BOMB || inventory.GetCurrentlySelectedItemCatagory() == ChimneyTileTemplate.Catagory.WEAPON || inventory.GetCurrentlySelectedItemPotionCatagory() == ChimneyTileTemplate.PotionsSubCatagory.POISON) && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory != ChimneyTileTemplate.Catagory.ENEMY)
			{
				// Don't use the item
			}
			else
			{
				inventoryItemTrigger.UseItem = false;
				inventory.UseItem(false);
			}
		}
	}

	void UpdateArmourUI()
	{
		if (hasArmour)
		{
			armourIconObject.SetActive(true);
			armourHitPointsText.gameObject.SetActive(true);
		}
		else
		{
			if (!tileSwipeRight.CollisionWithTile && !inventoryItemTrigger.UseItem)
			{
				armourIconObject.SetActive(false);
				armourHitPointsText.gameObject.SetActive(false);
			}
		}

		if (currentArmourHitPoints <= 0)
		{
			hasArmour = false;
			currentArmourHitPoints = 0;
			maxArmourHitPoints = 0;
		}

		if (currentArmourHitPoints > maxArmourHitPoints)
		{
			currentArmourHitPoints = maxArmourHitPoints;
		}

		// Show stats changing when using items from inentory
		if (inventoryItemTrigger.UseItem)
		{
			switch (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].catagory)
			{
				case ChimneyTileTemplate.Catagory.POTION:
					if (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)
					{
						int tempHitPoints = currentHitPoints + inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue;
						if (tempHitPoints > maxHitPoints)
							tempHitPoints = maxHitPoints;
						hitPointsText.color = hitPointsTextChangeColour;
						hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
					}
					else if (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE)
					{
						// If we want to do anything for clairvoyance potions, add the code here
					}
					else if (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.POISON)
					{
						// If the current tile is an enemy, show how the enemy's value is affected
					}
					break;
				case ChimneyTileTemplate.Catagory.ARMOUR:
					armourIconObject.SetActive(true);
					armourHitPointsText.gameObject.SetActive(true);
					if (inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue >= maxArmourHitPoints)
					{
						armourHitPointsText.color = armourHitPointsTextChangeColour;
						armourHitPointsText.text = inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue + "/" + inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue;
					}
					else
					{
						int tempArmourHitPoints = currentArmourHitPoints + inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue;
						if (tempArmourHitPoints > maxArmourHitPoints)
							tempArmourHitPoints = maxArmourHitPoints;
						armourHitPointsText.color = armourHitPointsTextChangeColour;
						armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
					}
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:

					break;
				default:
					break;
			}
		}
		else if (!tileSwipeRight.CollisionWithTile && !tileSwipeLeft.CollisionWithStorableTile && !inventoryItemTrigger.UseItem)
		{
			// Change UI elements back
			moneyText.color = Color.white;
			moneyText.text = "\u00A3" + currentMoney;
			hitPointsText.color = Color.white;
			hitPointsText.text = currentHitPoints + "/" + maxHitPoints;
			armourHitPointsText.color = Color.white;
			armourHitPointsText.text = currentArmourHitPoints + "/" + maxArmourHitPoints;
			if (!hasArmour)
			{
				armourIconObject.SetActive(false);
				armourHitPointsText.gameObject.SetActive(false);
			}
		}
	}
}
