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
	GameObject optionsMenu;
	[SerializeField]
	GameObject gameOverMenu;
	[SerializeField]
	GameObject tileDescriptionMenu;
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
	GameObject buyTextBackground;
	[SerializeField]
	GameObject discardTextBackground;
	[SerializeField]
	GameObject discardRightTextBackground;

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
	Text dayText;

	[SerializeField]
	GameObject heartIconObject;
	[SerializeField]
	Sprite heartNormalSprite;
	[SerializeField]
	Sprite heartBrokenSprite;

	bool resurrectionActive = false;
	public bool ResurrectionActive { get { return resurrectionActive; } set { resurrectionActive = value; } }

	[SerializeField]
	GameObject armourIconObject;
	[SerializeField]
	Sprite armourNormalSprite;
	[SerializeField]
	Sprite armourBrokenSprite;

	[SerializeField]
	GameObject resurrectionIconObject;

	ChimneyTileTemplate currentTileType;

	void Awake()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}

		// REMOVE THIS!!!
		StaticValueHolder.TotalMoney = 200;

		currentMoney = StaticValueHolder.TotalMoney;
		resurrectionActive = ShopChimneyValues.ResurrectionActive;

		maxHitPoints = ShopChimneyValues.MaxHitPoints;
		currentHitPoints = ShopChimneyValues.CurrentHitPoints;
		tileManager.NoOfLanterns = ShopChimneyValues.NumOfLanterns;
		maxArmourHitPoints = ShopChimneyValues.MaxArmourHitPoints;
		currentArmourHitPoints = ShopChimneyValues.CurrentArmoutHitPoints;

		if (currentArmourHitPoints > 0)
		{
			hasArmour = true;
		}
		
	}

	public delegate void EndDayMenuCallBack(bool active);
	public EndDayMenuCallBack onToggleEndDayMenu;
	
	void Start()
	{
		if (!tileManager.IsShopChimney)
		{
			dayText.text = "Day " + (StaticValueHolder.CurrentDay);
		}
		else
		{
			dayText.text = "Shop";
		}
		StaticValueHolder.DailyMoney = 0;

		// Set the text background objects to be disabled at first
		if (tileManager.IsShopChimney)
		{
			buyTextBackground.SetActive(false);
			discardTextBackground.SetActive(false);
		}
	}

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

		//// Make it so that you can't move the chimney about when the description screen is active
		//if (tileDescriptionMenu.activeSelf == true)
		//{
		//	onToggleEndDayMenu.Invoke(tileDescriptionMenu.activeSelf);
		//}

		// Set the current tile type
		currentTileType = tileManager.ChimneyTileTemplateArray[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum];

		// Passing the current money value between scenes
		StaticValueHolder.DailyMoney = currentMoney - StaticValueHolder.TotalMoney;

		if (resurrectionActive)
		{
			resurrectionIconObject.SetActive(true);
		}
		else
		{
			resurrectionIconObject.SetActive(false);
		}
	}

	public void ToggleEndDayMenu()
	{
		endDayMenu.SetActive(!endDayMenu.activeSelf);
		onToggleEndDayMenu.Invoke(endDayMenu.activeSelf);
	}

	public void ToggleOptionsMenu()
	{
		optionsMenu.SetActive(!optionsMenu.activeSelf);
		onToggleEndDayMenu.Invoke(optionsMenu.activeSelf);
	}

	public void endDayYesOption()
	{
		StaticValueHolder.HitPoints = currentHitPoints;
		StaticValueHolder.MaxHitPoints = maxHitPoints;
		ChangeSceneToDayOverStats();
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

	public void ChangeSceneToChimneyScene()
	{
		ShopChimneyValues.MaxArmourHitPoints = maxArmourHitPoints;
		ShopChimneyValues.CurrentArmoutHitPoints = currentArmourHitPoints;
		ShopChimneyValues.MaxHitPoints = maxHitPoints;
		ShopChimneyValues.CurrentHitPoints = currentHitPoints;
		ShopChimneyValues.NumOfLanterns = tileManager.NoOfLanterns;
		ShopChimneyValues.ResurrectionActive = resurrectionActive;
		inventory.SaveInventoryValues();
		StaticValueHolder.TotalMoney += StaticValueHolder.DailyMoney;
		SceneManager.LoadScene("ChimneyScene");
	}

	public void ChangeSceneToShopChimneyScene()
	{
		ShopChimneyValues.MaxArmourHitPoints = maxArmourHitPoints;
		ShopChimneyValues.CurrentArmoutHitPoints = currentArmourHitPoints;
		ShopChimneyValues.MaxHitPoints = maxHitPoints;
		ShopChimneyValues.CurrentHitPoints = currentHitPoints;
		ShopChimneyValues.NumOfLanterns = tileManager.NoOfLanterns;
		ShopChimneyValues.ResurrectionActive = resurrectionActive;
		inventory.SaveInventoryValues();
		StaticValueHolder.TotalMoney += StaticValueHolder.DailyMoney;
		SceneManager.LoadScene("ShopScene");
	}

	void LeftSwipingHandler()
	{
		if (tileSwipeLeft.CollisionWithStorableTile && !endDayMenu.activeSelf)
		{
			tileSwipeRight.CollisionWithTile = false;
			if (Input.GetMouseButtonUp(0) && !tileManager.IsShopChimney)
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
				//discardTextBackground.SetActive(false);
			}
			else if(Input.GetMouseButtonUp(0) && tileManager.IsShopChimney)
			{
				// Move to the next tile in the queue
				tileManager.MoveToNextTile();

				// Change where the camera goes to when the next tile is selected
				cameraControl.SetDesiredCamPos();

				tileSwipeLeft.CollisionWithStorableTile = false;
			}
			else if (!tileManager.IsShopChimney) 
			{
				// Display what the new values will be
				// Display what swiping it to the left does (Sell or Leave chimney)
				sellTextBackground.SetActive(true);
				// Change money text to gold colour and change the value to how much it would be if the player did this
				int tempMoney = currentMoney + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
				moneyText.text = "\u00A3" + tempMoney;
				moneyText.color = moneyTextChangeColour;
			}
			else if (tileManager.IsShopChimney)
			{
				discardTextBackground.SetActive(true);
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
			if (Input.GetMouseButtonUp(0) && !tileManager.IsShopChimney)
			{
				Debug.Log("Current TileNumber: " + tileManager.CurrentTileNumber);
				Debug.Log("No of Tiles: " + tileManager.TileObjects.Length);
				if (tileManager.CurrentTileNumber + 1 < tileManager.TileObjects.Length)
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && currentTileType.Storable)
					{
						inventory.AddItem(currentTileType, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum);
					}
					else if (currentTileType.catagory == ChimneyTileTemplate.Catagory.ARMOUR || (currentTileType.catagory == ChimneyTileTemplate.Catagory.POTION  && (currentTileType.potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE || currentTileType.potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)))
					{
						inventory.UseItem(true);
					}
					else if (currentTileType.catagory != ChimneyTileTemplate.Catagory.ENEMY)
					{
						currentMoney += tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
						StaticValueHolder.DailyMoney += tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
					}

					// If the tile was an enemy find out it's value and take it away from the player's hit points
					if (currentTileType.catagory == ChimneyTileTemplate.Catagory.ENEMY)
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
							if (resurrectionActive)
							{
								resurrectionActive = false;
								currentHitPoints = maxHitPoints / 2;
							}
							else
							{
								currentHitPoints = 0;
							}
						}

						currentMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
						StaticValueHolder.DailyMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
					}

					// Move to the next tile in the queue
					tileManager.MoveToNextTile();

					// Change where the camera goes to when the next tile is selected
					cameraControl.SetDesiredCamPos();
				}
				else
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && currentTileType.Storable)
					{
						inventory.AddItem(currentTileType, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum);
					}
					else if (currentTileType.catagory == ChimneyTileTemplate.Catagory.ARMOUR || (currentTileType.catagory == ChimneyTileTemplate.Catagory.POTION && (currentTileType.potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE || currentTileType.potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)))
					{
						inventory.UseItem(true);
					}
					else if (currentTileType.catagory != ChimneyTileTemplate.Catagory.ENEMY)
					{
						currentMoney += tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
						StaticValueHolder.DailyMoney += tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
					}

					// If the tile was an enemy find out it's value and take it away from the player's hit points
					if (currentTileType.catagory == ChimneyTileTemplate.Catagory.ENEMY)
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
							if (resurrectionActive)
							{
								resurrectionActive = false;
								currentHitPoints = maxHitPoints / 2;
							}
							else
							{
								currentHitPoints = 0;
							}
						}

						currentMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
						StaticValueHolder.DailyMoney += tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;

						Debug.Log("IT WORKS");

						if (currentTileType.catagory == ChimneyTileTemplate.Catagory.ENEMY && currentTileType.enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
						{
							// Move to the day stats scene
							ChangeSceneToDayOverStats();
						}

					}

					if (!tileManager.IsShopChimney)
					{
						// Move to the next tile in the queue
						tileManager.MoveToNextTile();
					}
					else
					{
						// Move to the day stats scene
						ChangeSceneToDayOverStats();
					}

				}
				tileSwipeRight.CollisionWithTile = false;
			}
			else
			{
				if (tileManager.IsShopChimney && Input.GetMouseButtonUp(0))
				{
					if (tileManager.CurrentTileNumber + 1 < tileManager.TileObjects.Length)
					{
						int tempMoney = currentMoney - tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;

						// If the inventory is not full and this item is storable, and the player has enough money to buy it, then add it to the inventory
						if (inventory.IsThereSpace() && currentTileType.Storable && tempMoney >= 0)
						{
							currentMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
							StaticValueHolder.DailyMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
							inventory.AddItem(currentTileType, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue, tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum);

							// Move to the next tile in the queue
							tileManager.MoveToNextTile();

							// Change where the camera goes to when the next tile is selected
							cameraControl.SetDesiredCamPos();
						}
						else
						{
							if (currentTileType.catagory == ChimneyTileTemplate.Catagory.SHOP_TILE)
							{
								if (currentTileType.tileName == "Resurrection")
								{
									resurrectionActive = true;
									currentMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
									StaticValueHolder.DailyMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
								}
								else if (currentTileType.tileName == "Lantern")
								{
									tileManager.NoOfLanterns++;
									currentMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
									StaticValueHolder.DailyMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
								}
								else if (currentTileType.tileName == "Health Needle")
								{
									maxHitPoints++;
									currentMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
									StaticValueHolder.DailyMoney -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().ConstTileValue;
								}
							}

							// Move to the next tile in the queue
							tileManager.MoveToNextTile();

							// Change where the camera goes to when the next tile is selected
							cameraControl.SetDesiredCamPos();
						}
					}
					else
					{
						// Move to the next tile in the queue
						tileManager.MoveToNextTile();
					}
				}
				else if (tileManager.IsShopChimney)
				{
					if (currentTileType.Storable && !inventory.IsThereSpace())
					{
						discardRightTextBackground.SetActive(true);
					}
					else
					{
						buyTextBackground.SetActive(true);
						moneyText.color = moneyTextChangeColour;
						int tempMoney = currentMoney - tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
						moneyText.text = "\u00A3" + tempMoney;
					}
				}
				else if (inventory.IsThereSpace() && currentTileType.Storable)
				{
					// Show the "Store" text on the ui
					storeTextBackground.SetActive(true);
				}
				else if (!inventory.IsThereSpace() && currentTileType.Storable && currentTileType.catagory != ChimneyTileTemplate.Catagory.ARMOUR && (currentTileType.catagory != ChimneyTileTemplate.Catagory.POTION))
				{
					sellRightTextBackground.SetActive(true);
					moneyText.color = moneyTextChangeColour;
					int tempMoney = currentMoney + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
					moneyText.text = "\u00A3" + tempMoney;
				}
				else
				{
					switch (tileManager.ChimneyTileTemplateArray[tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory)
					{
						case ChimneyTileTemplate.Catagory.ENEMY:
							fightTextBackground.SetActive(true);
							moneyText.color = moneyTextChangeColour;
							int tempEnemyMoney = currentMoney + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().ConstTileValue;
							moneyText.text = "\u00A3" + tempEnemyMoney;
							if (!hasArmour)
							{
								// Check if the enemy's value is 0, if so don't change the ui
								if (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue > 0)
								{

									int tempHitPoints = currentHitPoints - tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
									if (tempHitPoints < 0)
										tempHitPoints = 0;
									hitPointsText.color = hitPointsTextChangeColour;
									hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
									heartIconObject.GetComponent<Image>().sprite = heartBrokenSprite;
								}
							}
							else
							{
								// Check if the enemy's value is 0, if so don't change the ui
								if (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue > 0)
								{

									if (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue <= currentArmourHitPoints)
									{
										int tempArmourHitPoints = currentArmourHitPoints - tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
										if (tempArmourHitPoints < 0)
											tempArmourHitPoints = 0;
										armourHitPointsText.color = armourHitPointsTextChangeColour;
										armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
										armourIconObject.GetComponent<Image>().sprite = armourBrokenSprite;
									}
									else
									{
										int tempArmourHitPoints = currentArmourHitPoints - tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
										if (tempArmourHitPoints < 0)
											tempArmourHitPoints = 0;
										armourHitPointsText.color = armourHitPointsTextChangeColour;
										armourHitPointsText.text = tempArmourHitPoints + "/" + maxArmourHitPoints;
										int tempHitPoints = currentHitPoints - (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue - currentArmourHitPoints);
										if (tempHitPoints < 0)
											tempHitPoints = 0;
										hitPointsText.color = hitPointsTextChangeColour;
										hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
										heartIconObject.GetComponent<Image>().sprite = heartBrokenSprite;
										armourIconObject.GetComponent<Image>().sprite = armourBrokenSprite;
									}
								}
							}
							break;
						case ChimneyTileTemplate.Catagory.POTION:
							useTextBackground.SetActive(true);
							if (tileManager.ChimneyTileTemplateArray[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)
							{
								int tempHitPoints = currentHitPoints + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								if (tempHitPoints > maxHitPoints)
									tempHitPoints = maxHitPoints;
								hitPointsText.color = Color.green;
								hitPointsText.text = tempHitPoints + "/" + maxHitPoints;
							}
							else if (tileManager.ChimneyTileTemplateArray[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.CLAIRVOYANCE)
							{
								// If we want to do anything for clairvoyance potions, add the code here
							}
							else
							{
								useTextBackground.SetActive(false);
								sellRightTextBackground.SetActive(true);
								moneyText.color = moneyTextChangeColour;
								int tempMoney = currentMoney + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
								moneyText.text = "\u00A3" + tempMoney;
							}
						break;
						case ChimneyTileTemplate.Catagory.ARMOUR:
							useTextBackground.SetActive(true);
							armourIconObject.SetActive(true);
							armourHitPointsText.gameObject.SetActive(true);
							if (tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue >= maxArmourHitPoints)
							{
								armourHitPointsText.color = armourHitPointsTextChangeColour;
								armourHitPointsText.text = tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue + "/" + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
							}
							else
							{
								int tempArmourHitPoints = currentArmourHitPoints + tileManager.TileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileValue;
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
			heartIconObject.GetComponent<Image>().sprite = heartNormalSprite;
			armourIconObject.GetComponent<Image>().sprite = armourNormalSprite;
			sellRightTextBackground.SetActive(false);
			useTextBackground.SetActive(false);
			fightTextBackground.SetActive(false);
			storeTextBackground.SetActive(false);
			if (tileManager.IsShopChimney)
			{
				buyTextBackground.SetActive(false);
				discardTextBackground.SetActive(false);
				discardRightTextBackground.SetActive(false);
			}
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
			if ((inventory.GetCurrentlySelectedItemCatagory() == ChimneyTileTemplate.Catagory.BOMB || inventory.GetCurrentlySelectedItemCatagory() == ChimneyTileTemplate.Catagory.WEAPON || inventory.GetCurrentlySelectedItemPotionCatagory() == ChimneyTileTemplate.PotionsSubCatagory.POISON) && currentTileType.catagory != ChimneyTileTemplate.Catagory.ENEMY && !tileManager.IsShopChimney)
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

		// Show stats changing when using items from inventory
		if (inventoryItemTrigger.UseItem && !tileManager.IsShopChimney)
		{
			switch (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].catagory)
			{
				case ChimneyTileTemplate.Catagory.POTION:
					if (inventory.TileStored[inventory.SelectedSlot.GetComponent<InventorySlot>().SlotNum].potionSubCatagory == ChimneyTileTemplate.PotionsSubCatagory.HEALTH)
					{
						int tempHitPoints = currentHitPoints + inventory.SelectedSlot.GetComponent<InventorySlot>().ItemValue;
						if (tempHitPoints > maxHitPoints)
							tempHitPoints = maxHitPoints;
						hitPointsText.color = Color.green;
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

	public void SellingInventoryItem(int itemValue)
	{
		currentMoney += itemValue;
		StaticValueHolder.DailyMoney += itemValue;
	}
}
