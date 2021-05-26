using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    float timeMovement;

    [SerializeField]
    GameObject deathParticles, shield;

    public Vector3 moveDirection { get; set; } = Vector3.down;

    public delegate void death();
    public event death myDeath;

    // Start is called before the first frame update
    public void Start()
    {
        moveDirection = new Vector3(Mathf.Round(Random.Range(-1f, 1f)), Random.Range(-1, 0), 0);
        timeMovement = moveSpeed * Time.deltaTime;

        if (deathParticles == null)
        {
            Debug.LogError("Death Particles not found");
        }

        if(shield == null)
        {
            Debug.LogError("Enemy Shield Prefab not found");
        }
        else
        {
            float random = Random.Range(0f, 100f);
            if (random < 25f)
                Instantiate(shield, transform.position, shield.transform.rotation, this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        transform.position = new Vector3(transform.position.x + moveDirection.x * timeMovement, transform.position.y + moveDirection.y * timeMovement, transform.position.z);
        CheckScreenBounds();
    }

    public void CheckScreenBounds()
    {
        Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPortPosition.y < 0)
        {
            Vector3 swapPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1, viewPortPosition.z));
            transform.position = swapPos;
        }

        if (viewPortPosition.x > 1)
        {
            Vector3 swapPos = Camera.main.ViewportToWorldPoint(new Vector3(0f, viewPortPosition.y, viewPortPosition.z));
            transform.position = swapPos;
        }
        else if (viewPortPosition.x < 0)
        {
            Vector3 swapPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, viewPortPosition.y, viewPortPosition.z));
            transform.position = swapPos;
        }
    }

    public virtual void Die()
    {
        AudioSource aSource = deathParticles.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);

        if (myDeath != null)
        {
            myDeath();

            var temp = myDeath.GetInvocationList();
            foreach (var d in temp)
            {
                myDeath -= (d as death);
            }
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (myDeath != null)
        {
            var temp = myDeath.GetInvocationList();
            foreach (var d in temp)
            {
                myDeath -= (d as death);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "player":
                Debug.Log(this.GetComponent<Collider>());
                Debug.Log(other);
                other.GetComponent<Player>().Damage(-1);
                Die();
                break;

            case "laser":
                Die();
                break;

            case "powerup":
                break;

            default:
                break;
        }
    }

    public void AddMoveSpeed(float value)
    {
        moveSpeed += value;
        timeMovement = moveSpeed * Time.deltaTime;
    }
}
