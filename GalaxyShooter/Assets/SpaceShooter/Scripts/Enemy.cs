using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    GameObject deathParticles;

    public delegate void death();
    public event death myDeath;

    // Start is called before the first frame update
    void Start()
    {
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
        transform.position = new Vector3(transform.position.x, transform.position.y + (-1 * moveSpeed * Time.deltaTime), transform.position.z);
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
                try
                {
                    other.GetComponent<Player>().Damage(-1);
                }
                catch
                {
                    Debug.Log("Player component not found");
                }
                Die();
                break;

            case "laser":
                Die();
                break;

            case "powerup":
                break;

            default:
                Die();
                break;
        }
    }
}
