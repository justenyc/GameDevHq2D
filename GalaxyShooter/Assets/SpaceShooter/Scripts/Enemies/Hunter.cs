using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Enemy
{
    Material detectorMat;
    GameObject detector;
    Player p;
    [SerializeField] GameObject laser_enemy;

    [SerializeField] float fireRate = 3f;
    [SerializeField] float fireRateCD;

    bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        fireRateCD = fireRate;
        detector = GetComponentsInChildren<MeshRenderer>()[1].gameObject;
        detectorMat = detector.GetComponent<MeshRenderer>().material;
        SetDetectorColor(Color.red);
        p = GameObject.Find("Player").GetComponent<Player>();

        base.Start();

        moveDirection = new Vector3(1f, -1f, 0f);
        StartCoroutine(ChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        fireRateCD -= Time.deltaTime;
        fireRateCD = Mathf.Clamp(fireRateCD, 0, fireRate);
        ChangeDirectionInRelationToPlayer();
        base.Movement();
    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(3f);
        moveDirection = new Vector3(moveDirection.x * -1, -1, 0);
        StartCoroutine(ChangeDirection());
    }

    void ChangeDirectionInRelationToPlayer()
    {
        if (fireRateCD <= 0 && p != null)
        {
            SetDetectorColor(Color.white);
            if (p.transform.position.y < transform.position.y)
            {
                ShootRayCast(Vector3.down, new Vector3(0, 0, 180));
            }
            else
            {
                ShootRayCast(Vector3.up, Vector3.zero);
            }
        }
        ChangeDetectorPosition();
    }

    void FireLaser(Vector3 laserRotation)
    {
        GameObject laser = Instantiate(laser_enemy, transform.position, Quaternion.Euler(laserRotation));
        laser.GetComponent<Laser_Enemy>().SetTrailColor(new Color(1, 0, 1));
    }

    void ChangeDetectorPosition()
    {
        if (p.transform.position.y < transform.position.y)
        {
            detector.transform.localPosition = new Vector3(0, -25f, 0);
        }
        else
        {
            detector.transform.localPosition = new Vector3(0, 25f, 0);
        }
    }

    void ShootRayCast(Vector3 direction, Vector3 laserRot)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            if (hit.collider.tag.ToLower() == "player")
            {
                SetDetectorColor(Color.red);
                FireLaser(laserRot);
                fireRateCD = fireRate;
            }
            else if (hit.collider.tag.ToLower() == "powerup")
            {
                SetDetectorColor(Color.red);
                FireLaser(laserRot);
                fireRateCD = fireRate;
            }
        }
    }

    public void SetDetectorColor(Color newColor)
    {
        detectorMat.EnableKeyword("_EmissionColor");
        detectorMat.SetColor("_EmissionColor", newColor);
        detectorMat.EnableKeyword("_BaseColor");
        detectorMat.SetColor("_BaseColor", new Color(newColor.r, newColor.g, newColor.b, 0.25f));
    }
}
