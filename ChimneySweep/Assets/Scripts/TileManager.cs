using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileManager : MonoBehaviour {

	[SerializeField]
	GameMaster gm;
	[SerializeField]
	Inventory inventory;
	
	[SerializeField]
	bool isShopChimney = false;
	public bool IsShopChimney { get { return isShopChimney; }}

	[SerializeField]
	float spaceBetweenTiles = 100f;
	public float SpaceBetweenTiles { get { return spaceBetweenTiles; } }
	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	CameraControl cameraControl;

	// Colours for tile values
	[SerializeField]
	string armourDescription;
	[SerializeField]
	string weaponDescription;
	[SerializeField]
	string potionDescription;
	[SerializeField]
	string skipToolDescription;
	[SerializeField]
	string enemyDescription;
	[SerializeField]
	string bombDescription;
	[SerializeField]
	string moneyDescription;

	// Variables for holding down tiles
	bool showTileDescription = false;
	public bool ShowTileDescription { get { return showTileDescription; } set { showTileDescription = value; } }
	float elapsedTimeForTileBeingHeldDown = 0f;
	[SerializeField]
	float lengthOfTimeTileNeedsToBeHeldDown = 1f;

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
	Image currentTileBackgroundSprite;

	[SerializeField]
	TileCatagoryVariables tileCatagoryVariables;

	Vector2 mousePosition;
	ChimneyTileTemplate.Catagory currentTileCatagory;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; } set { tileDragMode = value; } }
	bool endDayMenuEnabled = false;
	public bool EndDayMenuEnabled { get { return endDayMenuEnabled; } }
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

	[SerializeField]
	Transform clairvoyanceTileAppearParticles;

	// Use this for initialization
	void Start ()
	{
		// Toggles control of the tiles when the end day menu is open
		GameMaster.gm.onToggleEndDayMenu += OnEndDayMenuToggle;


		for (int i = 0; i < chimneyTileTemplateArray.Length; i++)
		{
			// Check how many bosses are avaliable
			if (!isShopChimney)
			{
				if (chimneyTileTemplateArray[i].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
				{
					bossCounter++;
				}
			}

			// Make it so that you can't put enemies or money items in the inventory
			if (chimneyTileTemplateArray[i].catagory == ChimneyTileTemplate.Catagory.ENEMY || chimneyTileTemplateArray[i].catagory == ChimneyTileTemplate.Catagory.MONEY)
			{
				chimneyTileTemplateArray[i].Storable = false;
			}
		}

		if (!isShopChimney)
		{
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
		}

		// Create the size for all the arrays
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
			tileObjects[i].transform.localScale = tile.transform.localScale;

			// Randomly changes the type of each tile generated
			if (i == tileObjects.Length -1 && !isShopChimney)
			{
				int tempIndex = Random.Range(0, bossTileTemplates.Length);
				//Debug.Log("Index: " + tempIndex);
				//Debug.Log("Tile Random No: " + bossTileTemplates[tempIndex]);
				tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = bossTileTemplates[tempIndex];
				//Debug.Log("Tile Random No: " + bossTileTemplates[tempIndex]);
			}
			else
			{
				// Generate a random tile and if it is a boss tile then keep generating them until it is not a boss tile
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

			// Set tile backgrounds based on the category that tile belongs to
			SetTileBackgrounds(i);

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


		// If a shop has already been generated then set the values for that shop
		Debug.Log(ShopChimneyValues.NewShopGenerated);
		if (ShopChimneyValues.NewShopGenerated && isShopChimney)
		{
			LoadShopChimney();
			Debug.Log("START: " + currentlySelectedTile);
		}
		else
		{
			// Sets the selected tile to be the first tile generated
			tileObjects[0].GetComponent<ChimneyTile>().Selected = true;
			currentlySelectedTile = tileObjects[0];

			Debug.Log("START: " + currentlySelectedTile);
		}

		// Show three tiles at a time if the chimney is a shop chimney
		if (!isShopChimney)
		{
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
		else
		{
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

				if (currentTileNumber != tileObjects.Length - 2)
				{
					// Show puff of smoke before showing the next tile
					if (tileObjects[currentTileNumber + 2].GetComponent<SpriteRenderer>().sprite != chimneyTileTemplateArray[tileObjects[currentTileNumber + 2].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork)
					{
						tileAppearPS.transform.localPosition = new Vector3(0, tileObjects[currentTileNumber + 2].transform.localPosition.y + 10, 0);
						tileAppearPS.GetComponent<ParticleSystem>().Play();

						// Show the artwork for that tile
						tileObjects[currentTileNumber + 2].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[currentTileNumber + 2].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
					}
				}
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
		// If the chimney is a shop chimney then stop at the right amount of tiles to show
		if (isShopChimney)
		{
			switch (ShopChimneyValues.NoOfShopChimneyVisits)
			{
				case 0:
					if (currentTileNumber == 2)
					{
						// Make this go to another scene
						Debug.Log("Fix me");
						ShopChimneyValues.NoOfShopChimneyVisits++;
						ShopChimneyValues.CurrentTilenumber = currentTileNumber + 1;
						ShopChimneyValues.RandomlyGeneratedTileIndex = new int[tilePrefabs.Length];
						ShopChimneyValues.RandomlyGeneratedTileValue = new int[tilePrefabs.Length];
						for (int i = 0; i < tileObjects.Length; i++)
						{
							ShopChimneyValues.RandomlyGeneratedTileIndex[i] = tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum;
							ShopChimneyValues.RandomlyGeneratedTileValue[i] = tileObjects[i].GetComponent<ChimneyTile>().ConstTileValue;
						}
						ShopChimneyValues.NewShopGenerated = true;
						inventory.SaveInventoryValues();
						gm.ChangeSceneToChimneyScene();
						return;
					}
					break;
				case 1:
					if (currentTileNumber == 5)
					{
						// Make this go to another scene
						Debug.Log("Fix me");
						ShopChimneyValues.NoOfShopChimneyVisits++;
						ShopChimneyValues.CurrentTilenumber = currentTileNumber + 1;
						ShopChimneyValues.RandomlyGeneratedTileIndex = new int[tilePrefabs.Length];
						for (int i = 0; i < tileObjects.Length; i++)
						{
							ShopChimneyValues.RandomlyGeneratedTileIndex[i] = tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum;
						}
						ShopChimneyValues.NewShopGenerated = true;
						gm.ChangeSceneToChimneyScene();
						return;
					}
					break;
				default:
					break;
			}
		}

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
		if (currentTileNumber != tileObjects.Length - 1 && !isShopChimney)
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
		CurrentlySelectedTile = tileObjects[currentTileNumber + noOfTilesToSkip];
		currentTileNumber += noOfTilesToSkip;

		// Make the next tile visable
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

		cameraControl.SetDesiredCamPos();
	}

	public void ShowTiles(int noOfTilesToShow)
	{
		for (int i = 1; i < noOfTilesToShow + 1; i++)
		{
			if (tileObjects.Length > (currentTileNumber + i + 1))
			{
				// Add particles
				Transform _purpleParticleClone = (Transform)Instantiate(clairvoyanceTileAppearParticles);
				_purpleParticleClone.parent = tileGlow.transform.parent;
				_purpleParticleClone.localPosition = new Vector3(0, tileObjects[currentTileNumber + i + 1].transform.localPosition.y - 10, 0);
				Destroy(_purpleParticleClone.gameObject, 2f);

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
		currentTileBackgroundSprite.sprite = tileBackground[currentTileNumber].GetComponent<SpriteRenderer>().sprite;
	}

	public void SetUpInventoryTileDescription(InventorySlot inventorySlot)
	{
		tileDescriptionMenu.SetActive(true);

		switch (chimneyTileTemplateArray[inventorySlot.ChimneyTileTemplateIndex].catagory)
		{
			case ChimneyTileTemplate.Catagory.ARMOUR:
				inventorySlot.CatagoryName = "Armour";
				inventorySlot.CatagoryDescription = armourDescription;
				break;
			case ChimneyTileTemplate.Catagory.WEAPON:
				inventorySlot.CatagoryName = "Weapon";
				inventorySlot.CatagoryDescription = weaponDescription;
				break;
			case ChimneyTileTemplate.Catagory.POTION:
				inventorySlot.CatagoryName = "Potion";
				inventorySlot.CatagoryDescription = potionDescription;
				break;
			case ChimneyTileTemplate.Catagory.SKIPTOOL:
				inventorySlot.CatagoryName = "Skip Tool";
				inventorySlot.CatagoryDescription = skipToolDescription;
				break;
			case ChimneyTileTemplate.Catagory.ENEMY:
				inventorySlot.CatagoryName = "Enemy";
				inventorySlot.CatagoryDescription = enemyDescription;
				break;
			case ChimneyTileTemplate.Catagory.BOMB:
				inventorySlot.CatagoryName = "Bomb";
				inventorySlot.CatagoryDescription = bombDescription;
				break;
			case ChimneyTileTemplate.Catagory.MONEY:
				inventorySlot.CatagoryName = "Money";
				inventorySlot.CatagoryDescription = moneyDescription;
				break;
			default:
				break;
		}

		tileNameText.text = chimneyTileTemplateArray[inventorySlot.ChimneyTileTemplateIndex].tileName;
		tileTypeText.text = "Type: " + inventorySlot.CatagoryName;
		tileDescriptionText.text = chimneyTileTemplateArray[inventorySlot.ChimneyTileTemplateIndex].description;
		tileTypeDescriptionText.text = inventorySlot.CatagoryDescription;
		currentTileSprite.sprite = chimneyTileTemplateArray[inventorySlot.ChimneyTileTemplateIndex].artwork;
		currentTileBackgroundSprite.sprite = inventorySlot.TileBackgroundSprite;
	}

	public void ShowInventoryTileDescription(InventorySlot inventorySlot)
	{
		if (!inventorySlot.BeingDragged && inventorySlot.MouseOver && Input.GetMouseButton(0))
		{
			inventorySlot.ElapsedTimeForTileBeingHeldDown += Time.deltaTime;
			if (inventorySlot.ElapsedTimeForTileBeingHeldDown >= inventorySlot.LengthOfTimeTileNeedsToBeHeldDown)
			{
				showTileDescription = true;
				inventorySlot.ElapsedTimeForTileBeingHeldDown = 0f;
			}
		}
		else
		{
			inventorySlot.ElapsedTimeForTileBeingHeldDown = 0f;
		}

		if (showTileDescription)
		{
			showTileDescription = false;
			SetUpInventoryTileDescription(inventorySlot);
			tileDescriptionMenu.SetActive(true);
		}

		if (tileDescriptionMenu.activeSelf == true && Input.GetMouseButtonDown(0))
		{
			tileDescriptionMenu.SetActive(false);
		}
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

	void SetTileBackgrounds(int index)
	{
		switch (chimneyTileTemplateArray[tileObjects[index].GetComponent<ChimneyTile>().RandomTileTypeNum].catagory)
		{
			case ChimneyTileTemplate.Catagory.ARMOUR:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/ArmourTileBackground", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
			case ChimneyTileTemplate.Catagory.WEAPON:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/WeaponTileBackground", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
			case ChimneyTileTemplate.Catagory.POTION:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/PotionTileBackground", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
			case ChimneyTileTemplate.Catagory.SKIPTOOL:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/SkiptoolTileBackground", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
			case ChimneyTileTemplate.Catagory.ENEMY:
				if (chimneyTileTemplateArray[tileObjects[index].GetComponent<ChimneyTile>().RandomTileTypeNum].enemySubCatagory == ChimneyTileTemplate.EnemySubCatagory.BOSS)
				{
					tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/BossTileBackground", typeof(GameObject)), transform) as GameObject;
					tileBackground[index].transform.SetParent(gameObject.transform);
				}
				else
				{
					tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/MonsterTileBackground", typeof(GameObject)), transform) as GameObject;
					tileBackground[index].transform.SetParent(gameObject.transform);
				}
				break;
			case ChimneyTileTemplate.Catagory.BOMB:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/TileBackgrounds/BombTileBackground", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
			default:
				tileBackground[index] = Instantiate(Resources.Load("Prefabs/Tile_Background", typeof(GameObject)), transform) as GameObject;
				tileBackground[index].transform.SetParent(gameObject.transform);
				break;
		}
	}

	void LoadShopChimney()
	{
		for (int i = 0; i < tileObjects.Length; i++)
		{
			tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum = ShopChimneyValues.RandomlyGeneratedTileIndex[i];
			tileObjects[i].GetComponent<ChimneyTile>().ConstTileValue = ShopChimneyValues.RandomlyGeneratedTileValue[i];
			tileObjects[i].GetComponent<ChimneyTile>().TileValue = ShopChimneyValues.RandomlyGeneratedTileValue[i];
			// Set tile backgrounds based on the category that tile belongs to
			Destroy(tileBackground[i]);
			SetTileBackgrounds(i);
			float scale = i * tileObjects[i].transform.localScale.y + i * spaceBetweenTiles;
			tileBackground[i].transform.localPosition = gameObject.transform.localPosition + new Vector3(0, -scale, 0);

			// Change the text depending on the current card value
			tileValueText[i].text = tileObjects[i].GetComponent<ChimneyTile>().TileValue.ToString();

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
		tileObjects[ShopChimneyValues.CurrentTilenumber].GetComponent<ChimneyTile>().Selected = true;
		currentlySelectedTile = tileObjects[ShopChimneyValues.CurrentTilenumber];
		currentTileNumber = ShopChimneyValues.CurrentTilenumber;

		// Set which tiles should be used depending on the visit number
		switch (ShopChimneyValues.NoOfShopChimneyVisits)
		{
			case 0:
				break;
			case 1:
				for (int i = 0; i < 3; i++)
				{
					tileObjects[i].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
					tileObjects[i].GetComponent<ChimneyTile>().TileUsed = true;
				}
				break;
			case 2:
				for (int i = 0; i < 6; i++)
				{
					tileObjects[i].GetComponent<SpriteRenderer>().sprite = chimneyTileTemplateArray[tileObjects[i].GetComponent<ChimneyTile>().RandomTileTypeNum].artwork;
					tileObjects[i].GetComponent<ChimneyTile>().TileUsed = true;
				}
				break;
			default:
				break;
		}
	}
}
