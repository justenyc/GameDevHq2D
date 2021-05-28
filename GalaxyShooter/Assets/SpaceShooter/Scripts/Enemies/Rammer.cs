using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rammer : Enemy
{
    [SerializeField] float ramSpeed = 5f;

    Enemy_CircleSensor mySensor;
    // Start is called before the first frame update
    void Start()
    {
        mySensor = GetComponentInChildren<Enemy_CircleSensor>();
        mySensor.detectedPlayer += PlayerDetectedHandler;
        mySensor.playerLost += PlayerLostHandler;
        mySensor.SetFollow(this);
        mySensor.transform.parent = null;

        base.Start();

        this.moveDirection = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        base.Movement();
    }

    public override void Die()
    {
        mySensor.detectedPlayer -= PlayerDetectedHandler;
        mySensor.playerLost -= PlayerLostHandler;
        base.Die();
    }

    void PlayerDetectedHandler(Transform player)
    {
        moveDirection = (player.position - transform.position).normalized;
        Debug.Log(moveDirection);

        float xDistance = player.position.x - transform.position.x;
        float yDistance = player.position.y - transform.position.y;
        float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        this.AddMoveSpeed(ramSpeed);
    }

    void PlayerLostHandler(Transform player)
    {
        this.moveDirection = Vector3.down;
        Debug.Log(moveDirection);
        this.AddMoveSpeed(-ramSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
