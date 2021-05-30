using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Enemy
{
    //Enemy moves towards a random side at start and fires in fireRate intervals, 
    //but fires instantly if it detects a powerup
    [SerializeField] GameObject laser_enemy;
    [SerializeField] float fireRate = 3f; //How often it normally fires in seconds
    [SerializeField] float fireRateCD; //Separate CD for firing at Power Ups

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        fireRateCD = fireRate;
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0, 0);

        if (laser_enemy == null)
            Debug.LogError("Enemy Laser not set");

        StartCoroutine(FireLaser(fireRate));
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, -Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        fireRateCD -= Time.deltaTime;
        Mathf.Clamp(fireRateCD, 0, fireRate);

        base.Movement();
        CheckForPickUpInFront();
    }

    IEnumerator FireLaser(float intervalInSeconds)
    {
        yield return new WaitForSeconds(intervalInSeconds);
        GameObject laser = Instantiate(laser_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        laser.GetComponent<Laser_Enemy>().SetTrailColor(Color.red);
        StartCoroutine(FireLaser(intervalInSeconds));
    }

    void CheckForPickUpInFront()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            if (hit.collider.tag.ToLower() == "powerup")
            {
                if (fireRateCD <= 0)
                {
                    GameObject laser = Instantiate(laser_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    laser.GetComponent<Laser_Enemy>().SetTrailColor(Color.red);
                    fireRateCD = fireRate;
                }
            }
        }
    }
}
