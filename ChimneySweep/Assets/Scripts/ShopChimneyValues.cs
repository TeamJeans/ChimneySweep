using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopChimneyValues{

	static int noOfShopChimneyVisits = 0;
	static int currentTileNumber = 0;

	public static int NoOfShopChimneyVisits
	{
		get { return noOfShopChimneyVisits; }
		set { noOfShopChimneyVisits = value; }
	}

	public static int CurrentTilenumber
	{
		get { return currentTileNumber;}
		set { currentTileNumber = value; }
	}
}
