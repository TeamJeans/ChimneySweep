using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	TileManager tileManager;
	ChimneyTile selectedTile;

	[SerializeField]
	GameObject chimney;

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
			Vector2 smoothedPos = Vector2.Lerp(transform.localPosition, desiredPos, smoothSpeed);
			transform.localPosition = new Vector3(transform.localPosition.x,smoothedPos.y, transform.localPosition.z);
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
			desiredPos.y = (tileManager.CurrentTileNumber - (tileManager.TileObjects.Length / 2)) * ((tileManager.TileObjects[0].transform.localScale.y) + (tileManager.SpaceBetweenTiles));
			desiredPos = new Vector2(desiredPos.x, desiredPos.y);
		}
		else
		{
			desiredPos.y = -1 * (tileManager.TileObjects.Length / 2 - tileManager.CurrentTileNumber) * ((tileManager.TileObjects[0].transform.localScale.y) + (tileManager.SpaceBetweenTiles));
			desiredPos = new Vector2(desiredPos.x, desiredPos.y);
		}
	}
}
