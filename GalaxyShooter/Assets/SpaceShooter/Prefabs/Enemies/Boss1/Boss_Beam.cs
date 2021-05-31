using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Beam : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    void OnTriggerEnter(Collider other)
    {
        switch (other.tag.ToLower())
        {
            case "player":
                other.GetComponent<Player>().Damage(-1);
                Instantiate(explosion, other.transform.position, explosion.transform.rotation);
                break;

            case "shield_player":
                break;

            case "laser":
                break;

            case "powerup":
                break;

            default:
                break;
        }
    }
}
