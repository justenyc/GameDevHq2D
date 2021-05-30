using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : Laser
{
    [SerializeField] GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "player":
                other.GetComponent<Player>().Damage(-1);
                DisableSelf();
                break;

            case "shield_player":
                DisableSelf();
                break;

            case "laser":
                DisableSelf();
                break;

            case "powerup":
                DisableSelf();
                break;

            default:
                break;
        }
    }

    void DisableSelf()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<TrailRenderer>().enabled = false;

        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }
}
