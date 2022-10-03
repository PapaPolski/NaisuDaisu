using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pips : MonoBehaviour
{
    SphereCollider thisCollider;

    [SerializeField] float timeToPickUp;
    [SerializeField] float pickUpDeltaTime;
    MeshRenderer mesh;

   public void HitOffSide(string parentName)
    {
        Debug.Log("I'm free!");
        thisCollider = gameObject.GetComponent<SphereCollider>();

        thisCollider.enabled = true;
        Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        
        switch(parentName)
        {
            case "Side1":
                Debug.Log("Im hit off 1!");
                FlyOff(1);
                break;
            case "Side2":
                Debug.Log("Im hit off 2!");
                FlyOff(2);
                break;
            case "Side3":
                FlyOff(3);
                Debug.Log("Im hit off 3!");
                break;
            case "Side4":
                FlyOff(4);
                Debug.Log("Im hit off 4!");
                break;
            case "Side5":
                FlyOff(5);
                Debug.Log("Im hit off 5!");
                break;
            case "Side6":
                FlyOff(6);
                Debug.Log("Im hit off 6!");
                break;

        }


        mesh = GetComponent<MeshRenderer>();
        StartCoroutine(Delay());
    }

    void FlyOff(int dieSide)
    {

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(TimeOut());
    }

    IEnumerator TimeOut()
    {
        for (float i = 0; i < timeToPickUp; i += pickUpDeltaTime)
        {
            if (mesh.enabled)
            {
                mesh.enabled = false;
            }
            else
            {
                mesh.enabled = true;
            }
            yield return new WaitForSeconds(pickUpDeltaTime);
        }

        Destroy(this.gameObject);
    }
}
