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
	float lengthOfSide = 100f;
	[SerializeField]
	Vector3 firstSidePos;

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
			sides[i].transform.position = firstSidePos;
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
				sides[i].transform.position = new Vector3(sides[i].transform.position.x, sides[i].transform.position.y - lengthOfSide * i , sides[i].transform.position.z);
			}
			panelTransform.offsetMin = new Vector2(panelTransform.offsetMin.x, -35f * sides.Length);
			//panelTransform.offsetMax = new Vector2(panelTransform.offsetMax.x, 17.5f * sides.Length/3);
			tilesGenerated = true;
		}

	}
}
