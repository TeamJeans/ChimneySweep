using UnityEngine;

public class TileManager : MonoBehaviour {

	[SerializeField]
	float spaceBetweenTiles = 100f;

	Vector3 firstTilePos;
	Vector3 tileSize;

	bool tilesGenerated = false;

	public GameObject[] tilePrefabs; //size gets set in inspector! drag prefabs in there!
	public GameObject[] tiles;

	// Use this for initialization
	void Start () {

		tiles = new GameObject[tilePrefabs.Length]; //makes sure they match length
		for (int i = 0; i < tilePrefabs.Length; i++)
		{
			tiles[i] = Instantiate(tilePrefabs[i]) as GameObject;
			tiles[i].transform.parent = gameObject.transform;
		}

		firstTilePos = tiles[0].transform.position;
		tileSize = tiles[0].transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

		if (!tilesGenerated)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				tiles[i].transform.position = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y - tiles[i].transform.localScale.y - spaceBetweenTiles * i, tiles[i].transform.position.z);
			}
			tilesGenerated = true;
		}

	}
}
