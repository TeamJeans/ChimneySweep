using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Required when Using UI elements.
using UnityEngine;

public class CalendarManager : MonoBehaviour {

    public Text rentDue;
    public int day;
    public float dayCheck;
    public int week;
    public int rent;
    public bool rentPaid;

    // Use this for initialization
    void Start()
    {
        //reset the check for rentPaid at start of scene
        rentPaid = false;
        //gets the current day and checks if end of week
        day = StaticValueHolder.CurrentDay;
        dayCheck = StaticValueHolder.CurrentDay / 7;
        //used to increase rent
        week = day / 7;
        rent = 50 + week * 50;
        //show how much rent is due at end of week
        rentDue.text = "Rent Due: " + "\u00A3" + rent;
    }


	// Update is called once per frame
	void Update ()
    {
        //if time to pay rent
        if ((float)day / 7 == dayCheck && !rentPaid)
        {
            //if enough money to pay
            if (StaticValueHolder.CurrentMoney >= rent)
            {
                StaticValueHolder.CurrentMoney -= rent;
                rentPaid = true;
                Debug.Log("Rent Paid: " + rentPaid);
                
            }
            //if not enough to pay rent
            else
            {
                Debug.Log("GameOver");
            }
        }
    }
}
