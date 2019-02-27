using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveMoneyBag : MonoBehaviour{

    Vector3 originalPos;
    bool canDrag;
    bool touching;
    public GameObject bowl;
    public Animator animator;

    public void StartDragMoney()
    {
        originalPos = transform.position;
    }

    public void DragMoney()
    {
        transform.position = Input.mousePosition;
    }

    public void StopDragMoney()
    {
        if (touching)
        {
            Debug.Log("Drop");
            transform.position = bowl.transform.position;
            animator.SetBool("PlacedMoney", true);
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

    // Use this for initialization
    void Start ()
    {
        touching = false;
        canDrag = true;
	}

}
