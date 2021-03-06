using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] Type type = new Type();

    [SerializeField] float descentSpeed = 1, TripleShot_Duration = 5, Speed_Duration = 10, ammo_amount = 25;

    [SerializeField] GameObject Shield_Bubble;

    [SerializeField] Vector3 moveDirection = Vector3.down;

    Player player;

    private void Start()
    {
        moveDirection = Vector3.down;
        try
        {
            player = FindObjectOfType<Player>();
            player.playerMagnet += PlayerMagnetListener;
        }
        catch
        {

        }
    }
    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += moveDirection * descentSpeed * Time.deltaTime;

        if (Camera.main.WorldToViewportPoint(transform.position).y < 0 || Camera.main.WorldToViewportPoint(transform.position).y > 1 ||
            Camera.main.WorldToViewportPoint(transform.position).x < 0 || Camera.main.WorldToViewportPoint(transform.position).x > 1)
            Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag.ToLower())
        {
            case "player":
                Player p = other.GetComponent<Player>();
                SetPowerUp(p);
                break;

            case "laser_enemy":
                Destroy(this.gameObject);
                break;

            default:
                break;
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
        player.playerMagnet -= PlayerMagnetListener;
        Destroy(this.gameObject, 1);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.playerMagnet -= PlayerMagnetListener;
        }
    }

    void PlayerMagnetListener(Transform t)
    {
        player.playerMagnet -= PlayerMagnetListener;
        moveDirection = t.position - this.transform.position;
        descentSpeed = 5f;
    }

    void SetMoveDirection(Vector3 newDirection, float newSpeed)
    {
        moveDirection = newDirection;
        descentSpeed = newSpeed;
    }

    void SetPowerUp(Player player)
    {
        Player p = player;
        switch (type)
        {
            case Type.TripleShot:
                TripleShot newTs = p.gameObject.AddComponent<TripleShot>();
                newTs.SetDuration(TripleShot_Duration);
                newTs.SetUiDisplay(this.GetComponentInChildren<RectTransform>().gameObject);
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
                Speed newSpeed = p.gameObject.AddComponent<Speed>();
                newSpeed.SetDuration(Speed_Duration);
                newSpeed.SetUiDisplay(this.GetComponentInChildren<RectTransform>().gameObject);
                OnCollect();
                break;

            case Type.Ammo:
                p.AddAmmo((int)ammo_amount);
                OnCollect();
                break;

            case Type.Life:
                p.Damage(1);
                OnCollect();
                break;

            case Type.LaserRico:
                WeaponModifier newWM = p.gameObject.GetComponent<WeaponModifier>();
                newWM.SetNewProjectile("Laser_Rico");
                newWM.SetUiDisplay(this.GetComponentInChildren<RectTransform>().gameObject);
                OnCollect();
                break;

            case Type.StealFuel:
                p = player;
                p.StealFuel();
                OnCollect();
                break;

            default:
                Debug.LogError("type not defined or player not found");
                break;
        }
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