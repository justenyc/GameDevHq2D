using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField]
    TextMeshProUGUI scoreText, GameOverText, ammoDisplay;

    [SerializeField]
    Image lives_display, shields_display, GameOverScreen;

    [SerializeField]
    float currentScore = 0f, enemyScoreValue = 5f, bossScoreValue = 100f;

    [SerializeField]
    Sprite[] lives;

    public Image[] test;

    // Start is called before the first frame update
    void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        SpawnManager.instance.enemyDeath += EnemyDeathHandler;
        test = shields_display.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && GameOverScreen.gameObject.activeSelf)
        {
            SceneManager.LoadScene(0);
        }
    }

    void BossDeathHandler()
    {
        UpdateScore(bossScoreValue);
    }

    void DisplayGameOverScreen()
    {
        GameOverScreen.gameObject.SetActive(true);
        StartCoroutine(FlickerText());
    }

    void EnemyDeathHandler()
    {
        UpdateScore(enemyScoreValue);
    }

    IEnumerator FlickerText()
    {
        GameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        GameOverText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        StartCoroutine(FlickerText());
    }

    void SingletonCheck()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void UpdateAmmoDisplay(int ammo)
    {
        ammoDisplay.text = ammo.ToString() + "/15";
    }

    public void UpdateLivesDisplay(int value)
    {
        if (value >= 0)
        {
            lives_display.sprite = lives[value];
        }
        else
        {
            DisplayGameOverScreen();
        }    
    }

    void UpdateScore(float value)
    {
        currentScore += value;
        scoreText.text = "Score: " + currentScore;
    }

    public void UpdateShieldDisplay(int value)
    {
        Image[] displays = shields_display.GetComponentsInChildren<Image>();

        try
        {
            if (value > 0)
                displays[value].enabled = true;
            else
                displays[-value].enabled = false;
        }
        catch
        {
            Debug.Log(displays.Length);
        }
    }
}
