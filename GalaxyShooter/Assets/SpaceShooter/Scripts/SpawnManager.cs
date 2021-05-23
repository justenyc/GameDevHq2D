using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemy;

    [SerializeField]
    GameObject[] PowerUps;

    public static SpawnManager instance;

    public delegate void entityDeath();
    public event entityDeath enemyDeath;

    private void Awake()
    {
        SingletonCheck();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enemy != null)
        {
            StartCoroutine(Spawn());
        }
        else
        {
            Debug.LogError("Enemy Prefab in SpawnManager not found");
        }

        StartCoroutine(SpawnAmmo());
    }

    void EnemyDeathHandler()
    {
        if (enemyDeath != null)
        {
            enemyDeath();
        }
    }

    GameObject GetRandomPowerUp(bool NeedAmmo)
    {
        if (NeedAmmo == false)
        {
            int index = (int)Mathf.Round(Random.Range(0, PowerUps.Length));
            return PowerUps[index];
        }
        else
        {
            GameObject ammo = PowerUps[3];
            return ammo;
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3f);

        if (FindObjectOfType<Player>())
        {
            Vector3 spawnPoint = new Vector3(Random.Range(0.1f, 0.9f), 1, 10);
            Vector3 convertPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
            float random = Random.Range(0, 100);

            if (random < 1)
            {
                GameObject newEnemy = Instantiate(enemy, convertPoint, enemy.transform.rotation);
                newEnemy.transform.parent = this.gameObject.transform;
                newEnemy.GetComponent<Enemy>().myDeath += EnemyDeathHandler;
            }
            else
            {
                GameObject powerUpToSpawn = GetRandomPowerUp(false);
                GameObject powerUp = Instantiate(powerUpToSpawn, convertPoint, enemy.transform.rotation);
                powerUp.transform.parent = this.gameObject.transform;
            }

            StartCoroutine(Spawn());
        }
        else
        {
            foreach (Enemy en in GetComponentsInChildren<Enemy>())
            {
                Destroy(en.gameObject);
            }
        }
    }

    IEnumerator SpawnAmmo()
    {
        yield return new WaitForSeconds(3f);
        
        Vector3 spawnPoint = new Vector3(Random.Range(0.1f, 0.9f), 1, 10);
        Vector3 convertPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
        float random = Random.Range(0, 100);

        if (random < 10)
        {
            GameObject ammoPU = Instantiate(PowerUps[3], convertPoint, enemy.transform.rotation);
            ammoPU.transform.parent = this.gameObject.transform;
        }
        StartCoroutine(SpawnAmmo());
    }

    void SingletonCheck()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
