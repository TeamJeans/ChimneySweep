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
		if (selectedTile.Selected)
		{
			Vector2 smoothedPos = Vector2.Lerp(transform.position, desiredPos, smoothSpeed);
			transform.position = new Vector3(transform.position.x,smoothedPos.y, transform.position.z);
		}
	}

	public void SetDesiredCamPos()
	{
		desiredPos = tileManager.CurrentlySelectedTile.transform.position;
		desiredPos = new Vector2(desiredPos.x, desiredPos.y - 65);
		selectedTile = tileManager.CurrentlySelectedTile.GetComponent(typeof(ChimneyTile)) as ChimneyTile;
	}
}
