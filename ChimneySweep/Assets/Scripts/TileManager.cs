using UnityEngine;

public class TileManager : MonoBehaviour {

	[SerializeField]
	ChimneyTile[] tiles;

	public float spaceBetweenTiles = 100f;
	Vector3 firstTilePos;
	Vector3 tileSize;

	bool tilesGenerated = false;

	// Use this for initialization
	void Start () {
		firstTilePos = tiles[0].transform.position;
		tileSize = tiles[0].transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

		if (!tilesGenerated)
		{
			for (int i = 0; i < tiles.Length; i++)
			{
				tiles[i].transform.position = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y + tiles[i].transform.localScale.y + spaceBetweenTiles, tiles[i].transform.position.z);
			}
		}

	}
}
