using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneySidesGenerator : MonoBehaviour {

	public float spaceBetweenChimneySides = 100f;

	[SerializeField]
	private TileManager tileManager;

	Vector3 firstSidePos;
	Vector3 sideSize;

	bool tilesGenerated = false;

	//public GameObject[] sidePrefabs; //size gets set in inspector! drag prefabs in there!
	public GameObject[] sides;

	// Use this for initialization
	void Start()
	{
		sides = new GameObject[(tileManager.tilePrefabs.Length - 2) / 2]; //makes sure they match length
		for (int i = 0; i < sides.Length; i++)
		{
			sides[i] = Instantiate(Resources.Load("Prefabs/Chimney_Sides", typeof(GameObject)), transform) as GameObject;
			sides[i].transform.parent = gameObject.transform;
		}

		firstSidePos = sides[0].transform.position;
		sideSize = sides[0].transform.localScale;
	}

	// Update is called once per frame
	void Update()
	{

		if (!tilesGenerated)
		{
			for (int i = 1; i < sides.Length -1; i++)
			{
				sides[i].transform.position = new Vector3(sides[i].transform.position.x, (sides[i].transform.position.y - sides[i].transform.localScale.y) * i, sides[i].transform.position.z);
			}
			tilesGenerated = true;
		}

	}
}
