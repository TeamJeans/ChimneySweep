using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChimneyTile", menuName = "ChimneyTile")]
public class ChimneyTileTemplate : ScriptableObject
{
	public string tileName;
	public string description;
	public Sprite artwork;
	public int minTileValue;
	public int maxTileValue;
	public float spawnPercentage = 10.0f;
	public GameObject[] particleEffects;
	public float spawnPercentageIncreasePerWeek = 0.0f;

	public enum Catagory
	{
		ARMOUR,
		WEAPON,
		POTION,
		SKIPTOOL,
		ENEMY,
		BOMB,
		MONEY,
		EMPTY,
		SHOP_TILE
	};
	public enum PotionsSubCatagory
	{
		HEALTH,
		POISON,
		CLAIRVOYANCE
	}
	public enum EnemySubCatagory
	{
		NORMAL,
		BOSS
	}

	public Catagory catagory;
	public PotionsSubCatagory potionSubCatagory;
	public EnemySubCatagory enemySubCatagory;

	bool storable = true;
	public bool Storable { get { return storable; } set { storable = value; } }
}
