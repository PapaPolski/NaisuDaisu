using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSide : MonoBehaviour
{
    public int dieSide;
    public int currentPipAmount;
    private string sideName = "Side";
    int maxPipsToLose = 2;

    public List<GameObject> pipsInSide = new List<GameObject>();
    Die parentDie;
    
    // Start is called before the first frame update
    void Start()
    {
        string dieSideNum = gameObject.name.Replace(sideName, "");
        dieSide = int.Parse(dieSideNum);
        currentPipAmount = dieSide;

        foreach(Transform child in transform)
        {
            if(child.gameObject.GetComponent<Pips>())
            {
                pipsInSide.Add(child.gameObject);
            }
        }
        parentDie = GetComponentInParent<Die>();
    }
    
    public void LosePips(int numberOfPips)
    {
        currentPipAmount = numberOfPipsFunc(numberOfPips);

        for (int i = 0; i < numberOfPips; i++)
        {
            pipsInSide[i].GetComponent<Pips>().HitOffSide(pipsInSide[i].transform.parent.name);
            pipsInSide[i].transform.SetParent(null);
        }

        for(int j = pipsInSide.Count - 1; j >= 0; j--)
        {
            if(pipsInSide[j].transform.parent == null)
            {
                pipsInSide.RemoveAt(j);            }
        }



        if(currentPipAmount == 0)
        {
           // Destroy(transform.parent.gameObject);
        }
    }
    
    public int numberOfPipsFunc(int num)
    {
        currentPipAmount -= num;

        if (currentPipAmount <= 0)
            currentPipAmount = 0;

        return currentPipAmount;
    }

    public void CutInHalf()
    {
        if(parentDie.currentDieSize < parentDie.maxDieSize)
        {
            if(parentDie.GetComponentInChildren<DiceCheck>().previousRollResult > 0)
            {

            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Melee"))
        {
            PlayerMovement player = other.transform.GetComponentInParent<PlayerMovement>();
            
            switch (player.currentMeleeWeapon)
            {
                case MeleeWeapon.SUMO:
                    Debug.Log(this.gameObject.name + " hit by sumo");
                    break;
                case MeleeWeapon.SWORD:
                    Debug.Log(this.gameObject.name + " hit by sword");
                    CutInHalf();
                    break;
                case MeleeWeapon.BAT:
                    Debug.Log(this.gameObject.name + " hit by bat");
                    HitByBat(other.transform.GetComponentInParent<PlayerMovement>().currentBatPowerPercantage);
                    break;
            }
        }
    }

    void HitByBat(float hitPower)
    {
        this.GetComponentInParent<Die>().Throw(hitPower);
        if (currentPipAmount >= 1)
        {
            //Add more code for calculating critical vs num of pips to remove
                if(hitPower > 90)
                {
                    int random = Random.Range(1, 2);
                    LosePips(random);
                }
        }
    }
}
