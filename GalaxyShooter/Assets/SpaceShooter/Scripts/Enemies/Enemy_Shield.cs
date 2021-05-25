using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
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

    void Die()
    {
        AudioSource aSource = this.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);
        aSource.Stop();
        aSource.Play();

        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;

        transform.parent = null;
        Destroy(this.gameObject, 2f);
    }
}
