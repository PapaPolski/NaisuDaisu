using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private float invincibilityDurationSeconds;

    [SerializeField]
    private float invincibilityDeltaTime;

     MeshRenderer playerMesh;

    private bool isMeleeAttacking;
    public GameObject meleeZone;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        currentPlayerHealth = maxPlayerHealth;
        playerMesh = GetComponent<MeshRenderer>();
        isMeleeAttacking = false;
        meleeZone.SetActive(false);
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

        if(Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }

        if(Input.GetMouseButtonDown(1))
        {
            Melee();
            Debug.Log("Melee Attack");
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

        transform.Translate(movement * Time.deltaTime * speed, Space.World);
    }

    void FireBullet()
    {
        Instantiate(bullet, shooterObj.transform.position, shooterObj.transform.rotation);
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


    void Melee()
    {
        StartCoroutine(MeleeStrike());
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
}
