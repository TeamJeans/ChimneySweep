using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayOverStatsMenu : MonoBehaviour {

	public void DayOverMenuCalendarButton()
	{
		// Sets the amount of money earned for the current day
		StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.CurrentMoney - StaticValueHolder.TotalMoney;

		Debug.Log("Pressed");
		SceneManager.LoadScene("CalendarScene");
	}

	public void DayOverNextDayButton()
	{
		// Sets the amount of money earned for the current day
		StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.CurrentMoney - StaticValueHolder.TotalMoney;
		if (StaticValueHolder.CurrentDay < 7)
		{
			StaticValueHolder.CurrentDay++;
		}
		SceneManager.LoadScene("ChimneyScene");
	}
}
