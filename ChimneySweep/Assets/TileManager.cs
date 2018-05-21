using UnityEngine;

[System.Serializable]
public class ChimneyTile
{
	public string name;
}

public class TileManager : MonoBehaviour {

	[SerializeField]
	ChimneyTile[] tiles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
