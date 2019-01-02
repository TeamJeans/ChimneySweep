﻿using System.Collections;
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
		Debug.Log("Pressed");
		SceneManager.LoadScene("CalendarScene");
	}

	public void DayOverNextDayButton()
	{
		// Sets the amount of money earned for the current day
		StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.DailyMoney;
		if (StaticValueHolder.CurrentDay < 7)
		{
			StaticValueHolder.CurrentDay++;
		}
		SceneManager.LoadScene("ChimneyScene");
	}
}
