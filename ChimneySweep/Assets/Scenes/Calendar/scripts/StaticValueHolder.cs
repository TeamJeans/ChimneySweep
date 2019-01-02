using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds values between scenes
public static class StaticValueHolder
{
    //initialise here
    private static int currentMoney = 0, currentDay = 0, currentWeek = 0, totalMoney = 0;
    private static int[] dayValues = new int[7];
	private static int hitPoints = 0, maxHitPoints = 0;

    public static int[] DayValues
    {
        get { return dayValues; }
        set { dayValues = value; }
    }

    public static int CurrentMoney
    {
        get { return currentMoney; }
        set { currentMoney = value; }
    }

    public static int CurrentDay
    {
        get { return currentDay; }
        set { currentDay = value; }
    }

    public static int CurrentWeek
    {
        get { return currentWeek; }
        set { currentWeek = value; }
    }

    public static int TotalMoney
    {
        get { return totalMoney; }
        set { totalMoney = value; }
    }

	public static int HitPoints
	{
		get { return hitPoints; }
		set { hitPoints = value; }
	}

	public static int MaxHitPoints
	{
		get { return maxHitPoints; }
		set { maxHitPoints = value; }
	}
}