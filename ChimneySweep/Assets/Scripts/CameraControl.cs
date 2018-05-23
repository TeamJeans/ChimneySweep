using UnityEngine;

public class CameraControl : MonoBehaviour {

	Vector2 mouseOrigin;
	Vector2 mouseNewPos;

	bool leftMouseDown = false;
	bool dragOriginSet = false;

	float elapsedDragTime = 0f;
	float dragDistance = 0f;
	float dragSpeed = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// Camera is set to look at the top tile

		// If the player left clicks/touches the screen and moves the mouse/their finger, the camera will move in the opposite direction of the drag

		leftMouseDown = Input.GetKey(KeyCode.Mouse0);
		if (leftMouseDown)
		{
			if (!dragOriginSet)
			{
				dragOriginSet = true;
				mouseOrigin = Input.mousePosition;
			}
			mouseNewPos = Input.mousePosition;
			elapsedDragTime += Time.deltaTime;
			dragDistance = mouseNewPos.y - mouseOrigin.y;
			dragSpeed = dragDistance / elapsedDragTime;
			transform.position.Set(transform.position.x, transform.position.y + dragSpeed, transform.position.z);
		}
		else
		{
			dragOriginSet = false;
		}
	}
}
