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
    float countdownTimer;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        iconImage = this.GetComponentInChildren<Image>();
        startColor = iconImage.color;
        cooldown = GetComponentInChildren<Slider>();
        cooldown.value = 0;
        countdownTimer = player.weaponSwitchingCooldown;
    }

    private void Start()
    {
        OnWeaponChange();
    }

    private void Update()
    {
        if(player.canSwitchWeapon)
        {
            OnWeaponChange();
        }

        if(countdownTimer > 0)
        {
            countdownTimer -= 1 * Time.deltaTime;
        }
        cooldown.value = countdownTimer;
    }

    public void OnWeaponChange()
    {
        countdownTimer = player.weaponSwitchingCooldown;
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
}
