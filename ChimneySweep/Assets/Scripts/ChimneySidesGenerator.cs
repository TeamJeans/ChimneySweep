using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySidesGenerator : MonoBehaviour {

	public float spaceBetweenChimneySides;

	[SerializeField]
	RectTransform panelTransform;
	[SerializeField]
	TileManager tileManager;
	[SerializeField]
	Vector3 firstSidePos;
	Vector3 sidesScale = new Vector3(12.5f, 12.5f, 0);

	[SerializeField]
	GameObject chimneyHearth;
	public GameObject ChimneyHearth { get { return chimneyHearth; } }
	[SerializeField]
	GameObject chimneyHearthBackground;

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
			sides[i].transform.SetParent(gameObject.transform);
			sides[i].transform.position = firstSidePos + new Vector3(0, - i * sides[i].transform.localScale.y, 0);

			chimneyBackgroundTiles[i] = Instantiate(Resources.Load("Prefabs/Chimney_Background", typeof(GameObject)), transform) as GameObject;
			chimneyBackgroundTiles[i].transform.parent = gameObject.transform;
			chimneyBackgroundTiles[i].transform.position = firstSidePos + new Vector3(0, -i * sides[i].transform.localScale.y, 0);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Generate the sides
		if (!tilesGenerated)
		{
			// Changes how far you can scroll down the chimney
			panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -sides.Length * sides[0].transform.localScale.y - 25f);
			tilesGenerated = true;

			chimneyHearth.transform.position = new Vector3(chimneyHearth.transform.position.x, sides[sides.Length -1].transform.position.y - sides[sides.Length - 1].transform.localScale.y - spaceBetweenChimneySides, chimneyHearth.transform.position.z);
			chimneyHearthBackground.transform.position = new Vector3(chimneyHearth.transform.position.x, chimneyHearth.transform.position.y, chimneyHearth.transform.position.z);
		}
	}
}
