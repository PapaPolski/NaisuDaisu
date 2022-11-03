using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponIcon : MonoBehaviour
{
    public string weaponName;
    PlayerMovement player;
    Image iconImage;
    Color startColor, tempColor;
    public bool isMeleeWeapon;
    Slider cooldown;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        iconImage = this.GetComponent<Image>();
        startColor = iconImage.color;
        cooldown = GetComponentInChildren<Slider>();
        cooldown.value = 0;
    }

    private void Start()
    {
        OnWeaponChange();
    }

    private void Update()
    {
        if(!player.canSwitchWeapon)
        {
            OnWeaponChange();
        }
    }

    public void OnWeaponChange()
    {
        switch(isMeleeWeapon)
        {
            case false:
                if (weaponName == player.currentRangedWeapon.ToString())
                {
                    tempColor = startColor;
                    tempColor.a = 1f;
                    iconImage.color = tempColor;
                }
                else
                {
                    iconImage.color = startColor;
                }
                break;
            case true:
                if (weaponName == player.currentMeleeWeapon.ToString())
                {
                    tempColor = startColor;
                    tempColor.a = 1f;
                    iconImage.color = tempColor;
                }
                else
                {
                    iconImage.color = startColor;
                }
                break;
        } 
    }

    IEnumerator WeaponSwitchCooldown()
    {

    }
}
