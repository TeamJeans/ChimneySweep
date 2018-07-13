using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayOverStatsMenu : MonoBehaviour {

	public void DayOverMenuCalendarButton()
	{
		Debug.Log("Pressed");
		SceneManager.LoadScene("CalendarScene");
	}

	public void DayOverNextDayButton()
	{
		SceneManager.LoadScene("ChimneyScene");
	}
}
