using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayOverStatsMenu : MonoBehaviour {

	[SerializeField]
	Text moneyText;
	[SerializeField]
	Text hpText;

	void Start()
	{
		moneyText.text = "Money: \u00A3" + StaticValueHolder.DailyMoney;
		hpText.text = "HP: " + (StaticValueHolder.HitPoints) + "/" + (StaticValueHolder.MaxHitPoints);
	}

	public void DayOverMenuCalendarButton()
	{
		StaticValueHolder.TotalMoney += StaticValueHolder.DailyMoney;
		Debug.Log("Pressed");
		SceneManager.LoadScene("CalendarScene");
	}

	public void DayOverNextDayButton()
	{
		// Sets the amount of money earned for the current day
		StaticValueHolder.TotalMoney += StaticValueHolder.DailyMoney;
		StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.DailyMoney;
		if (StaticValueHolder.CurrentDay < 7) StaticValueHolder.CurrentDay++;

        //load next day unless its the end of the week in which case force back to calendar to pay rent
        if (StaticValueHolder.CurrentDay == 7) SceneManager.LoadScene("CalendarScene");
        else SceneManager.LoadScene("ChimneyScene");
	}
}
