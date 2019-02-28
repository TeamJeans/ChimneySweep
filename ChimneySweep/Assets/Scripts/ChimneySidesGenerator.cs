using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySidesGenerator : MonoBehaviour {

	public float spaceBetweenChimneySides;

	[SerializeField]
	ChimneyTextureChanger chimneyTextureChanger;
	[SerializeField]
	RectTransform canvasRect;

	[SerializeField]
	RectTransform panelTransform;
	[SerializeField]
	TileManager tileManager;

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
		sides = new GameObject[(tileManager.TilePrefabs.Length - 2) / 2]; //makes sure they match length
		chimneyBackgroundTiles = new GameObject[(tileManager.TilePrefabs.Length - 2) / 2];
		for (int i = 0; i < sides.Length; i++)
		{
			sides[i] = Instantiate(Resources.Load("Prefabs/Chimney_Sides", typeof(GameObject)), transform) as GameObject;
			sides[i].transform.SetParent(gameObject.transform);
			float scale = i * sides[i].transform.localScale.y;
			sides[i].transform.localPosition = sides[0].transform.localPosition + new Vector3(0, -scale, 0);

			chimneyBackgroundTiles[i] = Instantiate(Resources.Load("Prefabs/Chimney_Background", typeof(GameObject)), transform) as GameObject;
			chimneyBackgroundTiles[i].transform.parent = gameObject.transform;
			chimneyBackgroundTiles[i].transform.localPosition = chimneyBackgroundTiles[0].transform.localPosition + new Vector3(0, -scale, 0);
		}

		//chimneyTextureChanger.SetChimneyType(chimneyTextureChanger.CurrentChimneyType);
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

			float scale = (sides.Length) * sides[sides.Length - 1].transform.localScale.y;
			chimneyHearth.transform.localPosition = sides[0].transform.localPosition + new Vector3(0, -scale, 90);
			chimneyHearthBackground.transform.localPosition = sides[0].transform.localPosition + new Vector3(0, -scale, 90);
		}
	}
}
