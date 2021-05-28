using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Enemy
{
    [SerializeField] GameObject laser_enemy;
    [SerializeField] float fireRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0, 0);

        if (laser_enemy == null)
            Debug.LogError("Enemy Laser not set");

        StartCoroutine(FireLaser(fireRate));
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, -Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        base.Movement();
    }

    IEnumerator FireLaser(float intervalInSeconds)
    {
        yield return new WaitForSeconds(intervalInSeconds);
        GameObject laser = Instantiate(laser_enemy, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        laser.GetComponent<Laser_Enemy>().SetTrailColor(Color.red);
        StartCoroutine(FireLaser(intervalInSeconds));
    }
}
