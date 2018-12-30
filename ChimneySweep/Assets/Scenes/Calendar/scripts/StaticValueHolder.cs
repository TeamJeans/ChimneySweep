﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds values between scenes
public static class StaticValueHolder
{
    //initialise here
    private static int currentMoney, currentDay, currentWeek, totalMoney;
    private static int[] dayValues = new int[7];


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
}