using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving;
    public Vector3 lastPos;
    public int dieResult = 0;
    public DiceSpawner diceSpawner;

    public Vector3 positionOfLastCheck;

    public int currentDieSize;
    public int maxDieSize;

    public int dieSideCounter;
    public int currentDieHits;
    int maxDieCombo;
    bool inActiveCombo;
    Coroutine comboCoroutine;

    public float hitResetTime = 2f;

    public bool dieIsStunned;

    public void FixedUpdate()
    {
        if (this.transform.position.Equals(lastPos)) isMoving = false; else isMoving = true;
        lastPos = this.transform.position;

        if(!isMoving)
        {
            DieStopped();
            positionOfLastCheck = this.transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        diceSpawner = GameObject.Find("DiceSpawner").GetComponent<DiceSpawner>() ;
        maxDieSize = 3;
        dieSideCounter = 0;
        inActiveCombo = false;
        maxDieCombo = 3;
        dieIsStunned = false;
    }

    public void DiceSpawned(int sideToDisplay, int dieSizeCounter)
    {
        switch(sideToDisplay)
        {
            case 0:
                Throw(1000);
                break;
            case 1:
                this.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 2:
                this.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3:
                this.transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case 4:
                this.transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            case 5:
                this.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case 6:
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
        if (currentDieSize == 0)
            currentDieSize = 1;

        currentDieSize = dieSizeCounter;
    }

    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Throw(float speed)
    {
       //Throw in the direction of the baseball hit
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        rb.AddForce(force * speed);
    }

    void DieStopped()
    {
        GetComponentInChildren<DiceCheck>().CheckResult();
        diceSpawner.UpdateTotal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Bullet>())
        {
            Destroy(other.gameObject);
            Debug.Log("Die with value " + dieResult + " has been shot");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Contact");
    }

    public void DieHitByBat()
    {
        currentDieHits++;
        if (currentDieHits >= maxDieCombo)
        {
            currentDieHits = maxDieCombo;
            DieStunned();
        }

        if (!inActiveCombo)
        {
            comboCoroutine = StartCoroutine(HitCD());
            inActiveCombo = true;
        }
        else if (inActiveCombo && !dieIsStunned)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = StartCoroutine(HitCD());
        }
}

    void DieStunned()
    {
        if (!dieIsStunned)
        {
            dieIsStunned = true;
            StartCoroutine(StunCooldown());
        }
    }

    IEnumerator HitCD()
    {
        yield return new WaitForSeconds(hitResetTime);
        inActiveCombo = false;
        currentDieHits = 0;
    }

    IEnumerator StunCooldown()
    {
        yield return new WaitForSeconds(2f);
        dieIsStunned = false;
    }
}
