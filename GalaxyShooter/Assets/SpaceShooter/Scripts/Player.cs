using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int lives = 3;
    [SerializeField] private float moveSpeed = 8, fireRate = 0.5f, boostSpeed = 3;
    private bool canFire = true;

    [Header("VFX")]
    [SerializeField] GameObject[] Damages;
    [SerializeField] GameObject laser, deathParticles;

    [Header("Misc")]
    [SerializeField] Transform[] ExtraPoints;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        transform.position = Vector3.zero;
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
        animator.SetInteger("Move", Mathf.Abs(value));

        if (value > 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    }

    public void Damage()
    {
        lives -= 1;

        UiManager.instance.UpdateLivesDisplay(lives);

        if (lives < 0)
        {
            Die();
        }
        Damages[lives].SetActive(true);
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
        if (Input.GetKeyDown(KeyCode.Space) && canFire == true)
        {
            StartCoroutine(FireCooldown());
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            AddSpeed(boostSpeed);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            AddSpeed(-boostSpeed);
        }
    }

    IEnumerator FireCooldown()
    {
        try
        {
            Instantiate(laser, transform.position + Vector3.up, Quaternion.identity);
        }
        catch
        {
            Debug.LogError("Prefab not found");
        }

        yield return new WaitForEndOfFrame();
        canFire = false;

        yield return new WaitForSeconds(fireRate);
        canFire = true;
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

        Vector3 edge = Camera.main.ViewportToWorldPoint(new Vector3(viewportPoint.x, Mathf.Clamp(viewportPoint.y, 0, 0.3f), viewportPoint.z));
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

    public bool GetCanFire()
    {
        return canFire;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public GameObject GetLaser()
    {
        return laser;
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
}
