using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveMoneyBag : MonoBehaviour {

    public GameObject bowl;
    public GameObject nextButton;
    public Animator nextDayButtonAnim;
    public Animator animator;
    public Text rentDue;
    Vector3 originalPos;
    bool touching;
    int countDown;
    bool paid;
    int rent;
    


    //count the total down when paying rent
    IEnumerator CountTotalDown()
    {
        countDown = -1;
        //print out each letter with slight delay to give typing effect
        for (int i = 0; i < rent; i++)
        {
            Debug.Log("Looping, money is: " + StaticValueHolder.DailyMoney + "      Count is: " + countDown);
            rentDue.text = StaticValueHolder.TotalMoney + countDown + "/" + rent;
            countDown--;
            yield return new WaitForSeconds(rent / 1000);
        }
        StaticValueHolder.TotalMoney -= rent;
        paid = true;
    }

    IEnumerator WaitForSwing()
    {
        yield return new WaitForSeconds(0.5f);
        nextDayButtonAnim.SetBool("Swing", true);
    }

    public void StartDragMoney()
    {
        originalPos = transform.position;
    }

    public void DragMoney()
    {
        transform.position = Input.mousePosition;
    }


    //once dropped bag count down money
    public void StopDragMoney()
    {
        if (touching)
        {
            Debug.Log("Drop");
            transform.position = bowl.transform.position;
            animator.SetBool("PlacedMoney", true);
            StartCoroutine(CountTotalDown());            
        }
        else transform.position = originalPos;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("ScaleBowl")) touching = true;
        Debug.Log("Touch");
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("ScaleBowl")) touching = false;
        Debug.Log("un-Touch");
    }






    void Start()
    {
        paid = false;
        touching = false;
        rent = 200 + StaticValueHolder.CurrentWeek * 200;
        rentDue.text = StaticValueHolder.TotalMoney + "/" + rent;
        nextButton.SetActive(false);
    }


    private void Update()
    {
        if (paid)
        {
            //if enough money to pay rent
            if (StaticValueHolder.TotalMoney >= 0)
            {
                //reset days at end of the week
                StaticValueHolder.CurrentDay = 1;
                StaticValueHolder.CurrentWeek += 1;
                for (int i = 0; i < 8; i++) StaticValueHolder.DayValues[i] = 0;

                //make next day button appear
                nextButton.SetActive(true);
                nextDayButtonAnim.SetBool("swingDown", true);   //start swing animation
                StartCoroutine(WaitForSwing());                 //wait for it to finish and move to still sign
            }
            //if not enough to pay rent
            else
            {
                Debug.Log("GameOver");
            }
        }
        
    }
}
