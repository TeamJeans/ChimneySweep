using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalendarMenu : MonoBehaviour {

	void CalendarBackButton()
	{
		SceneManager.LoadScene("MainMenuScene");
	}

	public void CalendarNextDayButton()
	{
        if (StaticValueHolder.CurrentDay != 8)
        {
            SceneManager.LoadScene("ChimneyScene");
        }
		
	}
}
