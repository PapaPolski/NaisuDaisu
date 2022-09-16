using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheck : MonoBehaviour
{

    public int pipsToAdd = 0;
    public int previousRollResult = 0;
    bool initalCheckComplete = false;

   public void CheckResult()
    {
        int layerMask = LayerMask.GetMask("DieSide");
        Vector3 upwrd = -(transform.position - new Vector3(0, 10, 0));

        RaycastHit hit;

        Ray upwardsRay = new Ray(transform.position, upwrd);

        if (Physics.Raycast(upwardsRay, out hit, 10, layerMask))
        {
            if (hit.collider.isTrigger)
            {
                Debug.Log(hit.collider.name);
                Debug.DrawRay(upwardsRay.origin, upwrd, Color.red, 20);
                pipsToAdd = hit.collider.GetComponent<DieSide>().currentPipAmount;
            }
        }

        if (!initalCheckComplete)
        {
            initalCheckComplete = true;
            CheckPips();
        }
        else if(initalCheckComplete)
        {
            if(pipsToAdd != previousRollResult && this.transform.GetComponentInParent<Transform>().position != GetComponentInParent<Die>().positionOfLastCheck)
            {
                GetComponentInParent<Die>().diceSpawner.totalRolled -= previousRollResult;
                CheckPips();
            }
        }
    }

    void CheckPips()
    {
        GetComponentInParent<Die>().diceSpawner.totalRolled += pipsToAdd;
        previousRollResult = pipsToAdd;
    }
}
