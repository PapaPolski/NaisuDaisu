using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsTitleScroll : MonoBehaviour
{
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.forward * 10 * Time.deltaTime;
    }
}
