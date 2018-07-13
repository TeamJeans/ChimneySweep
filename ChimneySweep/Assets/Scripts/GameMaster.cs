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
			if (tileManager.tileObjects[tileManager.CurrentTileNumber + 1] != null)
			{
				tileManager.CurrentlySelectedTile.transform.position = new Vector3(0, tileManager.CurrentlySelectedTile.transform.position.y, tileManager.CurrentlySelectedTile.transform.position.z);
				tileManager.TileDragMode = false;
				tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().Selected = false;
				tileManager.tileObjects[tileManager.CurrentTileNumber].GetComponent<ChimneyTile>().TileUsed = true;
				tileManager.tileObjects[tileManager.CurrentTileNumber + 1].GetComponent<ChimneyTile>().Selected = true;
				cameraControl.SetDesiredCamPos();
			}
			tileSwipeRight.CollisionWithTile = false;
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
