using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSwipeLeft : MonoBehaviour {

	[SerializeField]
	bool collisionWithEnemyTile = false;
	public bool CollisionWithEnemyTile { get { return collisionWithEnemyTile; } set { collisionWithEnemyTile = value; } }

	[SerializeField]
	bool collisionWithStorableTile = false;
	public bool CollisionWithStorableTile { get { return collisionWithStorableTile; } set { collisionWithStorableTile = value; } }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "EnemyTile")
		{
			Debug.Log("Triggered");
			collisionWithEnemyTile = true;
		}

		if (col.gameObject.tag == "StorableTile")
		{
			Debug.Log("Triggered");
			collisionWithStorableTile = true;
		}
	}

	void OnTriggerExit2D()
	{
		collisionWithEnemyTile = false;
		collisionWithStorableTile = false;
	}
}
