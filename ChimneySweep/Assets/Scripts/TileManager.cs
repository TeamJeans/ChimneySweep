using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;
	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	ScrollControl scrollControl;

	// Colours for tile values
	[SerializeField]
	Color ArmourColour;
	[SerializeField]
	Color WeaponColour;
	[SerializeField]
	Color PotionColour;
	[SerializeField]
	Color SkipToolColour;
	[SerializeField]
	Color EnemyColour;
	[SerializeField]
	Color BombColour;
	[SerializeField]
	Color MoneyColour;

	Vector2 mousePosition;
	ChimneyTileTemplate.Catagory currentTileCatagory;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; } set { tileDragMode = value; } }
	bool tilesGenerated = false;
	bool endDayMenuEnabled = false;
	bool tileSelected = false;

	public GameObject[] tilePrefabs;
	public GameObject[] tileObjects;
	public ChimneyTileTemplate[] chimneyTileTemplate;
	public Text tileValueText;

	[SerializeField]
	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } set { currentlySelectedTile = value; } }
	[SerializeField]
	int currentTileNumber = 0;
	public int CurrentTileNumber{ get { return currentTileNumber; } }

	[SerializeField]
	GameObject tileGlow;
	[SerializeField]
	GameObject tileBackground;
	[SerializeField]
	GameObject tileUsed;
	[SerializeField]
	GameObject tileValuesObject;

	// Use this for initialization
	void Start ()
	{

		// Toggles control of the tiles when the end day menu is open
		GameMaster.gm.onToggleEndDayMenu += OnEndDayMenuToggle;

		tileObjects = new GameObject[tilePrefabs.Length];
		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			// Creates the array of blank tiles
			tileObjects[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tileObjects[i].transform.parent = gameObject.transform;

			// Randomly changes the type of each tile generated
			tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = Random.Range(2, chimneyTileTemplate.Length);
			// Find the min and max values for the current chimney tile template and generate a random number and set it to be the tiles value
			tileObjects[i].GetComponent<ChimneyTile>().TileValue = Random.Range(chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].minTileValue, chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].maxTileValue);
		}
		// Sets the selected tile to be the first tile generated
		tileObjects[0].GetComponent<ChimneyTile>().Selected = true;
		currentlySelectedTile = tileObjects[0];

		for (int i = 0; i < chimneyTileTemplate.Length; i++)
		{
			// Make it so that you can't put enemies or money items in the inventory
			if (chimneyTileTemplate[i].catagory == ChimneyTileTemplate.Catagory.ENEMY || chimneyTileTemplate[i].catagory == ChimneyTileTemplate.Catagory.MONEY)
			{
				chimneyTileTemplate[i].Storable = false;
			}
		}

		// Give the tiles tags
		for (int i = 0; i < tileObjects.Length; i++)
		{
			if (chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
			{
				tileObjects[i].gameObject.tag = "EnemyTile";
			}
			else
			{
				tileObjects[i].gameObject.tag = "StorableTile";
			}
		}
	}
	
	void OnEndDayMenuToggle(bool active)
	{
		endDayMenuEnabled = active;
	}

	// Update is called once per frame
	void Update ()
	{
		// Change the text depending on the current card value
		tileValueText.text = tileObjects[currentTileNumber].GetComponent<ChimneyTile>().TileValue.ToString();

		currentTileCatagory = chimneyTileTemplate[currentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].catagory;
		switch (currentTileCatagory)
		{
			case ChimneyTileTemplate.Catagory.ARMOUR:
				tileValueText.color = ArmourColour;
				break;
			case ChimneyTileTemplate.Catagory.WEAPON:
				tileValueText.color = WeaponColour;
				break;
			case ChimneyTileTemplate.Catagory.POTION:
				tileValueText.color = PotionColour;
				break;
			case ChimneyTileTemplate.Catagory.SKIPTOOL:
				tileValueText.color = SkipToolColour;
				break;
			case ChimneyTileTemplate.Catagory.ENEMY:
				tileValueText.color = EnemyColour;
				break;
			case ChimneyTileTemplate.Catagory.BOMB:
				tileValueText.color = BombColour;
				break;
			case ChimneyTileTemplate.Catagory.MONEY:
				tileValueText.color = MoneyColour;
				break;
			case ChimneyTileTemplate.Catagory.EMPTY:
				tileValueText.color = BombColour;
				break;
			default:
				break;
		}

		// Generate tiles
		if (!tilesGenerated)
		{
			for (int i = 0; i < tileObjects.Length; i++)
			{
				tileObjects[i].transform.position = new Vector3(tileObjects[i].transform.position.x, tileObjects[i].transform.position.y - tileObjects[i].transform.localScale.y * i - spaceBetweenTiles * i, tileObjects[i].transform.position.z);
			}
			tilesGenerated = true;
		}

		// Find the selected tile
			for (int i = 0; i < tileObjects.Length; i++)
			{
				if (tileObjects[i].GetComponent<ChimneyTile>().Selected)
				{
					tileSelected = true;
					currentlySelectedTile = tileObjects[i];
					currentTileNumber = i;
				}
			}

		// Make the tile glow appear on the selected tile
			tileGlow.transform.position = new Vector3(currentlySelectedTile.transform.position.x, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
			tileBackground.transform.position = new Vector3(currentlySelectedTile.transform.position.x, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
			tileValuesObject.transform.position = new Vector3(currentlySelectedTile.transform.position.x, currentlySelectedTile.transform.position.y - 5.5f, currentlySelectedTile.transform.position.z);

		// Make the tile glow render only when a tile is selected
		if (tileSelected)
		{
			tileGlow.GetComponent<SpriteRenderer>().enabled = true;
			scrollControl.ScrollRect.enabled = false;
		}
		else
		{
			tileGlow.GetComponent<SpriteRenderer>().enabled = false;
			scrollControl.ScrollRect.enabled = true;
		}

		if (!currentlySelectedTile.GetComponent<ChimneyTile>().Selected)
		{
			tileSelected = false;
		}


		// If the user has dragged left or right, drag the tile in that direction
		if (swipeControls.SwipeLeft || swipeControls.SwipeRight && !endDayMenuEnabled)
			{
				if (currentlySelectedTile.GetComponent<ChimneyTile>().MouseOver)
				{
					tileDragMode = true;
				}
			}

		// If the player lets go of the tile it will go back to it's original position
			if (!Input.GetMouseButton(0))
			{
				currentlySelectedTile.transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
				tileDragMode = false;
			}

		// Makes the tile follow the cursor when in drag mode
			if (tileDragMode && !endDayMenuEnabled)
			{
				mousePosition = Input.mousePosition;
				mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
				currentlySelectedTile.transform.position = new Vector3(mousePosition.x, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
			}
	}

	public void MoveToNextTile()
	{
		// Puts the tile back where it is supposed to be
		currentlySelectedTile.transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
		// Drag mode is disabled so the next tile doesn't automatically get dragged
		tileDragMode = false;
		// Change the selected tile to be the next in the queue
		tileObjects[currentTileNumber].GetComponent<ChimneyTile>().Selected = false;
		tileObjects[currentTileNumber].GetComponent<ChimneyTile>().TileUsed = true;
		tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().Selected = true;

		// Put the tile used sprite under the current tile
		tileUsed.transform.position = new Vector3(tileObjects[currentTileNumber + 1].transform.position.x, tileObjects[currentTileNumber + 1].transform.position.y, tileObjects[currentTileNumber + 1].transform.position.z);
	}
}
