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
        base.SetTrailColor(Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "player":
                other.GetComponent<Player>().Damage(-1);
                DisableSelf();
                break;

            case "laser":
                DisableSelf();
                break;

            case "powerup":
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
