using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public Animator anim;
    private bool weaponWheelSelected = false;
    public Image selectedItem;
    public Sprite noImage;
    public static int weaponID;

    PlayerMovement player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            weaponWheelSelected = true;
        }
        if(Input.GetMouseButtonUp(2))
        {
            weaponWheelSelected = false;
        }

        if(weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
            player.canFire = false;
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
            player.canFire = true;
        }

        switch(weaponID)
        {
            case 0: //nothing
                selectedItem.sprite = noImage;
                break;
            case 1: //Sumo
                Debug.Log("Sumo");
                player.EquipWeapon(MeleeWeapon.SUMO);
                weaponWheelSelected = false;
                break;
            case 2: //Sword
                Debug.Log("Sword");
                player.EquipWeapon(MeleeWeapon.SWORD);
                weaponWheelSelected = false;
                break;
            case 3: //Bat
                player.EquipWeapon(MeleeWeapon.BAT);
                weaponWheelSelected = false;
                break;
            case 4: //AK
                player.EquipWeapon(RangedWeapon.AK);
                weaponWheelSelected = false;
                break;
            case 5: //Shotgun
                Debug.Log("Shotgun");
                player.EquipWeapon(RangedWeapon.SHOTGUN);
                weaponWheelSelected = false;
                break;
        }
    }
}
