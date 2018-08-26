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
	string catagoryName;
	public string CatagoryName { get { return catagoryName; } set { catagoryName = value; } }
	string catagoryDescrition;
	public string CatagoryDescription { get { return catagoryDescrition; } set { catagoryDescrition = value; } }

	public enum Catagory
	{
		ARMOUR,
		WEAPON,
		POTION,
		SKIPTOOL,
		ENEMY,
		BOMB,
		MONEY,
		EMPTY
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
