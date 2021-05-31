using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] float health = 35;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float movementIntervals = 3;

    bool attacking = false;
    bool canMove = true;
    ParticleSystem ps;

    [SerializeField] GameObject beam;
    [SerializeField] GameObject explosion;

    Vector3 moveToTarget;
    Vector3 targetRotation;

    float rotateSpeedTemp;
    Player p;
    AudioSource aSource;
    // Start is called before the first frame update
    void Start()
    {
        moveToTarget = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, -Camera.main.transform.position.z));
        p = FindObjectOfType<Player>();
        ps = GetComponentInChildren<ParticleSystem>();
        aSource = GetComponent<AudioSource>();
        rotateSpeedTemp = rotateSpeed;

        StartCoroutine(ActionSync(movementIntervals));
    }

    // Update is called once per frame
    void Update()
    {
        SyncParticles();
        FacePlayer();
        Movement();
    }


    IEnumerator ActionSync(float intervals)
    {
        yield return new WaitForSeconds(intervals);
        RandomPointOnScreen();
        Attack();
        StartCoroutine(ActionSync(movementIntervals));
    }

    void Attack()
    {
        //Maybe add more than one attack later
        BeamAttack();
    }

    public void BeamAttack()
    {
        if (attacking == false)
        {
            StartCoroutine(ToggleDefaults(7f));

            attacking = true;
            canMove = false;
            rotateSpeed = 1f;

            GameObject newBeam = Instantiate(beam, transform.position, beam.transform.rotation, this.transform);
            newBeam.transform.localRotation = Quaternion.Euler(new Vector3 (0,0,180));
            newBeam.transform.localPosition = Vector3.zero + new Vector3(0, 3.5f, 0);
        }
    }

    public void Damage(float value)
    {
        health += value;

        GameObject newExplosion = Instantiate(base.GetDeathParticles(), transform.position, base.GetDeathParticles().transform.rotation);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        base.Die();
        Instantiate(explosion, transform.position, explosion.transform.rotation);
    }

    void FacePlayer()
    {
        if (p != null)
        {
            float xDistance = p.transform.position.x - transform.position.x;
            float yDistance = p.transform.position.y - transform.position.y;
            float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle - 90)), rotateSpeed * Time.deltaTime);
        }
    }

    void Movement()
    {
        if (canMove == true)
        {
            transform.position = Vector3.Lerp(transform.position, moveToTarget, base.GetMoveSpeed() * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "laser":
                Damage(-1);
                break;

            default:
                break;
        }
    }

    void RandomPointOnScreen()
    {
        Vector3 currentViewPortPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (currentViewPortPoint.x > 0.5f)
        {
            Vector3 randomViewportPoint = new Vector3(Random.Range(0.1f, 0.4f), Random.Range(0.5f, 0.9f), -Camera.main.transform.position.z);
            moveToTarget = Camera.main.ViewportToWorldPoint(randomViewportPoint);
        }
        else
        {
            Vector3 randomViewportPoint = new Vector3(Random.Range(0.5f, 0.9f), Random.Range(0.5f, 0.9f), -Camera.main.transform.position.z);
            moveToTarget = Camera.main.ViewportToWorldPoint(randomViewportPoint);
        }
    }

    void ResetDefaults()
    {
        canMove = true;
        attacking = false;
        rotateSpeed = rotateSpeedTemp;

        aSource.Stop();
        aSource.Play();
    }

    void SyncParticles()
    {
        var main = ps.main;
        main.startRotation = transform.rotation.eulerAngles.z * -Mathf.Deg2Rad;
    }

    IEnumerator ToggleDefaults(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ResetDefaults();
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    public void SetRotateSpeed(float value)
    {
        rotateSpeed = value;
    }
}
