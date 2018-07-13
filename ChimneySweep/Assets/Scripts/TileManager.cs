using UnityEngine;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;
	[SerializeField]
	Swipe swipeControls;

	Vector2 mousePosition;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; } set { tileDragMode = value; } }
	bool tilesGenerated = false;
	bool endDayMenuEnabled = false;
	bool tileSelected = false;

	public GameObject[] tilePrefabs;
	public GameObject[] tileObjects;
	public ChimneyTileTemplate[] chimneyTileTemplate;

	[SerializeField]
	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } set { currentlySelectedTile = value; } }
	[SerializeField]
	int currentTileNumber = 0;
	public int CurrentTileNumber{ get { return currentTileNumber; } }

	[SerializeField]
	GameObject tileGlow;

	// Use this for initialization
	void Start () {

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
		}
		// Sets the selected tile to be the first tile generated
		currentlySelectedTile = tileObjects[0];
	}
	
	void OnEndDayMenuToggle(bool active)
	{
		endDayMenuEnabled = active;
	}

	// Update is called once per frame
	void Update ()
	{


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

		// Make the tile glow render only when a tile is selected
			if (tileSelected)
			{
				tileGlow.GetComponent<SpriteRenderer>().enabled = true;
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
}
