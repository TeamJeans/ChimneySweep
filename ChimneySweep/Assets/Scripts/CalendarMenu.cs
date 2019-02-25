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
		SceneManager.LoadScene("ChimneyScene");
	}
}
