using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public static GameMaster gm;

	[SerializeField]
	TileSwipeLeft tileSwipeLeft;

	[SerializeField]
	GameObject endDayMenu;


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
	}

	public void ToggleEndDayMenu()
	{
		endDayMenu.SetActive(!endDayMenu.activeSelf);
		onToggleEndDayMenu.Invoke(endDayMenu.activeSelf);
	}
}
