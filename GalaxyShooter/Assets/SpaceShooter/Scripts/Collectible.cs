using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    Type type = new Type();

    [SerializeField]
    float descentSpeed = 1, TripleShot_Duration = 5, Speed_Duration = 10;

    [SerializeField]
    GameObject Shield_Bubble;

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + -descentSpeed * Time.deltaTime, transform.position.z);

        if (Camera.main.WorldToViewportPoint(transform.position).y < 0)
            Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            switch (type)
            {
                case Type.TripleShot:
                    p.gameObject.AddComponent<TripleShot>().SetDuration(TripleShot_Duration);
                    OnCollect();
                    break;

                case Type.Shield:
                    Shield_Bubble[] bubbles = p.GetComponentsInChildren<Shield_Bubble>();
                    if (bubbles.Length < 1)
                        Instantiate(Shield_Bubble, p.transform.position, Shield_Bubble.transform.rotation, p.gameObject.transform);
                    else
                        bubbles[0].AddStrength(1);

                    OnCollect();
                    break;

                case Type.Speed:
                    p.gameObject.AddComponent<Speed>().SetDuration(Speed_Duration);
                    OnCollect();
                    break;

                case Type.Ammo:
                    p.AddAmmo(15);
                    OnCollect();
                    break;

                case Type.Life:
                    p.Damage(1);
                    OnCollect();
                    break;

                case Type.LaserRico:
                    p.gameObject.GetComponent<WeaponModifier>().SetNewProjectile("Laser_Rico");
                    OnCollect();
                    break;

                case Type.StealFuel:
                    p.StealFuel();
                    OnCollect();
                    break;

                default:
                    Debug.LogError("type not defined or player not found");
                    break;
            }
        }

    }

    void OnCollect()
    {
        AudioSource aSource = this.GetComponent<AudioSource>();
        aSource.Stop();
        aSource.Play();

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            sr.enabled = false;

        this.GetComponent<BoxCollider>().enabled = false;
        Destroy(this.gameObject, 1);
    }

    enum Type
    {
        TripleShot,
        Shield,
        Speed,
        Ammo,
        Life,
        LaserRico,
        StealFuel
    };
}