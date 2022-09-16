using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieChecker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {

        switch(other.gameObject.name)
        { 
            case "Side1":
                other.GetComponentInParent<Die>().dieResult = 6;
                break;

            case "Side2":
                other.GetComponentInParent<Die>().dieResult = 5;
                break;

            case "Side3":
                other.GetComponentInParent<Die>().dieResult = 4;
                break;

            case "Side4":
                other.GetComponentInParent<Die>().dieResult = 3;
                break;

            case "Side5":
                other.GetComponentInParent<Die>().dieResult = 2;
                break;

            case "Side6":
                other.GetComponentInParent<Die>().dieResult = 1;
                break;

        }
    }
}
