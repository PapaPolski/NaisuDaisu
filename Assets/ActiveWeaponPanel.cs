using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeaponPanel : MonoBehaviour
{
    PlayerMovement player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

}
