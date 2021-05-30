using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int lives = 3, ammo = 15;
    [SerializeField] private float moveSpeed = 8, fireRate = 0.5f, boostSpeed = 3, fuel = 5, yClamp = 0.5f;
    private bool canFire = true;
    float remainingFuel = 5, baseMoveSpeed;

    [Header("VFX")]
    [SerializeField] GameObject[] Damages;
    [SerializeField] GameObject projectile, deathParticles;

    [Header("Misc")]
    [SerializeField] Transform[] ExtraPoints;

    Animator animator;
    SpriteRenderer thruster;

    public delegate void magnet(Transform t);
    public event magnet playerMagnet;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        transform.position = Vector3.zero;
        fuel = remainingFuel;
        baseMoveSpeed = moveSpeed;
        thruster = GameObject.Find("Thruster").GetComponent<SpriteRenderer>();

        if (deathParticles == null)
        {
            Debug.LogError("Death Particles not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        InputListener();
    }

    void AnimateCharacter(int value)
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        animator.SetInteger("Move", Mathf.Abs(value));

        if (value > 0)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        ammo = Mathf.Clamp(ammo, 0, 15);
    }

    public void Damage(int amount)
    {
        lives += amount;
        lives = Mathf.Clamp(lives, -1, 3);

        UiManager.instance.UpdateLivesDisplay(lives);

        if (lives < 0)
        {
            Die();
        }
        else if (amount > 0)
            Damages[lives - 1].SetActive(false);
        else
            Damages[lives].SetActive(true);

        if (amount < 0)
            Camera.main.GetComponent<Animator>().Play("CameraShake", -1, 0f);
    }

    void Die()
    {
        AudioSource aSource = deathParticles.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
        Destroy(this.gameObject);
    }

    void InputListener()
    {
        if (ammo > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canFire == true)
            {
                StartCoroutine(FireCooldown());
            }
            UiManager.instance.UpdateAmmoDisplay(ammo);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (playerMagnet != null && remainingFuel >= fuel/2)
            {
                remainingFuel -= fuel / 2;
                playerMagnet(this.transform);
            }
        }
        FuelHandler();
    }

    IEnumerator FireCooldown()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position + Vector3.up, Quaternion.identity);
        try
        {
            newProjectile.GetComponent<Laser>().SetTrailColor(Color.green);
        }
        catch
        {

        }
        ammo--;

        yield return new WaitForEndOfFrame();
        canFire = false;

        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    void FuelHandler()
    {
        if (remainingFuel > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                remainingFuel -= Time.deltaTime;
            }
            else
            {
                remainingFuel += Time.deltaTime;
                remainingFuel = Mathf.Clamp(remainingFuel, 0, fuel);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                AddSpeed(boostSpeed);
                thruster.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (moveSpeed > baseMoveSpeed)
                {
                    AddSpeed(-boostSpeed);
                }
                thruster.enabled = false;
            }
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            remainingFuel += Time.deltaTime;
            thruster.enabled = false;
        }
        UiManager.instance.UpdateFuelDisplay(remainingFuel / fuel);
    }

    void Movement()
    {
        transform.position = new Vector3(transform.position.x + Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime,
                                        transform.position.y + Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime,
                                        0);
        ClampMovement();
        AnimateCharacter(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")));
    }

    void ClampMovement()
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 edge = Camera.main.ViewportToWorldPoint(new Vector3(viewportPoint.x, Mathf.Clamp(viewportPoint.y, 0, yClamp), viewportPoint.z));
        transform.position = edge;

        if (viewportPoint.x < 0f)
        {
            Vector3 edgeWrap = Camera.main.ViewportToWorldPoint(new Vector3(1, viewportPoint.y, viewportPoint.z));
            transform.position = edgeWrap;
        }
        else if (viewportPoint.x > 1f)
        {
            Vector3 edgeWrap = Camera.main.ViewportToWorldPoint(new Vector3(0, viewportPoint.y, viewportPoint.z));
            transform.position = edgeWrap;
        }
    }

    public float GetAmmo()
    {
        return ammo;
    }

    public bool GetCanFire()
    {
        return canFire;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public GameObject GetProjectile()
    {
        return projectile;
    }

    public void SetProjectile(GameObject newProjectile)
    {
        projectile = newProjectile;
    }

    public int GetLives()
    {
        return lives;
    }

    public Transform[] GetTransforms()
    {
        return ExtraPoints;
    }

    public void AddSpeed(float modifier)
    {
        moveSpeed += modifier;
    }

    public void StealFuel()
    {
        remainingFuel = 0;
    }
}
