using UnityEngine;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;
	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } }


	Vector2 mousePosition;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; }}
	bool tilesGenerated = false;
	bool endDayMenuEnabled = false;

	public GameObject[] tilePrefabs;
	public GameObject[] tiles;

	// Use this for initialization
	void Start () {

		GameMaster.gm.onToggleEndDayMenu += OnEndDayMenuToggle;

		tiles = new GameObject[tilePrefabs.Length];
		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			tiles[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tiles[i].transform.parent = gameObject.transform;
		}
		currentlySelectedTile = tiles[0];
	}
	
	void OnEndDayMenuToggle(bool active)
	{
		endDayMenuEnabled = active;
	}

	// Update is called once per frame
	void Update () {

		// Generate tiles
		if (!tilesGenerated)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				tiles[i].transform.position = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y - tiles[i].transform.localScale.y * i - spaceBetweenTiles * i, tiles[i].transform.position.z);
			}
			tilesGenerated = true;
		}

		// Find the selected tile
		for (int i = 0; i < tiles.Length; i++)
		{
			ChimneyTile tempTile = tiles[i].GetComponent(typeof(ChimneyTile)) as ChimneyTile;
			if (tempTile.Selected)
			{
				currentlySelectedTile = tiles[i];
			}
		}

		// If the user has dragged left or right, drag the tile in that direction
		if (swipeControls.SwipeLeft || swipeControls.SwipeRight && !endDayMenuEnabled)
		{
			ChimneyTile tileScript = currentlySelectedTile.GetComponent(typeof(ChimneyTile)) as ChimneyTile;
			if (tileScript.MouseOver)
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
