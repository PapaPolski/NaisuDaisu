using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSide : MonoBehaviour
{
    public int dieSide;
    public int currentPipAmount;
    private string sideName = "Side";
    int maxPipsToLose = 2;
    int colliderCounter; 

    public List<GameObject> pipsInSide = new List<GameObject>();
    Die parentDie;
    public DiceSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        string dieSideNum = gameObject.name.Replace(sideName, "");
        dieSide = int.Parse(dieSideNum);
        currentPipAmount = dieSide;
        colliderCounter = 0;

        foreach(Transform child in transform)
        {
            if(child.gameObject.GetComponent<Pips>())
            {
                pipsInSide.Add(child.gameObject);
            }
        }
        parentDie = GetComponentInParent<Die>();
        spawner = GameObject.Find("DiceSpawner").GetComponent<DiceSpawner>();
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
        Debug.Log("Cutting function called");
        float dieResult = parentDie.GetComponentInChildren<DiceCheck>().previousRollResult;

        if (parentDie.currentDieSize < parentDie.maxDieSize)
        {
            if(dieResult > 0 && parentDie.dieSideCounter == 1)
            {
                if(dieResult == 1)
                {
                    Vector3 positionA = this.transform.position + new Vector3(1, 0, 0);
                    SpawnNewDie(1, positionA);
                    Vector3 positionB = this.transform.position + new Vector3(-1, 0, 0);
                    SpawnNewDie(1, positionB);
                }
                              
                else if (dieResult % 2 == 0)
                {
                    float dieNumberToSpawn = dieResult / 2;
                    Vector3 positionA = this.transform.position + new Vector3(1, 0, 0);
                    SpawnNewDie((int)dieNumberToSpawn, positionA);
                    Vector3 positionB = this.transform.position + new Vector3(-1, 0, 0);
                    SpawnNewDie((int)dieNumberToSpawn, positionB);
                }
                else
                {
                    int a = Mathf.FloorToInt(dieResult / 2);
                    Debug.Log(a);
                    int b = Mathf.CeilToInt(dieResult / 2);
                    Debug.Log(b);
                    SpawnNewDie(a, this.transform.position + new Vector3(1, 0, 0));
                    SpawnNewDie(b, this.transform.position + new Vector3(-1, 0, 0));
                }  
                spawner.totalRolled -= this.transform.parent.GetComponentInChildren<DiceCheck>().pipsToAdd;
                spawner.UpdateTotal();
                Destroy(gameObject.transform.parent.gameObject);
            }
            else
            {
               // Destroy(this.transform.parent.gameObject);
            }
        }
    }

    void SpawnNewDie(int diceNumberToSpawn, Vector3 position)
    {
        GameObject dicePrefab = GameObject.Find("DiceSpawner").GetComponent<DiceSpawner>().dicePrefab;
        GameObject diceToSpawn = Instantiate(dicePrefab, (Vector3) position, this.transform.rotation);
        diceToSpawn.GetComponent<Die>().DiceSpawned(diceNumberToSpawn, parentDie.currentDieSize+1);
       // diceToSpawn.GetComponent<Die>().currentDieSize += this.GetComponentInParent<Die>().currentDieSize;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Melee"))
        {
            if (parentDie.dieSideCounter == 0)
            {
                parentDie.dieSideCounter++;
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
                StartCoroutine(CounterCooldown());
            }
        }
    }

    void HitByBat(float hitPower)
    {
        this.GetComponentInParent<Die>().Throw(hitPower * 4);
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

    IEnumerator CounterCooldown()
    {
        yield return new WaitForSeconds(1);
        parentDie.dieSideCounter = 0;
    }
}
