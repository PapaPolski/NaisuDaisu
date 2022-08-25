using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pips : MonoBehaviour
{
   SphereCollider collider;

    [SerializeField] float timeToPickUp;
    [SerializeField] float pickUpDeltaTime;
    MeshRenderer mesh;

   public void HitOffSide()
    {
        Debug.Log("I'm free!");
        collider = gameObject.GetComponent<SphereCollider>();

        collider.enabled = true;
        Rigidbody rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        //rb.AddForce()
        mesh = GetComponent<MeshRenderer>();
        StartCoroutine(Delay());
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
