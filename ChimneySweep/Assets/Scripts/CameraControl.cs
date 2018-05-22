using UnityEngine;

public class CameraControl : MonoBehaviour {

	Transform mouseOrigin;
	Transform mouseNewPos;

	bool leftMouseDown;

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
			Debug.Log("Mouse0 has been pressed!");

		}
	}
}
