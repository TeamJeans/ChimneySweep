using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Required when Using UI elements.
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalendarManager : MonoBehaviour {

    public GameObject[] calDays;
    public GameObject dayDone;

    public int currentMoney;
    public Text rentDue;
    public int day;
    public float dayCheck;
    public int week;
    public int rent;
    public bool rentPaid;

    // Use this for initialization
    void Start()
    {
        currentMoney = StaticValueHolder.CurrentMoney;

        StaticValueHolder.TotalMoney += currentMoney;

        //add one to the days-------------------------------------------------------give it to aidans script
        StaticValueHolder.CurrentDay += 1;

        //reset the check for rentPaid at start of scene
        rentPaid = false;
        //gets the current day and checks if end of week
        dayCheck = StaticValueHolder.CurrentDay / 7;
        //used to increase rent
        rent = 50 + week * 50;
        //show how much rent is due at end of week
        rentDue.text = "Rent Due: " + "\u00A3" + rent;

        //reset days at the end of the week
        if (StaticValueHolder.CurrentDay > 7)
        {
            StaticValueHolder.CurrentDay = 0;
            StaticValueHolder.CurrentWeek += 1;
        }

        //cross off completed days
        for (int i = 0; i < StaticValueHolder.CurrentDay; i++)
        {
            GameObject myDayDone = Instantiate(dayDone, calDays[i].transform.position, calDays[i].transform.rotation);
            myDayDone.transform.SetParent(GameObject.Find("/UIOverlay/Calendar").transform);
        }
    }


	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("day: " + day);
        //Debug.Log("Money: " + StaticValueHolder.CurrentMoney);
        //if time to pay rent
        if ((float)StaticValueHolder.CurrentDay / 7 == dayCheck && !rentPaid && StaticValueHolder.CurrentDay > 0)
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
        //TODO add day for testing, remove later --------------------------------------------------------
        if(Input.GetKeyUp("a"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
