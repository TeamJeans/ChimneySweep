using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveInventoryItem : MonoBehaviour {

	bool removeItem = false;
	public bool RemoveItem { get { return removeItem; }set { removeItem = value; } }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "InventoryItem")
		{
			Debug.Log("Removed Item");
			removeItem = true;
		}
	}

	void OnTriggerExit2D()
	{
		removeItem = false;
	}
}
