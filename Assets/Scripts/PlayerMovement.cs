using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MeleeWeapon { SUMO = 0, SWORD = 1, BAT = 2 };
public enum RangedWeapon { AK = 0, SHOTGUN = 1 };

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    float horizontal, vertical;
    float verticalMoveLimit = 0.8f;
    Vector3 movement;
    Rigidbody playerBody;

    public GameObject bullet;
    public GameObject shooterObj; 
    public Image aimSprite;

    public Camera mainCamera;

    public int maxPlayerHealth = 3;
    public int currentPlayerHealth;
    private bool isInvuln = false;

    private bool canMove;

    [SerializeField]
    private float invincibilityDurationSeconds;

    [SerializeField]
    private float invincibilityDeltaTime;

    MeshRenderer playerMesh;

    public MeleeWeapon currentMeleeWeapon;
    public RangedWeapon currentRangedWeapon;
    public bool canSwitchWeapon;
    public float weaponSwitchingCooldown = 1.0f;
    public bool canFire = true;

    private bool isMeleeAttacking;
    public GameObject meleeZone;

    public int maxAmmo;
    private int currentAmmo;
    public Text ammoText;

    float meleeChargeTimer = 0.0f;
    float meleeChargeMax = 1f;
    public float currentBatPowerPercantage;
    bool chargingMelee = false;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        currentPlayerHealth = maxPlayerHealth;
        playerMesh = GetComponent<MeshRenderer>();
        isMeleeAttacking = false;
        meleeZone.SetActive(false);
        canMove = true;
        canSwitchWeapon = true;
        currentAmmo = maxAmmo;
        ammoText.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = mainCamera.WorldToScreenPoint(transform.position);

        Vector3 direction = mousePosition - playerPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(-angle + 90, Vector3.up);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetMouseButtonDown(0) && canFire)
        {
            if(currentAmmo > 0)
                Fire();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Melee();
        }
        if(Input.GetMouseButtonUp(1))
        {
            if (chargingMelee)
            {
                chargingMelee = false;
                BatStrike(meleeChargeTimer);
            }
        }

        if(chargingMelee)
        {
            meleeChargeTimer += Time.deltaTime;

            if (meleeChargeTimer >= meleeChargeMax)
            {
                BatStrike(meleeChargeTimer);
            }
        }


        if(Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(1);
        }

        if(isMeleeAttacking)
        {
            meleeZone.SetActive(true);
        }
        else
        {
            meleeZone.SetActive(false);
        }

       /* if (canSwitchWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentMeleeWeapon != MeleeWeapon.SUMO)
            {
                Debug.Log("Switching to Sumo");
                EquipWeapon(MeleeWeapon.SUMO);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && currentMeleeWeapon != MeleeWeapon.SWORD)
            {
                Debug.Log("Switching to Sword");
                EquipWeapon(MeleeWeapon.SWORD);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && currentMeleeWeapon != MeleeWeapon.BAT)
            {
                Debug.Log("Switching to Bat");
                EquipWeapon(MeleeWeapon.BAT);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0) && currentRangedWeapon != RangedWeapon.SHOTGUN)
            {
                Debug.Log("Switching to Shotgun");
                EquipWeapon(RangedWeapon.SHOTGUN);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9) && currentRangedWeapon != RangedWeapon.AK)
            {
                Debug.Log("Switching to AK");
                EquipWeapon(RangedWeapon.AK);
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= verticalMoveLimit;
            vertical *= verticalMoveLimit;
        }

        movement = new Vector3(horizontal, 0, vertical);

        if(canMove)
        transform.Translate(movement * Time.deltaTime * speed, Space.World);
    }

    void Fire()
    {
            switch (currentRangedWeapon)
            {
                case RangedWeapon.AK:
                    Instantiate(bullet, shooterObj.transform.position, shooterObj.transform.rotation);
                    currentAmmo--;
                    ammoText.text = currentAmmo.ToString();
                    break;
                case RangedWeapon.SHOTGUN:
                    ParticleSystem shotgun = gameObject.GetComponentInChildren<ParticleSystem>();
                    shotgun.Play();
                    currentAmmo--;
                    ammoText.text = currentAmmo.ToString();
                    break;
            }
    }

    public void TakeDamage(int damage)
    {
        if(isInvuln)
        {
            return;
        }

        currentPlayerHealth-= damage;

        if(currentPlayerHealth <= 0)
        {
            Debug.Log("Game Over");
            return;
        }

        if(!isInvuln)
        {
            StartCoroutine(Invuln());
        }
    }

    public void EquipWeapon(MeleeWeapon meleeWeaponEquipped)
    {
        currentMeleeWeapon = meleeWeaponEquipped;
        StartCoroutine(SwitchingCooldown());
    }

    public void EquipWeapon(RangedWeapon rangedWeaponEquipped)
    {
        currentRangedWeapon = rangedWeaponEquipped;
        StartCoroutine(SwitchingCooldown());
    }


    void Melee()
    {
        switch (currentMeleeWeapon)
        {
            case MeleeWeapon.SUMO:

                break;
            case MeleeWeapon.SWORD:
                StartCoroutine(MeleeStrike());
                break;
            case MeleeWeapon.BAT:
                chargingMelee = true;
                break;
        }
    }

    void BatStrike(float chargeTime)
    {
        currentBatPowerPercantage = (chargeTime / meleeChargeMax) * 100;
        meleeChargeTimer = 0f;
        StartCoroutine(MeleeStrike());
       // Debug.Log(currentBatPowerPercantage);

    }
    IEnumerator Invuln()
    {
        isInvuln = true;

        for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTime)
        {
            if (playerMesh.enabled)
            {
                playerMesh.enabled = false;
            }
            else
            {
                playerMesh.enabled = true;
            }
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        isInvuln = false;
        playerMesh.enabled = true;
    }

    IEnumerator MeleeStrike()
    {
        if (!isMeleeAttacking)
        {
            isMeleeAttacking = true;

            yield return new WaitForSeconds(0.2f);

            isMeleeAttacking = false;
        }
    }

    IEnumerator SwitchingCooldown()
    {
        canSwitchWeapon = false;

        yield return new WaitForSeconds(weaponSwitchingCooldown);

        canSwitchWeapon = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pip"))
        {
            Debug.Log("Pip picked up");
            currentAmmo++;
            if (currentAmmo > maxAmmo)
                currentAmmo = maxAmmo;
            ammoText.text = currentAmmo.ToString();
            Destroy(collision.gameObject);
        }
    }
}
