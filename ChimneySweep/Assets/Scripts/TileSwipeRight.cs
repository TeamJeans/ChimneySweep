using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwipeRight : MonoBehaviour {

	bool collisionWithTile = false;
	public bool CollisionWithTile { get { return collisionWithTile; } set { collisionWithTile = value; } }

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log("Triggered");
		collisionWithTile = true;
	}
}
