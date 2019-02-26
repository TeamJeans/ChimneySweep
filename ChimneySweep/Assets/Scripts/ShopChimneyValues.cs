using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopChimneyValues {

	// Shop chimney tiles
	static int noOfShopChimneyVisits = 0;
	static int currentTileNumber = 0;
	static int numOfLanterns = 0;
	static bool resurrectionActive = false;
	static int maxHitPoints = 10;
	static int currentHitPoints = maxHitPoints;
	static int maxArmourHitPoints = 0;
	static int currentArmoutHitPoints = 0;
	static int[] randomlyGeneratedTileIndex;
	static int[] randomlyGeneratedTileValue;

	public static int NoOfShopChimneyVisits
	{
		get { return noOfShopChimneyVisits; }
		set { noOfShopChimneyVisits = value; }
	}

	public static int CurrentTilenumber
	{
		get { return currentTileNumber; }
		set { currentTileNumber = value; }
	}

	public static int NumOfLanterns
	{
		get { return numOfLanterns; }
		set { numOfLanterns = value; }
	}

	public static bool ResurrectionActive
	{
		get { return resurrectionActive; }
		set { resurrectionActive = value; }
	}

	public static int CurrentHitPoints
	{
		get { return currentHitPoints; }
		set { currentHitPoints = value; }
	}

	public static int MaxHitPoints
	{
		get { return maxHitPoints; }
		set { maxHitPoints = value; }
	}

	public static int CurrentArmoutHitPoints
	{
		get { return currentArmoutHitPoints; }
		set { currentArmoutHitPoints = value; }
	}

	public static int MaxArmourHitPoints
	{
		get { return maxArmourHitPoints; }
		set { maxArmourHitPoints = value; }
	}

	public static int[] RandomlyGeneratedTileIndex
	{
		get { return randomlyGeneratedTileIndex; }
		set { randomlyGeneratedTileIndex = value; }
	}

	public static int[] RandomlyGeneratedTileValue
	{
		get { return randomlyGeneratedTileValue; }
		set { randomlyGeneratedTileValue = value; }
	}


	// Inventory
	static int[] inventoryItemIndex;
	static int[] inventoryItemValue;
	static ChimneyTileTemplate[] inventoryTilesStored;
	static GameObject[] inventoryTileBackgrounds;
	static bool newShopGenerated = false;
	static bool inventoryEmpty = true;

	public static int[] InventoryItemIndex
	{
		get { return inventoryItemIndex; }
		set { inventoryItemIndex = value; }
	}

	public static int[] InventoryItemValue
	{
		get { return inventoryItemValue; }
		set { inventoryItemValue = value; }
	}

	public static ChimneyTileTemplate[] InventoryTilesStored
	{
		get { return inventoryTilesStored; }
		set { inventoryTilesStored = value; }
	}

	public static GameObject[] InventoryTileBackgrounds
	{
		get { return inventoryTileBackgrounds; }
		set { inventoryTileBackgrounds = value; }
	}

	public static bool NewShopGenerated
	{
		get { return newShopGenerated; }
		set { newShopGenerated = value; }
	}

	public static bool InventoryEmpty
	{
		get { return inventoryEmpty; }
		set { inventoryEmpty = value; }
	}

}
