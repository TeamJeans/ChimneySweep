using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Swipe swipeControls;
	private Vector3 desiredPosition;

	private void Update()
	{
		if (swipeControls.SwipeDown)
			desiredPosition += Vector3.up;

		if (swipeControls.SwipeUp)
			desiredPosition += Vector3.down;

		transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 100f * Time.deltaTime);
	}
}
