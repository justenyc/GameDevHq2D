using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifier : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    [SerializeField] Dictionary<string, GameObject> weaponsDictionary = new Dictionary<string, GameObject>();
    [SerializeField] float weaponChangeDuration = 5f, lifeTime = 0;
    [SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in weapons)
            weaponsDictionary.Add(go.name, go);
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
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            player.SetProjectile(weaponsDictionary["Laser"]);
            lifeTime = Mathf.Clamp(lifeTime, 0, weaponChangeDuration);
        }
    }

    public void SetNewProjectile(string newProjectile)
    {
        BoostLifetime(weaponChangeDuration);
        player.SetProjectile(weaponsDictionary[newProjectile]);
    }
}
