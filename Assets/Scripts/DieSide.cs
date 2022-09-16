using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSide : MonoBehaviour
{
    public int dieSide;
    public int currentPipAmount;
    private string sideName = "Side";

    public List<GameObject> pipsInSide = new List<GameObject>();
    
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
    }
    
    public void LosePips(int numberOfPips)
    {
        currentPipAmount = numberOfPipsFunc(numberOfPips);

        for (int i = 0; i < numberOfPips; i++)
        {
            pipsInSide[i].transform.SetParent(null);
            pipsInSide[i].GetComponent<Pips>().HitOffSide();
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
                    int random = Random.Range(1, 2);
                    if(currentPipAmount >= 1)
                        LosePips(random);

                    break;
            }
        }
    }
}
