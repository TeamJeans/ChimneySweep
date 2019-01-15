using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimneyTextureChanger : MonoBehaviour {

	// References to other scripts
	[SerializeField]
	ChimneySidesGenerator chimneySidesGenerator;

	// Chimney parts
	[SerializeField]
	GameObject chimneyTop;
	[SerializeField]
	GameObject chimneyTopBackground;
	[SerializeField]
	GameObject[] chimneySides;
	[SerializeField]
	GameObject[] chimneyBackground;
	[SerializeField]
	GameObject chimneyHearth;
	[SerializeField]
	GameObject chimneyHearthBackground;

	// Sprite arrays
	Sprite[] selectedChimney = new Sprite[6];
	[SerializeField]
	Sprite[] orangeChimney = new Sprite[6];
	[SerializeField]
	Sprite[] greyChimney = new Sprite[6];
	[SerializeField]
	Sprite[] blueChimney = new Sprite[6];

	[SerializeField]
	bool randomChimney;

	// Enums
	public enum ChimneyType
	{
		ORANGE,
		GREY,
		BLUE,
		NUM_OF_TYPES
	}

	[SerializeField]
	ChimneyType currentChimneyType;
	public ChimneyType CurrentChimneyType { get { return currentChimneyType; } }
	enum ChimneyPart
	{
		TOP = 0,
		TOP_BACKGROUND = 1,
		SIDES = 2,
		MAIN_BACKGROUND = 3,
		HEARTH = 4,
		HEARTH_BACKGROUND = 5
	}

	int randomTypeNo = 0;
	// Use this for initialization
	void Start ()
	{
		if (randomChimney)
		{
			randomTypeNo = Random.Range(0, (int)ChimneyType.NUM_OF_TYPES);
			currentChimneyType = (ChimneyType)randomTypeNo;
		}
		SetChimneyType(currentChimneyType);
	}

	public void SetChimneyType(ChimneyType ct)
	{
		switch (ct)
		{
			case ChimneyType.ORANGE:
				selectedChimney = orangeChimney;
				break;
			case ChimneyType.GREY:
				selectedChimney = greyChimney;
				break;
			case ChimneyType.BLUE:
				selectedChimney = blueChimney;
				break;
			default:
				break;
		}

		chimneyTop.GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.TOP];
		chimneyTopBackground.GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.TOP_BACKGROUND];
		chimneySides = chimneySidesGenerator.sides;
		chimneyBackground = chimneySidesGenerator.chimneyBackgroundTiles;
		for (int i = 0; i < chimneySidesGenerator.sides.Length; i++)
		{
			chimneySides[i].GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.SIDES];
			chimneyBackground[i].GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.MAIN_BACKGROUND];
		}
		chimneyHearth.GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.HEARTH];
		chimneyHearthBackground.GetComponent<SpriteRenderer>().sprite = selectedChimney[(sbyte)ChimneyPart.HEARTH_BACKGROUND];
	}
}
