using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : PowerUp
{
    [SerializeField]
    float bonusSpeed = 5, lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        Speed[] stacks = GetComponents<Speed>();
        lifeTime = base.GetDuration();

        if (stacks.Length > 1)
        {
            stacks[0].BoostLifetime(base.GetDuration());
            Destroy(this);
        }
        else
        {
            GetPlayer().AddSpeed(bonusSpeed);
        }
    }

    private void Update()
    {
        Countdown();
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
                GetPlayer().AddSpeed(-bonusSpeed);
                Destroy(this);
            }
        }
        else if (base.GetDuration() == 0)
            lifeTime = 99;
    }
}
