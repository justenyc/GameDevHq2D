using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    float timeMovement;

    [SerializeField]
    GameObject deathParticles;

    [SerializeField] Vector3 moveDirection = Vector3.down;

    public delegate void death();
    public event death myDeath;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = new Vector3(Mathf.Round(Random.Range(-1f, 1f)), Random.Range(-1, 0), 0);
        timeMovement = moveSpeed * Time.deltaTime;
        if (deathParticles == null)
        {
            Debug.LogError("Death Particles not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position = new Vector3(transform.position.x + moveDirection.x * timeMovement, transform.position.y + moveDirection.y * timeMovement, transform.position.z);
        CheckScreenBounds();
    }

    void CheckScreenBounds()
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

    private void Die()
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

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "player":
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
}
