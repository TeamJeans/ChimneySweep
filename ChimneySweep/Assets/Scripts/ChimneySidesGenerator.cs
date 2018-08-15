using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySidesGenerator : MonoBehaviour {

	public float spaceBetweenChimneySides = 100f;

	[SerializeField]
	RectTransform panelTransform;
	[SerializeField]
	TileManager tileManager;
	[SerializeField]
	Vector3 firstSidePos;

	[SerializeField]
	GameObject chimneyHearth;

	bool tilesGenerated = false;
	public GameObject[] sides;
	public GameObject[] chimneyBackgroundTiles;

	// Use this for initialization
	void Start()
	{
		sides = new GameObject[(tileManager.tilePrefabs.Length - 2) / 2]; //makes sure they match length
		chimneyBackgroundTiles = new GameObject[(tileManager.tilePrefabs.Length - 2) / 2];
		for (int i = 0; i < sides.Length; i++)
		{
			sides[i] = Instantiate(Resources.Load("Prefabs/Chimney_Sides", typeof(GameObject)), transform) as GameObject;
			sides[i].transform.parent = gameObject.transform;
			sides[i].transform.position = firstSidePos;
			chimneyBackgroundTiles[i] = Instantiate(Resources.Load("Prefabs/Chimney_Background", typeof(GameObject)), transform) as GameObject;
			chimneyBackgroundTiles[i].transform.parent = gameObject.transform;
			chimneyBackgroundTiles[i].transform.position = firstSidePos;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Generate the sides
		if (!tilesGenerated)
		{
			for (int i = 1; i < sides.Length; i++)
			{
				sides[i].transform.position = new Vector3(sides[i].transform.position.x, sides[i].transform.position.y - sides[i].transform.localScale.y * i - spaceBetweenChimneySides*i , sides[i].transform.position.z);
				chimneyBackgroundTiles[i].transform.position = new Vector3(chimneyBackgroundTiles[i].transform.position.x, chimneyBackgroundTiles[i].transform.position.y - chimneyBackgroundTiles[i].transform.localScale.y * i - spaceBetweenChimneySides * i, chimneyBackgroundTiles[i].transform.position.z);
			}
			panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -sides.Length * sides[0].transform.localScale.y - 20f);
			tilesGenerated = true;

			chimneyHearth.transform.position = new Vector3(chimneyHearth.transform.position.x, sides[sides.Length -1].transform.position.y - sides[sides.Length - 1].transform.localScale.y - spaceBetweenChimneySides, chimneyHearth.transform.position.z);
		}

	}
}
