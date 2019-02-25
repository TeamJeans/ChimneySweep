using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayRent : MonoBehaviour {
    public int rent;
    public Text rentDue;

    // Use this for initialization
    public void Start()
    {
        //used to increase rent
        rent = 200 + StaticValueHolder.CurrentWeek * 200;
        //show how much rent is due at end of week
        rentDue.text = StaticValueHolder.TotalMoney + "/" + rent;


        //if time to pay rent
        if ((float)StaticValueHolder.CurrentDay == 8)
        {
            //if enough money to pay
            if (StaticValueHolder.TotalMoney >= rent)
            {
                StaticValueHolder.TotalMoney -= rent;
                //reset days at end of the week
                StaticValueHolder.CurrentDay = 1;
                StaticValueHolder.CurrentWeek += 1;
                for (int i = 0; i < 8; i++)
                {
                    StaticValueHolder.DayValues[i] = 0;
                }
            }
            //if not enough to pay rent
            else
            {
                Debug.Log("GameOver");
            }
        }
    }
}
