using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dayOverDisplay : MonoBehaviour {

    public TextMeshProUGUI DayEnd;

	// Use this for initialization
	void Start ()
    {
        DayEnd.text = "End of day\n" + StaticValueHolder.CurrentDay;
    }
}
