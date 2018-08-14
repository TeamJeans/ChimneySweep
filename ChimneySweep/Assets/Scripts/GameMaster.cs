using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	InventoryItemTrigger inventoryItemTrigger;

	[SerializeField]
	TileSwipeLeft tileSwipeLeft;

	[SerializeField]
	TileSwipeRight tileSwipeRight;

	[SerializeField]
	TileManager tileManager;

	[SerializeField]
	GameObject endDayMenu;

	[SerializeField]
	CameraControl cameraControl;

	[SerializeField]
	Inventory inventory;

	[SerializeField]
	RemoveInventoryItem removeInventoryItem;

	// UI related variables
	[SerializeField]
	int maxHitPoints = 10;
	[SerializeField]
	int currentHitPoints = 10;
	public int CurrentHitPoints { get { return currentHitPoints; } set { currentHitPoints = value; } }
	[SerializeField]
	int currentMoney = 0;

	bool hasArmour = false;
	int armourRating;

	[SerializeField]
	Text moneyText;
	[SerializeField]
	Text hitPointsText;


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
		moneyText.text = "Money: \u00A3" + currentMoney;
		hitPointsText.text = "HP: " + currentHitPoints + "/" + maxHitPoints;

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
			}
		}
	}

	void RightSwipingHandler()
	{
		if (tileSwipeRight.CollisionWithTile)
		{
			tileSwipeLeft.CollisionWithEnemyTile = false;
			if (Input.GetMouseButtonUp(0))
			{
				if (tileManager.tileObjects[tileManager.CurrentTileNumber + 1] != null)
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable)
					{
						inventory.AddItem(tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum], tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue, tileManager.tileValueText.color);
					}

					// If the tile was an enemy find out it's value and take it away from the player's hit points
					if (tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
					{
						currentHitPoints -= tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().TileValue;
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
				tileSwipeRight.CollisionWithTile = false;
			}
		}
	}

	void CheckIfUsingItem()
	{
		if (inventoryItemTrigger.UseItem && !removeInventoryItem.RemoveItem && Input.GetMouseButtonUp(0))
		{
			inventoryItemTrigger.UseItem = false;
			inventory.UseItem();
		}
	}
}
