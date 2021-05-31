using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int waveNumber = 0;
    [SerializeField] int numberToSpawn;
    [SerializeField] int enemiesSpawned;
    [SerializeField] int enemiesDefeated = 0;

    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject boss;

    [SerializeField]
    GameObject[] PowerUps;

    [SerializeField] float SpawnRate = 3, spawnChance = 90;

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
        numberToSpawn = 10 + waveNumber * 2;
        NextWave();
    }

    void BeginSpawning()
    {
        if (waveNumber % 5 == 0)
        {
            Instantiate(boss, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.8f, -Camera.main.transform.position.z)), boss.transform.rotation, this.transform);
        }

        if (enemyPrefabs.Length > 0)
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
            enemiesDefeated++;
            enemyDeath();

            if (enemiesDefeated == numberToSpawn)
                NextWave();
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
            GameObject ammo = PowerUps[4];
            return ammo;
        }
    }

    void NextWave()
    {
        waveNumber++;
        numberToSpawn = 10 + waveNumber * 2;
        enemiesDefeated = 0;
        enemiesSpawned = 0;
        StopAllCoroutines();
        StartCoroutine(Wait(5f));
        UiManager.instance.UpdateWaveAnnouncer(waveNumber);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnRate);

        if (FindObjectOfType<Player>())
        {
            Vector3 spawnPoint = new Vector3(Random.Range(0.1f, 0.9f), 1, -Camera.main.transform.position.z);
            Vector3 convertPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
            float random = Random.Range(0, 100);

            if (random < spawnChance)
            {
                if (enemiesSpawned < numberToSpawn)
                {
                    int randomIndex = Mathf.RoundToInt(Random.Range(0, enemyPrefabs.Length));
                    GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex], convertPoint, enemyPrefabs[randomIndex].transform.rotation);
                    newEnemy.transform.parent = this.gameObject.transform;
                    newEnemy.GetComponent<Enemy>().myDeath += EnemyDeathHandler;
                    enemiesSpawned++;
                }
            }
            else
            {
                GameObject powerUpToSpawn = GetRandomPowerUp(false);
                GameObject powerUp = Instantiate(powerUpToSpawn, convertPoint, powerUpToSpawn.transform.rotation);
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
        yield return new WaitForSeconds(SpawnRate);
        
        Vector3 spawnPoint = new Vector3(Random.Range(0.1f, 0.9f), 1, -Camera.main.transform.position.z);
        Vector3 convertPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
        float random = Random.Range(0, 100);

        if (random < 25)
        {
            GameObject ammoPU = Instantiate(PowerUps[4], convertPoint, PowerUps[4].transform.rotation);
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

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        BeginSpawning();
    }
}
