using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequirementManager : MonoBehaviour
{
    //possible level requirements
    public int maxTotalNotToCross;
    public int minRequirement;
    public int numberOfDice;
    public int pipNumberBanned;
    public bool doublesRequired;
    public bool noShotFired;
    public bool noMeleeUsed;


    public float timerMaxTime;
    public Text totalGoalText, timeText;

    DiceSpawner diceSpawner;

    bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        diceSpawner = GameObject.Find("DiceSpawner").GetComponent<DiceSpawner>();
        SetMinAndMax();
    }

    void SetMinAndMax()
    {
        maxTotalNotToCross = Random.Range(5, 12);
        minRequirement = Random.Range(5, 12);
        if (minRequirement > maxTotalNotToCross || minRequirement == maxTotalNotToCross)
            SetMinAndMax();
        else
            CreateScoreString();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerIsRunning)
        {
            if(timerMaxTime > 0)
            {
                timerMaxTime -= Time.deltaTime;
                UpdateTimerText(timerMaxTime);
            }
            else
            {
                timerIsRunning = false;
                timerMaxTime = 0;
                GameOver();
            }
        }
    }

    void CreateScoreString()
    {
        string totalLevelValue = minRequirement.ToString() + " - " + maxTotalNotToCross.ToString();
        UpdateTotalText(totalLevelValue);
        timerIsRunning = true;
    }

    void GameOver()
    {
        Debug.Log("Game Over");

        if(diceSpawner.totalRolled <= maxTotalNotToCross && diceSpawner.totalRolled >= minRequirement)
        {
            Debug.Log("You win!");
        }
        else
        {
            Debug.Log("You lose!");
        }
    }

    void UpdateTotalText(string txt)
    {
        totalGoalText.text = txt;
    }

    void UpdateTimerText(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
