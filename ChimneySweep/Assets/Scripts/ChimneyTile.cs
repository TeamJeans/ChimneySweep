using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChimneyTile : MonoBehaviour
{
	[SerializeField]
	bool selected = false;
	public bool Selected { get { return selected; } }

	bool mouseOver = false;

	void Start()
	{

	}

	void OnMouseOver()
	{
		// Check if the user has tapped
		if (Input.GetMouseButtonDown(0))
		{
			selected = true;
		}
		if (!mouseOver)
		{
			mouseOver = true;
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !mouseOver)
		{
			selected = false;
		}
	}

	void OnMouseExit()
	{
		if (mouseOver)
		{
			mouseOver = false;
		}
	}
}
