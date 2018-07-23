using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwipeLeft : MonoBehaviour {

	bool collisionWithTile = false;
	public bool CollisionWithTile { get { return collisionWithTile; } set { collisionWithTile = value; } }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag != "InventoryItem")
		{
			Debug.Log("Triggered");
			collisionWithTile = true;
		}
	}
}
