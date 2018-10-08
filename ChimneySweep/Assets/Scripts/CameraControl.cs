using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	TileManager tileManager;
	ChimneyTile selectedTile;

	[SerializeField]
	RectTransform panelTransform;

	[SerializeField]
	float smoothSpeed = 0.125f;
	Vector2 desiredPos;

	void Start()
	{
		SetDesiredCamPos();
	}

	void LateUpdate()
	{
		selectedTile = tileManager.CurrentlySelectedTile.GetComponent(typeof(ChimneyTile)) as ChimneyTile;
		if (selectedTile.Selected)
		{
			Vector2 smoothedPos = Vector2.Lerp(transform.position, desiredPos, smoothSpeed);
			transform.position = new Vector3(transform.position.x,smoothedPos.y, transform.position.z);
		}
		else
		{
			SetDesiredCamPos();
		}
	}

	public void SetDesiredCamPos()
	{
		if (tileManager.CurrentTileNumber > (tileManager.TileObjects.Length/2))
		{
			desiredPos.y = 2 * (tileManager.CurrentTileNumber - (tileManager.TileObjects.Length / 2)) * ((tileManager.TileObjects[0].transform.localScale.y/2) + (tileManager.SpaceBetweenTiles / 2));
			desiredPos = new Vector2(desiredPos.x, desiredPos.y);
		}
		else
		{
			desiredPos.y = -2 * (tileManager.TileObjects.Length / 2 - tileManager.CurrentTileNumber) * ((tileManager.TileObjects[0].transform.localScale.y/2) + (tileManager.SpaceBetweenTiles / 2));
			desiredPos = new Vector2(desiredPos.x, desiredPos.y);
		}
	}
}
