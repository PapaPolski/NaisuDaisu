using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving;
    public Vector3 lastPos;
    public int dieResult = 0;
    public DiceSpawner diceSpawner;

    public bool diceChecked;

    public void FixedUpdate()
    {
        if (this.transform.position.Equals(lastPos)) isMoving = false; else isMoving = true;
        lastPos = this.transform.position;

        if(!isMoving)
        {
            DieStopped();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        diceSpawner = GameObject.Find("DiceSpawner").GetComponent<DiceSpawner>() ;
        Throw(1000);
        diceChecked = false;
    }

    private void OnEnable()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Throw(float speed)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        rb.AddForce(force * speed);
    }

    void DieStopped()
    {
        if (!diceChecked)
        {
            diceChecked = true;
            diceSpawner.totalRolled += dieResult;
            foreach (Transform child in transform)
            {
                if(!child.gameObject.GetComponent<DieSide>())
                        child.gameObject.SetActive(false);
            }
            diceSpawner.UpdateTotal();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Bullet>())
        {
            Destroy(other.gameObject);
            Debug.Log("Die with value " + dieResult + " has been shot");

            //IDEA 1 -> Just reduce the value of the dice by 1. Not very exciting but more predictable

            /* diceSpawner.totalRolled -= 1;
            diceSpawner.UpdateTotal();

            if (dieResult > 1)
            {
                dieResult--;
                Debug.Log("New Result is " + dieResult);

                Vector3 saveRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);

                switch (dieResult)
                {
                    case 5:
                        transform.rotation = Quaternion.LookRotation(new Vector3(-1, saveRotation.y, saveRotation.z), Vector3.forward);
                        break;
                    case 4:
                        transform.rotation = Quaternion.LookRotation(new Vector3(saveRotation.x, 1, saveRotation.z), Vector3.forward);
                        break;
                    case 3:
                        transform.rotation = Quaternion.LookRotation(new Vector3(saveRotation.x, -1, saveRotation.z), Vector3.forward);
                        break;
                    case 2:
                        transform.rotation = Quaternion.LookRotation(new Vector3(1, saveRotation.y, saveRotation.z), Vector3.forward);
                        break;
                    case 1:
                        transform.rotation = Quaternion.LookRotation(new Vector3(saveRotation.x, saveRotation.y, -1), Vector3.forward);
                        break;
                }

            }
            else if (dieResult <= 1)
            {
                GameObject.Destroy(gameObject);
            }*/


            //IDEA 2 -> Re roll the die from the shot power, but reduce all pips by 1. More chaotic.
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Contact");
    }
}
