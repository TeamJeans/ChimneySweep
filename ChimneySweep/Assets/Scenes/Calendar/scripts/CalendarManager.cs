using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;  // Required when Using UI elements.
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CalendarManager : MonoBehaviour {

    public GameObject[] calDays;
    public GameObject[] calGold;
    public GameObject dayDone;
    public GameObject countUpDone;

    public Text Gold1;
    public Text Gold2;
    public Text Gold3;
    public Text Gold4;
    public Text Gold5;
    public Text Gold6;
    public Text Gold7;

    public float countUp;
    Vector3 UIBudge;

    IEnumerator Type()
    {
        UIBudge.x = 0;
        UIBudge.y = 0;
        UIBudge.z = -10;

        //print out each letter with slight delay to give typing effect
        for (int i = 0; i < StaticValueHolder.DailyMoney+1; i++)
        {
            //get the current day and only count up on that day
            switch (StaticValueHolder.CurrentDay-1)
            {
                case 0:
                    {
                        Gold7.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 1:
                    {
                        Gold1.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 2:
                    {
                        Gold2.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 3:
                    {
                        Gold3.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 4:
                    {
                        Gold4.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 5:
                    {
                        Gold5.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 6:
                    {
                        Gold6.text = countUp + "";
                        countUp++;
                        break;
                    }
                case 7:
                    {
                        Gold7.text = countUp + "";
                        countUp++;
                        break;
                    }
                default:
                    break;
            }
            yield return new WaitForSeconds(countUp /900);
        }

        //particle when done counting up appears over number of gold of day
        if (StaticValueHolder.CurrentDay - 2 >= 0)
        {
            GameObject CountUpDone = Instantiate(countUpDone, calGold[StaticValueHolder.CurrentDay - 2].transform.position + UIBudge, calGold[StaticValueHolder.CurrentDay - 2].transform.rotation);
            CountUpDone.transform.SetParent(GameObject.Find("/UIOverlay/Calendar").transform);
        }

        //particle when done for day 7 since its treated as day 0
        if (Gold7.text != "0")
        {
            GameObject CountUpDone = Instantiate(countUpDone, calGold[6].transform.position + UIBudge, calGold[6].transform.rotation);
            CountUpDone.transform.SetParent(GameObject.Find("/UIOverlay/Calendar").transform);
        }
    }


    


    // Use this for initialization
    void Start()
    {
        countUp = 0;
        if (StaticValueHolder.CurrentDay < 0) StaticValueHolder.CurrentDay = 0;


        //add one to the days-------------------------------------------------------give it to aidans script
        //show each days money
        StaticValueHolder.DayValues[StaticValueHolder.CurrentDay] = StaticValueHolder.DailyMoney;
        
        
        //cross off completed days
        for (int i = 0; i < StaticValueHolder.CurrentDay; i++)
        {
            GameObject myDayDone = Instantiate(dayDone, calDays[i].transform.position, calDays[i].transform.rotation);
            myDayDone.transform.SetParent(GameObject.Find("/UIOverlay/Calendar").transform);
        }

        Debug.Log("Money before thread" + StaticValueHolder.DailyMoney + "");


        //show values for each day, will overwrite current day with count up
        Gold1.text = StaticValueHolder.DayValues[1] + ""; //for some reason there has to be a string in here or it doesnt work so leave the empty string
        Gold2.text = StaticValueHolder.DayValues[2] + "";
        Gold3.text = StaticValueHolder.DayValues[3] + "";
        Gold4.text = StaticValueHolder.DayValues[4] + "";
        Gold5.text = StaticValueHolder.DayValues[5] + "";
        Gold6.text = StaticValueHolder.DayValues[6] + "";
        Gold7.text = StaticValueHolder.DayValues[7] + "";

        StaticValueHolder.CurrentDay += 1;

        //count up money on screen in real time
        StartCoroutine(Type());
        Debug.Log("Done");

    }
}
