using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;
	public float SpaceBetweenTiles { get { return spaceBetweenTiles; } }
	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	ScrollControl scrollControl;

	// Colours for tile values
	[SerializeField]
	Color armourColour;
	[SerializeField]
	string armourDescription;
	[SerializeField]
	Color weaponColour;
	[SerializeField]
	string weaponDescription;
	[SerializeField]
	Color potionColour;
	[SerializeField]
	string potionDescription;
	[SerializeField]
	Color skipToolColour;
	[SerializeField]
	string skipToolDescription;
	[SerializeField]
	Color enemyColour;
	[SerializeField]
	string enemyDescription;
	[SerializeField]
	Color bombColour;
	[SerializeField]
	string bombDescription;
	[SerializeField]
	Color moneyColour;
	[SerializeField]
	string moneyDescription;

	// Variables for holding down tiles
	bool showTileDescription = false;
	public bool ShowTileDescription { get { return showTileDescription; } set { showTileDescription = value; } }
	float elapsedTimeForTileBeingHeldDown = 0f;
	[SerializeField]
	float lengthOfTimeTileNeedsToBeHeldDown = 2f;
	[SerializeField]
	GameObject tileDescriptionMenu;
	[SerializeField]
	Text tileNameText;
	[SerializeField]
	Text tileTypeText;
	[SerializeField]
	Text tileDescriptionText;
	[SerializeField]
	Text tileTypeDescriptionText;
	[SerializeField]
	Image currentTileSprite;

	[SerializeField]
	TileCatagoryVariables tileCatagoryVariables;

	Vector2 mousePosition;
	ChimneyTileTemplate.Catagory currentTileCatagory;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; } set { tileDragMode = value; } }
	bool endDayMenuEnabled = false;
	bool tileSelected = false;
	bool hasHearthTileBeenGenerated = false;
	bool particleEffectsInstatiated = false;

	[SerializeField]
	GameObject[] tilePrefabs;
	public GameObject[] TilePrefabs { get { return tilePrefabs; } }
	GameObject[] tileObjects;
	public GameObject[] TileObjects { get { return tileObjects; } set { tileObjects = value; } } 
	[SerializeField]
	ChimneyTileTemplate[] chimneyTileTemplateArray;
	public ChimneyTileTemplate[] ChimneyTileTemplateArray { get { return chimneyTileTemplateArray; } }

	[SerializeField]
	GameObject tileValuesCanvas;
	Text[] tileValueText;
	public Text[] TileValueText { get { return tileValueText; } }
	Text currentTileValueText;
	public Text CurrentTileValueText { get { return currentTileValueText; } }

	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } set { currentlySelectedTile = value; } }
	int currentTileNumber = 0;
	public int CurrentTileNumber{ get { return currentTileNumber; } }

	[SerializeField]
	GameObject tileGlow;
	[SerializeField]
	GameObject tileAppearPS;

	[SerializeField]
	GameObject tile;
	GameObject[] tileBackground;
	GameObject[] tileUsedArray;
	[SerializeField]
	GameObject tileUsed;
	GameObject[] tileValuesObject;
	[SerializeField]
	Sprite tileBackSprite;
	[SerializeField]
	ChimneySidesGenerator chimneySidesGenerator;
	[SerializeField]
	GameObject tileTextScale;
	int bossCounter = 0;
	int[] bossTileTemplates;
	GameObject[] tempParticleArray; // Stores each particle for a tile

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

		for (int i = 0; i < chimneyTileTemplateArray.Length; i++)
		{
			// Check how many bosses are avaliable
			if (chimneyTileTemplateArray[i].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
			{
				bossCounter++;
			}

			// Make it so that you can't put enemies or money items in the inventory
			if (chimneyTileTemplateArray[i].catagory == ChimneyTileTemplate.Catagory.ENEMY || chimneyTileTemplateArray[i].catagory == ChimneyTileTemplate.Catagory.MONEY)
			{
				chimneyTileTemplateArray[i].Storable = false;
			}
		}
		bossTileTemplates = new int[bossCounter];   // Give the boss array the right size

		for (int i = 0; i < bossTileTemplates.Length; i++)
			bossTileTemplates[i] = 0;

		int counter = 0;
		for (int i = 0; i < chimneyTileTemplateArray.Length; i++)
		{
			if (chimneyTileTemplateArray[i].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
			{
				bossTileTemplates[counter] = i;
				counter++;
			}
		}

		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			// Creates the array of blank tiles
			tileObjects[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tileObjects[i].transform.SetParent(gameObject.transform);
			tileObjects[i].transform.localScale = tile.transform.localScale;

			// Randomly changes the type of each tile generated
			if (i == tileObjects.Length -1)
			{
				int tempIndex = Random.Range(0, bossTileTemplates.Length);
				//Debug.Log("Index: " + tempIndex);
				//Debug.Log("Tile Random No: " + bossTileTemplates[tempIndex]);
				tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = bossTileTemplates[tempIndex];
				//Debug.Log("Tile Random No: " + bossTileTemplates[tempIndex]);
			}
			else
			{
				do
				{
					tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = GetRandomTileBasedOnPercentage();
				}
				while (chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS);
			}


			// Find the min and max values for the current chimney tile template and generate a random number and set it to be the tiles value
			tileObjects[i].GetComponent<ChimneyTile>().TileValue = Random.Range(chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].minTileValue, chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].maxTileValue);

			tileValuesObject[i] = Instantiate(Resources.Load("Prefabs/TileValuesObject", typeof(GameObject)), transform) as GameObject;
			tileValuesObject[i].transform.SetParent(tileValuesCanvas.transform);
			tileValueText[i] = Instantiate(Resources.Load("Prefabs/TileValueText", typeof(Text))) as Text;
			tileValueText[i].gameObject.transform.SetParent(tileValuesObject[i].transform);
			tileValueText[i].transform.localScale = tileTextScale.transform.localScale;

			tileBackground[i] = Instantiate(Resources.Load("Prefabs/Tile_Background", typeof(GameObject)), transform) as GameObject;
			tileBackground[i].transform.SetParent(gameObject.transform);

			tileUsedArray[i] = Instantiate(Resources.Load("Prefabs/Tile_Used", typeof(GameObject)), transform) as GameObject;
			tileUsedArray[i].transform.SetParent(gameObject.transform);

			// Setup the positions for the tiles and their values
			float scale = i * tileObjects[i].transform.localScale.y + i * spaceBetweenTiles;
			tileObjects[i].transform.localPosition = gameObject.transform.localPosition + new Vector3(0,-scale,0);
			tileValuesObject[i].transform.localPosition = gameObject.transform.localPosition + new Vector3(-2.5f, -scale + 7.5f, 0);
			tileBackground[i].transform.localPosition = gameObject.transform.localPosition + new Vector3(0, -scale, 0);
			tileUsedArray[i].transform.localPosition = gameObject.transform.localPosition + new Vector3(0, -scale, 0);

			// Change the text depending on the current card value
			tileValueText[i].text = tileObjects[i].GetComponent<ChimneyTile>().TileValue.ToString();
			currentTileCatagory = chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory;
			switch (currentTileCatagory)
			{
				case ChimneyTileTemplate.Catagory.ARMOUR:
					tileValueText[i].color = armourColour;
					break;
				case ChimneyTileTemplate.Catagory.WEAPON:
					tileValueText[i].color = weaponColour;
					break;
				case ChimneyTileTemplate.Catagory.POTION:
					tileValueText[i].color = potionColour;
					break;
				case ChimneyTileTemplate.Catagory.SKIPTOOL:
					tileValueText[i].color = skipToolColour;
					break;
				case ChimneyTileTemplate.Catagory.ENEMY:
					tileValueText[i].color = enemyColour;
					break;
				case ChimneyTileTemplate.Catagory.BOMB:
					tileValueText[i].color = bombColour;
					break;
				case ChimneyTileTemplate.Catagory.MONEY:
					tileValueText[i].color = moneyColour;
					break;
				case ChimneyTileTemplate.Catagory.EMPTY:
					tileValueText[i].color = bombColour;
					break;
				default:
					break;
			}

			// Give the tiles tags
			if (chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory == ChimneyTileTemplate.Catagory.ENEMY)
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

		Debug.Log("START: " + currentlySelectedTile);

		// Make the next tile visable. This needs to be done at the start because "Move to next tile isn't called"
		if (currentTileNumber != tileObjects.Length - 1)
		{
			// Show puff of smoke before showing the next tile
			if (tileObjects[currentTileNumber + 1].GetComponent<SpriteRenderer>().sprite != chimneyTileTemplateArray[tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork)
			{
				tileAppearPS.transform.localPosition = new Vector3(0, tileObjects[currentTileNumber + 1].transform.localPosition.y + 10, 0);
				tileAppearPS.GetComponent<ParticleSystem>().Play();

				// Show the artwork for that tile
				tileObjects[currentTileNumber + 1].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
			}

		}
	}

	// Update is called once per frame
	void Update()
	{
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
			tileValuesObject[tileObjects.Length - 1].transform.localPosition = new Vector3(tileObjects[tileObjects.Length - 1].transform.localPosition.x - 2.5f, tileObjects[tileObjects.Length - 1].transform.localPosition.y + 7.5f, tileObjects[tileObjects.Length - 1].transform.position.z);
		}

		// Move the text with it's tile
			tileValuesObject[currentTileNumber].transform.localPosition = new Vector3(tileObjects[currentTileNumber].transform.localPosition.x - 2.5f, tileObjects[currentTileNumber].transform.localPosition.y + 7.5f, tileObjects[currentTileNumber].transform.position.z);
			tileBackground[currentTileNumber].transform.position = new Vector3(tileObjects[currentTileNumber].transform.position.x, tileObjects[currentTileNumber].transform.position.y, tileObjects[currentTileNumber].transform.position.z);


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

		// Deal with showing the tile descriptions
			if (currentlySelectedTile.transform.position.x == 0 && currentlySelectedTile.GetComponent<ChimneyTile>().MouseOver && Input.GetMouseButton(0))
			{
				elapsedTimeForTileBeingHeldDown += Time.deltaTime;
				if (elapsedTimeForTileBeingHeldDown >= lengthOfTimeTileNeedsToBeHeldDown)
				{
					showTileDescription = true;
					elapsedTimeForTileBeingHeldDown = 0f;
				}
			}
			else
			{
				elapsedTimeForTileBeingHeldDown = 0f;
			}

			if (showTileDescription)
			{
				showTileDescription = false;
				SetUpCurrentTileDescription();
				tileDescriptionMenu.SetActive(true);
			}

			if (tileDescriptionMenu.activeSelf == true && Input.GetMouseButtonDown(0))
			{
				tileDescriptionMenu.SetActive(false);
			}

		// Intatiate particle effects
		if (!particleEffectsInstatiated)
		{
			particleEffectsInstatiated = true;
			instantiateParticles();
		}
	}

	void OnEndDayMenuToggle(bool active)
	{
		endDayMenuEnabled = active;
	}

	public void MoveToNextTile()
	{
		// Destroy particle effects
		if (particleEffectsInstatiated)
		{
			particleEffectsInstatiated = false;
			destroyParticles();
		}

		// Puts the tile back where it is supposed to be
		currentlySelectedTile.transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
		tileBackground[currentTileNumber].transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
		tileValuesObject[currentTileNumber].transform.localPosition = new Vector3(- 2.5f, tileObjects[currentTileNumber].transform.localPosition.y + 7.5f, tileObjects[currentTileNumber].transform.position.z);

		// Drag mode is disabled so the next tile doesn't automatically get dragged
		tileDragMode = false;

		// Change the selected tile to be the next in the queue
		tileObjects[currentTileNumber].GetComponent<ChimneyTile>().Selected = false;
		tileObjects[currentTileNumber].GetComponent<ChimneyTile>().TileUsed = true;
		tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().Selected = true;


		// Put the tile used sprite under the current tile
		tileUsed.transform.position = new Vector3(tileObjects[currentTileNumber + 1].transform.position.x, tileObjects[currentTileNumber + 1].transform.position.y, tileObjects[currentTileNumber + 1].transform.position.z);
		CurrentlySelectedTile = tileObjects[currentTileNumber + 1];
		currentTileNumber++;

		// Make the next tile visable
		if (currentTileNumber != tileObjects.Length - 1)
		{
			// Show puff of smoke before showing the next tile
			if (tileObjects[currentTileNumber + 1].GetComponent<SpriteRenderer>().sprite != chimneyTileTemplateArray[tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork)
			{
				tileAppearPS.transform.localPosition = new Vector3(0,tileObjects[currentTileNumber + 1].transform.localPosition.y + 10,0);
				tileAppearPS.GetComponent<ParticleSystem>().Play();

				// Show the artwork for that tile
				tileObjects[currentTileNumber + 1].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[currentTileNumber + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
			}

		}
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
			if (tileObjects.Length > (currentTileNumber + i + 1))
			{
				tileObjects[currentTileNumber + i + 1].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[currentTileNumber + i + 1].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
			}
		}
	}

	public void SetUpCurrentTileDescription()
	{
		tileDescriptionMenu.SetActive(true);

		switch (chimneyTileTemplateArray[tileObjects[currentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory)
		{
			case ChimneyTileTemplate.Catagory.ARMOUR:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Armour";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = armourDescription;
				break;
			case ChimneyTileTemplate.Catagory.WEAPON:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Weapon";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = weaponDescription;
				break;
			case ChimneyTileTemplate.Catagory.POTION:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Potion";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = potionDescription;
				break;
			case ChimneyTileTemplate.Catagory.SKIPTOOL:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Skip Tool";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = skipToolDescription;
				break;
			case ChimneyTileTemplate.Catagory.ENEMY:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Enemy";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = enemyDescription;
				break;
			case ChimneyTileTemplate.Catagory.BOMB:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Bomb";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = bombDescription;
				break;
			case ChimneyTileTemplate.Catagory.MONEY:
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName = "Money";
				tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription = moneyDescription;
				break;
			default:
				break;
		}

		tileNameText.text = chimneyTileTemplateArray[tileObjects[currentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].tileName;
		tileTypeText.text = "Type: " + tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryName;
		tileDescriptionText.text = chimneyTileTemplateArray[tileObjects[currentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].description;
		tileTypeDescriptionText.text = tileObjects[currentTileNumber].GetComponent<ChimneyTile>().CatagoryDescription;
		currentTileSprite.sprite = chimneyTileTemplateArray[tileObjects[currentTileNumber].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
	}

	int GetRandomTileBasedOnPercentage()
	{
		ChimneyTileTemplate.Catagory randomCatagory = GetRandomTileCatagory();

		float tileWeight = 0;
		for (int i = 0; i < chimneyTileTemplateArray.Length; i++)
		{
			if (chimneyTileTemplateArray[i].catagory == randomCatagory)
			{
				tileWeight += chimneyTileTemplateArray[i].spawnPercentage + chimneyTileTemplateArray[i].spawnPercentageIncreasePerWeek * StaticValueHolder.CurrentWeek;
			}
		}

		float randomValue = Random.Range(0, tileWeight);

		for (int i = 0; i < chimneyTileTemplateArray.Length; i++)
		{
			if (chimneyTileTemplateArray[i].catagory == randomCatagory)
			{
				if (randomValue <= (chimneyTileTemplateArray[i].spawnPercentage + chimneyTileTemplateArray[i].spawnPercentageIncreasePerWeek * StaticValueHolder.CurrentWeek))
				{
					return i;
				}
				randomValue -= (chimneyTileTemplateArray[i].spawnPercentage + chimneyTileTemplateArray[i].spawnPercentageIncreasePerWeek * StaticValueHolder.CurrentWeek);
			}	
		}

		return 0;
	}

	ChimneyTileTemplate.Catagory GetRandomTileCatagory()
	{
		float catagoryItemWeight = tileCatagoryVariables.SumOfPercentages;
		float randomCatagoryValue = Random.Range(0, catagoryItemWeight);
		for (int i = 0; i < tileCatagoryVariables.percentages.Length; i++)
		{
			if (randomCatagoryValue <= tileCatagoryVariables.percentages[i])
			{
				Debug.Log((ChimneyTileTemplate.Catagory)i);
				return (ChimneyTileTemplate.Catagory)i;
			}
			randomCatagoryValue -= tileCatagoryVariables.percentages[i];
		}

		return (ChimneyTileTemplate.Catagory)0;
	}

	void instantiateParticles()
	{
		if (chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects != null)
		{
			tempParticleArray = new GameObject[chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects.Length];
			for (int i = 0; i < chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects.Length; i++)
			{
				tempParticleArray[i] = chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects[i];
				tempParticleArray[i] = Instantiate(tempParticleArray[i]) as GameObject;
				tempParticleArray[i].transform.SetParent(tileGlow.transform);
				tempParticleArray[i].transform.localPosition = new Vector3(0,-0.3f,0);
			}
		}
	}

	void destroyParticles()
	{
		if (chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects != null)
		{
			for (int i = 0; i < chimneyTileTemplateArray[CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].particleEffects.Length; i++)
			{
				Destroy(tempParticleArray[i]);
			}
		}
	}
}
