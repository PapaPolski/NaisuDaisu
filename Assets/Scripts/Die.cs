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

    public int currentDieSize = 1;
    public int maxDieSize;

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
    }

    public void DiceSpawned(int sideToDisplay)
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
    }

    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Throw(float speed)
    {
       // transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
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
}
