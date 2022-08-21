using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType {  NONE = 0, MELEE = 1, RANGED = 2};

    [SerializeField] protected float weaponCooldown;
    [SerializeField] protected string weaponName;
    [SerializeField] protected int weaponType;
    [SerializeField] protected int maxWeaponAmmoSize;
    [SerializeField] protected int currentWeaponAmmo;

    public float timer;
    public bool weaponCooldownActive;
    public bool weaponUsed;

    protected virtual void Awake()
    {
        Init(0.0f, 0);
    }

    protected virtual void Init(float _weaponCooldown, int _maxWeaponAmmoSize)
    {
        weaponCooldown = 0.0f;
        weaponType = 0;
        maxWeaponAmmoSize = 0;
        currentWeaponAmmo = maxWeaponAmmoSize;
    }

    // Update is called once per frame
    protected void Update()
    {
        UseWeapon();
        Cooldown();
    }

    protected virtual void UseWeapon()
    {
        if(Input.GetMouseButtonDown(0) && weaponType == 1)
        {
            MeleeAttack();
        }

        if(Input.GetMouseButtonDown(1) && weaponType == 2)
        {
            RangedAttack();
        }
    }

    protected virtual void Cooldown()
    {

    }

    protected virtual void MeleeAttack()
    {
        Debug.Log("Melee Attack");
    }

    protected virtual void RangedAttack()
    {
        Debug.Log("Ranged Attack");
    }
}

public class SumoStrike : Weapon
{

    WeaponType type;

    protected override void Init()
    {
        base.Awake();
        type = WeaponType.MELEE;
        weaponCooldown = 3.0f;
        weaponType = 1;
        maxWeaponAmmoSize = 10;
    }

}
