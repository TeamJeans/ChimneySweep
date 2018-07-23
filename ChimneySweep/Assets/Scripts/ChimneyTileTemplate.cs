using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChimneyTile", menuName = "ChimneyTile")]
public class ChimneyTileTemplate : ScriptableObject
{
	public string tileName;
	public string description;
	public Sprite artwork;
	public enum Catagory
	{
		ARMOUR,
		WEAPON,
		POTION,
		SKIPTOOL,
		ENEMY,
		BOMB,
		SPELL,
		MONEY,
		EMPTY
	};
	public Catagory catagory;

	bool storable = true;
	public bool Storable { get { return storable; } set { storable = value; } }
}
