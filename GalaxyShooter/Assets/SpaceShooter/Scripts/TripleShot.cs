using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : PowerUp
{
    [SerializeField]
    Transform point1, point2;

    [SerializeField]
    float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        lifeTime = base.GetDuration();
        point1 = GetPlayer().GetTransforms()[0];
        point2 = GetPlayer().GetTransforms()[1];

        TripleShot[] stacks = GetComponents<TripleShot>();

        if (stacks.Length > 1)
        {
            stacks[0].BoostLifetime(base.GetDuration());
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputListener();
        Countdown();
    }

    void InputListener()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GetPlayer().GetCanFire() == true)
        {
            try
            {
                GameObject projectile = GetPlayer().GetProjectile();
                Instantiate(projectile, point1.position, projectile.transform.rotation);
                Instantiate(projectile, point2.position, projectile.transform.rotation);
            }
            catch
            {
                Debug.LogError("Prefab not found");
            }
        }
    }

    public void BoostLifetime(float boost)
    {
        lifeTime += boost;
    }

    void Countdown()
    {
        if (base.GetDuration() > 0)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
            {
                Destroy(this);
            }
        }
        else if (base.GetDuration() == 0)
            lifeTime = 99;
    }
}
