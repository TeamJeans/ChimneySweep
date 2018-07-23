using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	TileSwipeLeft tileSwipeLeft;

	[SerializeField]
	TileSwipeRight tileSwipeRight;

	[SerializeField]
	TileManager tileManager;

	[SerializeField]
	GameObject endDayMenu;

	[SerializeField]
	CameraControl cameraControl;

	[SerializeField]
	Inventory inventory;


	void Awake()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
		}
	}

	public delegate void EndDayMenuCallBack(bool active);
	public EndDayMenuCallBack onToggleEndDayMenu;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (tileSwipeLeft.CollisionWithTile && !endDayMenu.activeSelf)
		{
			ToggleEndDayMenu();
			tileSwipeLeft.CollisionWithTile = false;
		}

		if (tileSwipeRight.CollisionWithTile)
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (tileManager.tileObjects[tileManager.CurrentTileNumber + 1] != null)
				{
					// If the inventory is not full and this item is storable, add it to the next empty slot in the inventory
					if (inventory.IsThereSpace() && tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum].Storable)
					{
						inventory.AddItem(tileManager.chimneyTileTemplate[tileManager.CurrentlySelectedTile.GetComponent<ChimneyTile>().RandomTileTypeNum]);
					}

					// Move to the next tile in the queue
					tileManager.MoveToNextTile();

					// Change where the camera goes to when the next tile is selected
					cameraControl.SetDesiredCamPos();
				}
				tileSwipeRight.CollisionWithTile = false;
			}
		}
	}

	public void ToggleEndDayMenu()
	{
		endDayMenu.SetActive(!endDayMenu.activeSelf);
		onToggleEndDayMenu.Invoke(endDayMenu.activeSelf);
	}

	public void ChangeSceneToDayOverStats()
	{
		SceneManager.LoadScene("DayOverStatsScene");
	}
}
