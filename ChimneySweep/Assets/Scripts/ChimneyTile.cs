using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChimneyTile : MonoBehaviour
{
	TileManager tileManager;

	string catagoryName;
	public string CatagoryName { get { return catagoryName; } set { catagoryName = value; } }
	string catagoryDescrition;
	public string CatagoryDescription { get { return catagoryDescrition; } set { catagoryDescrition = value; } }

	[SerializeField]
	bool selected = false;
	public bool Selected { get { return selected; } set { selected = value; } }

	int randomTileTypeNum;
	public int RandomTileTypeNum { get { return randomTileTypeNum; } set { randomTileTypeNum = value; } }

	bool mouseOver = false;
	public bool MouseOver { get { return mouseOver; } }

	bool tileUsed = false;
	public bool TileUsed { get { return tileUsed; } set { tileUsed = value; } }

	int tileValue;
	public int TileValue { get { return tileValue; } set { tileValue = value; } }

	int constTileValue;
	public int ConstTileValue { get { return constTileValue; } }

	void Start()
	{
		// Find the tile manager
		if (tileManager == null)
		{
			tileManager = GameObject.FindGameObjectWithTag("TileManager").GetComponent(typeof(TileManager)) as TileManager;
		}

		constTileValue = tileValue;
	}

	void OnMouseOver()
	{
		// Check if the user has tapped
		if (Input.GetMouseButtonDown(0) && tileManager.CurrentlySelectedTile == gameObject)
		{
			selected = true;
		}
		if (!mouseOver)
		{
			mouseOver = true;
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !mouseOver)
		{
			selected = false;
		}


		// Set the tile to have the information it needs
		if (selected)
		{
			// Display the artwork for the selected tile
			GetComponent<SpriteRenderer>().sprite = tileManager.ChimneyTileTemplateArray[randomTileTypeNum].artwork;
		}
		else if (tileUsed)
		{
			// Make a transparent black square appear over the used tile
			
		}
		else if (!tileManager.CurrentlySelectedTile == gameObject)
		{
			// Set the art work to show the back of the tile
			GetComponent<SpriteRenderer>().sprite = tileManager.ChimneyTileTemplateArray[0].artwork;
		}
	}

	void OnMouseExit()
	{
		if (mouseOver)
		{
			mouseOver = false;
		}
	}
}
