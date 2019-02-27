using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayRent : MonoBehaviour {
    public int rent;
    public Text rentDue;

    public Animator moveCalendar;
    public Animator moveBackground;
    float countUp;

    IEnumerator CountTotalUp()
    {
        //print out each letter with slight delay to give typing effect
        for (int i = 0; i < StaticValueHolder.DailyMoney + 1; i++)
        {
            Debug.Log("Looping, money is: " + StaticValueHolder.DailyMoney + "      Count is: " +  countUp);
            rentDue.text = StaticValueHolder.TotalMoney - StaticValueHolder.DailyMoney + countUp + "/" + rent;
            countUp++;
            yield return new WaitForSeconds(countUp / 900);
        }
    }


    // Use this for initialization
    public void Start()
    {
        //used to increase rent
        rent = 200 + StaticValueHolder.CurrentWeek * 200;
        //show how much rent is due at end of week
        StartCoroutine(CountTotalUp());
    }

    public void PayRentFunc()
    {
        //if time to pay rent
        if ((float)StaticValueHolder.CurrentDay == 8)
        {
            moveBackground.SetBool("payingRentTransition", true);
            moveCalendar.SetBool("payingRentTransition", true);

            //if enough money to pay
            if (StaticValueHolder.TotalMoney >= rent)
            {
                StaticValueHolder.TotalMoney -= rent;
                //reset days at end of the week
                StaticValueHolder.CurrentDay = 1;
                StaticValueHolder.CurrentWeek += 1;
                for (int i = 0; i < 8; i++) StaticValueHolder.DayValues[i] = 0;
            }
            //if not enough to pay rent
            else
            {
                Debug.Log("GameOver");
            }
        }
    }
}


