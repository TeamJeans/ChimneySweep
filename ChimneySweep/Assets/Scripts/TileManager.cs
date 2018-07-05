using UnityEngine;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;
	[SerializeField]
	Swipe swipeControls;
	[SerializeField]
	GameObject currentlySelectedTile;
	public GameObject CurrentlySelectedTile { get { return currentlySelectedTile; } }

	[SerializeField]
	float tileSwipeSpeed = 0.125f;
	Vector2 mousePosition;

	Vector3 firstTilePos;
	Vector3 tileSize;

	[SerializeField]
	bool tileDragMode = false;
	public bool TileDragMode{get{ return tileDragMode; }}
	bool tilesGenerated = false;

	public GameObject[] tilePrefabs;
	public GameObject[] tiles;

	// Use this for initialization
	void Start () {

		tiles = new GameObject[tilePrefabs.Length];
		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			tiles[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tiles[i].transform.parent = gameObject.transform;
		}

		firstTilePos = tiles[0].transform.position;
		tileSize = tiles[0].transform.localScale;
		currentlySelectedTile = tiles[0];
	}
	
	// Update is called once per frame
	void Update () {

		// Generate tiles
		if (!tilesGenerated)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				tiles[i].transform.position = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y - tiles[i].transform.localScale.y - spaceBetweenTiles * i, tiles[i].transform.position.z);
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
		if (swipeControls.SwipeLeft || swipeControls.SwipeRight)
		{
			tileDragMode = true;
		}

		if (!Input.GetMouseButton(0))
		{
			currentlySelectedTile.transform.position = new Vector3(0, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
			tileDragMode = false;
		}

		if (tileDragMode)
		{
			mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
			currentlySelectedTile.transform.position = new Vector3(mousePosition.x, currentlySelectedTile.transform.position.y, currentlySelectedTile.transform.position.z);
		}
	}
}
