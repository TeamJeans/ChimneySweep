using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemTrigger : MonoBehaviour {

	[SerializeField]
	bool useItem = false;
	public bool UseItem { get { return useItem; } set { useItem = value; } }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "InventoryItem")
		{
			useItem = true;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "InventoryItem")
		{
			useItem = false;
		}
	}
}
