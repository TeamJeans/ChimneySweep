using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCatagoryVariables : MonoBehaviour {

	public float[] percentages;

	public float armourPercentage = 10;
	public float weaponPercentage = 10;
	public float potionPercentage = 10;
	public float enemyPercentage = 10;
	public float skipToolPercentage = 10;
	public float bombPrecentage = 10;
	public float moneyPercentage = 0;

	float sumOfPercentages = 0.0f;
	public float SumOfPercentages{ get { return sumOfPercentages; } }

	void Awake()
	{
		for (int i = 0; i < percentages.Length; i++)
		{
			switch ((ChimneyTileTemplate.Catagory)i)
			{
				case ChimneyTileTemplate.Catagory.ARMOUR:
					percentages[i] = armourPercentage;
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:
					percentages[i] = weaponPercentage;
					break;
				case ChimneyTileTemplate.Catagory.POTION:
					percentages[i] = potionPercentage;
					break;
				case ChimneyTileTemplate.Catagory.SKIPTOOL:
					percentages[i] = skipToolPercentage;
					break;
				case ChimneyTileTemplate.Catagory.ENEMY:
					percentages[i] = enemyPercentage;
					break;
				case ChimneyTileTemplate.Catagory.BOMB:
					percentages[i] = bombPrecentage;
					break;
				case ChimneyTileTemplate.Catagory.MONEY:
					percentages[i] = moneyPercentage;
					break;
				case ChimneyTileTemplate.Catagory.EMPTY:
					percentages[i] = 0;
					break;
				default:
					break;
			}
			sumOfPercentages += percentages[i];
		}
	}
}
