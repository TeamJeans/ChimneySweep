using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Required when Using UI elements.
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalendarManager : MonoBehaviour {

    public GameObject[] calDays;
    public GameObject dayDone;

    public Text rentDue;
    public Text Gold1;
    public Text Gold2;
    public Text Gold3;
    public Text Gold4;
    public Text Gold5;
    public Text Gold6;
    public Text Gold7;

    public int day;
    public float dayCheck;
    public int week;
    public int rent;
    public bool rentPaid;

    // Use this for initialization
    void Start()
    {
        //reset days at the end of the week
        if (StaticValueHolder.CurrentDay > 7)
        {
            StaticValueHolder.CurrentDay = 0;
            StaticValueHolder.CurrentWeek += 1;
            for (int i = 0; i < 7; i++)
            {
                StaticValueHolder.DayValues[i] = 0;
            }
        }

        if (StaticValueHolder.CurrentDay < 0) StaticValueHolder.CurrentDay = 0;


        //add one to the days-------------------------------------------------------give it to aidans script
        //show each days money
        StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.DailyMoney;

        
        
       

        //reset the check for rentPaid at start of scene
        rentPaid = false;
        //gets the current day and checks if end of week
        dayCheck = StaticValueHolder.CurrentDay / 7;
        //used to increase rent
        rent = 200 + week * 200;
        //show how much rent is due at end of week
        rentDue.text = StaticValueHolder.TotalMoney + "/" + rent;

        Gold1.text = StaticValueHolder.DayValues[0] + ""; //for some reason there has to be a string in here or it doesnt work so leave the empty string
        Gold2.text = StaticValueHolder.DayValues[1] + "";
        Gold3.text = StaticValueHolder.DayValues[2] + "";
        Gold4.text = StaticValueHolder.DayValues[3] + "";
        Gold5.text = StaticValueHolder.DayValues[4] + "";
        Gold6.text = StaticValueHolder.DayValues[5] + "";
        Gold7.text = StaticValueHolder.DayValues[6] + "";



        //cross off completed days
        for (int i = 0; i < StaticValueHolder.CurrentDay; i++)
        {
            GameObject myDayDone = Instantiate(dayDone, calDays[i].transform.position, calDays[i].transform.rotation);
            myDayDone.transform.SetParent(GameObject.Find("/UIOverlay/Calendar").transform);
        }

        StaticValueHolder.CurrentDay += 1;
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
            if (StaticValueHolder.DailyMoney >= rent)
            {
                StaticValueHolder.DailyMoney -= rent;
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
