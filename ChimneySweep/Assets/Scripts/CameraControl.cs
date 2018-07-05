using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	TileManager tileManager;

	[SerializeField]
	float smoothSpeed = 0.125f;

	//public float SmoothSpeed { get { return smoothSpeed; } }

	// Update is called once per frame
	void LateUpdate()
	{
		Vector2 desiredPos = tileManager.CurrentlySelectedTile.transform.position;
		Vector2 smoothedPos = Vector2.Lerp(transform.position, desiredPos, smoothSpeed);
		transform.position = new Vector3(transform.position.x, smoothedPos.y, transform.position.z);
	}
}
