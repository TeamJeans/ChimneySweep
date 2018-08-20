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
	bool endDayMenuEnabled = false;
	bool tileSelected = false;
	bool hasHearthTileBeenGenerated = false;

	public GameObject[] tilePrefabs;
	public GameObject[] tileObjects;
	public ChimneyTileTemplate[] chimneyTileTemplate;
	public ChimneyTileTemplate[] bossTypesArray;

	[SerializeField]
	GameObject tileValuesCanvas;
	public Text[] tileValueText;
	public Text currentTileValueText;

	[SerializeField]
	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } set { currentlySelectedTile = value; } }
	[SerializeField]
	int currentTileNumber = 0;
	public int CurrentTileNumber{ get { return currentTileNumber; } }

	[SerializeField]
	GameObject tileGlow;
	[SerializeField]
	GameObject[] tileBackground;
	[SerializeField]
	GameObject[] tileUsedArray;
	[SerializeField]
	GameObject tileUsed;
	[SerializeField]
	GameObject[] tileValuesObject;
	[SerializeField]
	Sprite tileBackSprite;
	[SerializeField]
	ChimneySidesGenerator chimneySidesGenerator;

	// Use this for initialization
	void Start ()
	{
		// Toggles control of the tiles when the end day menu is open
		GameMaster.gm.onToggleEndDayMenu += OnEndDayMenuToggle;

		tileObjects = new GameObject[tilePrefabs.Length];
		tileValueText = new Text[tileObjects.Length];
		tileValuesObject = new GameObject[tileObjects.Length];
		tileBackground = new GameObject[tileObjects.Length];
		tileUsedArray = new GameObject[tileObjects.Length];
		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			// Creates the array of blank tiles
			tileObjects[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tileObjects[i].transform.SetParent(gameObject.transform);

			// Randomly changes the type of each tile generated
			//while (chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
			//{
			tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = Random.Range(2, chimneyTileTemplate.Length);
			//}
			// Find the min and max values for the current chimney tile template and generate a random number and set it to be the tiles value
			tileObjects[i].GetComponent<ChimneyTile>().TileValue = Random.Range(chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].minTileValue, chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].maxTileValue);

			tileValuesObject[i] = Instantiate(Resources.Load("Prefabs/TileValuesObject", typeof(GameObject)), transform) as GameObject;
			tileValuesObject[i].transform.SetParent(tileValuesCanvas.transform);
			tileValueText[i] = Instantiate(Resources.Load("Prefabs/TileValueText", typeof(Text))) as Text;
			tileValueText[i].gameObject.transform.SetParent(tileValuesObject[i].transform);

			tileBackground[i] = Instantiate(Resources.Load("Prefabs/Tile_Background", typeof(GameObject)), transform) as GameObject;
			tileBackground[i].transform.SetParent(gameObject.transform);

			tileUsedArray[i] = Instantiate(Resources.Load("Prefabs/Tile_Used", typeof(GameObject)), transform) as GameObject;
			tileUsedArray[i].transform.SetParent(gameObject.transform);

			// Setup the positions for the tiles and their values
			tileObjects[i].transform.position = new Vector3(tileObjects[i].transform.position.x, tileObjects[i].transform.position.y - tileObjects[i].transform.localScale.y * i - spaceBetweenTiles * i, tileObjects[i].transform.position.z);
			tileValuesObject[i].transform.position = new Vector3(tileObjects[i].transform.position.x, tileObjects[i].transform.position.y - 5.5f, tileObjects[i].transform.position.z);
			tileBackground[i].transform.position = new Vector3(tileObjects[i].transform.position.x, tileObjects[i].transform.position.y, tileObjects[i].transform.position.z);
			tileUsedArray[i].transform.position = new Vector3(tileObjects[i].transform.position.x, tileObjects[i].transform.position.y, tileObjects[i].transform.position.z);

			// Change the text depending on the current card value
			tileValueText[i].text = tileObjects[i].GetComponent<ChimneyTile>().TileValue.ToString();
			currentTileCatagory = chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory;
			switch (currentTileCatagory)
			{
				case ChimneyTileTemplate.Catagory.ARMOUR:
					tileValueText[i].color = ArmourColour;
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:
					tileValueText[i].color = WeaponColour;
					break;
				case ChimneyTileTemplate.Catagory.POTION:
					tileValueText[i].color = PotionColour;
					break;
				case ChimneyTileTemplate.Catagory.SKIPTOOL:
					tileValueText[i].color = SkipToolColour;
					break;
				case ChimneyTileTemplate.Catagory.ENEMY:
					tileValueText[i].color = EnemyColour;
					break;
				case ChimneyTileTemplate.Catagory.BOMB:
					tileValueText[i].color = BombColour;
					break;
				case ChimneyTileTemplate.Catagory.MONEY:
					tileValueText[i].color = MoneyColour;
					break;
				case ChimneyTileTemplate.Catagory.EMPTY:
					tileValueText[i].color = BombColour;
					break;
				default:
					break;
			}

			// Give the tiles tags
			if (chimneyTileTemplate[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
			{
				tileObjects[i].gameObject.tag = "EnemyTile";
			}
			else
			{
				tileObjects[i].gameObject.tag = "StorableTile";
			}
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

			// Find the boss tiles
			if (chimneyTileTemplate[i].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
			{
				bossTypesArray[i] = chimneyTileTemplate[i];
			}
		}

		// For the hearth tile
		//tileObjects[tileObjects.Length -1].GetComponent<ChimneyTile>().RandomTileTypeNum = Random.Range(0, bossTypesArray.Length);
		//for (int i = 0; i < chimneyTileTemplate.Length; i++)
		//{
		//	if (bossTypesArray[tileObjects[tileObjects.Length - 1].GetComponent<ChimneyTile>().RandomTileTypeNum] == chimneyTileTemplate[i])
		//	{
		//		tileObjects[tileObjects.Length - 1].GetComponent<ChimneyTile>().RandomTileTypeNum = i;
		//		return;
		//	}
		//}
		//tileObjects[tileObjects.Length - 1].GetComponent<ChimneyTile>().TileValue = Random.Range(bossTypesArray[tileObjects[tileObjects.Length - 1].GetComponent<ChimneyTile>().RandomTileTypeNum].minTileValue, bossTypesArray[tileObjects[tileObjects.Length - 1].GetComponent<ChimneyTile>().RandomTileTypeNum].maxTileValue);
	}
	
	void OnEndDayMenuToggle(bool active)
	{
		endDayMenuEnabled = active;
	}

	// Update is called once per frame
	void Update()
	{
		// Change the text depending on the current card value
		tileValueText[currentTileNumber].text = tileObjects[currentTileNumber].GetComponent<ChimneyTile>().TileValue.ToString();

		// Generate hearth tile if it hasn't already done so
		if (!hasHearthTileBeenGenerated)
		{
			hasHearthTileBeenGenerated = true;

			// Setup the hearth tile
			tileObjects[tileObjects.Length - 1].transform.position = new Vector3(tileObjects[tileObjects.Length - 1].transform.position.x, chimneySidesGenerator.ChimneyHearth.transform.position.y, tileObjects[tileObjects.Length - 1].transform.position.z);
			tileBackground[tileObjects.Length - 1].transform.position = new Vector3(tileObjects[tileObjects.Length - 1].transform.position.x, chimneySidesGenerator.ChimneyHearth.transform.position.y, tileObjects[tileObjects.Length - 1].transform.position.z);
			tileUsedArray[tileObjects.Length - 1].transform.position = new Vector3(tileObjects[tileObjects.Length - 1].transform.position.x, chimneySidesGenerator.ChimneyHearth.transform.position.y, tileObjects[tileObjects.Length - 1].transform.position.z);
			tileValuesObject[tileObjects.Length - 1].transform.position = new Vector3(tileObjects[tileObjects.Length - 1].transform.position.x, chimneySidesGenerator.ChimneyHearth.transform.position.y, tileObjects[tileObjects.Length - 1].transform.position.z);
		}

		// Move the text with it's tile
			tileValuesObject[currentTileNumber].transform.position = new Vector3(tileObjects[currentTileNumber].transform.position.x, tileObjects[currentTileNumber].transform.position.y - 5.5f, tileObjects[currentTileNumber].transform.position.z);
			tileBackground[currentTileNumber].transform.position = new Vector3(tileObjects[currentTileNumber].transform.position.x, tileObjects[currentTileNumber].transform.position.y, tileObjects[currentTileNumber].transform.position.z);

		// Cycle through all the tiles
		for (int i = 0; i < tileObjects.Length; i++)
		{

			// Check if the text should be showing or not
			if (tileObjects[i].GetComponent<SpriteRenderer>().sprite == tileBackSprite)
				{
					tileValueText[i].gameObject.SetActive(false);
				}
				else
				{
					tileValueText[i].gameObject.SetActive(true);
				}

			// Find the selected tile
				if (tileObjects[i].GetComponent<ChimneyTile>().Selected)
				{
					tileSelected = true;
					currentlySelectedTile = tileObjects[i];
					currentTileNumber = i;
				}

			// If the tile has been used put a transparent black cover over it
				if (tileObjects[i].GetComponent<ChimneyTile>().TileUsed)
				{
					tileUsedArray[i].SetActive(true);
				}
		}

		// Find what the current text value is
			currentTileValueText = tileValueText[currentTileNumber];

		// Make the tile glow render only when a tile is selected
			if (tileSelected)
			{
				tileGlow.GetComponent<SpriteRenderer>().enabled = true;
				// Make the tile glow appear on the selected tile
				tileGlow.transform.position = new Vector3(currentlySelectedTile.transform.position.x, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
			}
			else
			{
				tileGlow.GetComponent<SpriteRenderer>().enabled = false;
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

	public void SkipTiles(int noOfTilesToSkip)
	{
		// Puts the tile back where it is supposed to be
		currentlySelectedTile.transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
		// Drag mode is disabled so the next tile doesn't automatically get dragged
		tileDragMode = false;
		// Change the selected tile to be the next in the queue
		tileObjects[currentTileNumber].GetComponent<ChimneyTile>().Selected = false;
		for (int i = 0; i < noOfTilesToSkip; i++)
		{
			tileObjects[currentTileNumber + i].GetComponent<ChimneyTile>().TileUsed = true;
		}
		tileObjects[currentTileNumber + noOfTilesToSkip].GetComponent<ChimneyTile>().Selected = true;

		// Put the tile used sprite under the current tile
		tileUsed.transform.position = new Vector3(tileObjects[currentTileNumber + noOfTilesToSkip].transform.position.x, tileObjects[currentTileNumber + noOfTilesToSkip].transform.position.y, tileObjects[currentTileNumber + noOfTilesToSkip].transform.position.z);
	}

	public void ShowTiles(int noOfTilesToShow)
	{
		for (int i = 0; i < noOfTilesToShow; i++)
		{
			tileObjects[currentTileNumber + i + 1].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplate[tileObjects[currentTileNumber + i + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
		}
	}
}
