using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSpawner : MonoBehaviour
{
    public GameObject dicePrefab;
    public int diceAmountToSpawn;

    public int totalRolled;

    public Text totalRolledText;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDice();
        }
    }

    void SpawnDice()
    {
        for (int i = 0; i < diceAmountToSpawn; i++)
        {
            GameObject diceToSpawn = Instantiate(dicePrefab, this.gameObject.transform.position, Quaternion.Euler(Random.Range(2.557f, -2.557f), Random.Range(0f, -5.795f), gameObject.transform.rotation.z));
            diceToSpawn.GetComponent<Die>().DiceSpawned(0, 1);
        }
    }

    public void UpdateTotal()
    {
        totalRolledText.text = totalRolled.ToString();
    }


}
