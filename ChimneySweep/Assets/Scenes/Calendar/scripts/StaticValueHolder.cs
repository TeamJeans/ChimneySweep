using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//holds values between scenes
public static class StaticValueHolder
{
    //initialise here
    private static int currentMoney, currentDay;

    public static int CurrentMoney
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = value;
        }
    }

    public static int CurrentDay
    {
        get
        {
            return currentDay;
        }
        set
        {
            currentDay = value;
        }
    }
}