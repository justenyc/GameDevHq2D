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
    TextMeshProUGUI scoreText, GameOverText;

    [SerializeField]
    Image lives_display, GameOverScreen;

    [SerializeField]
    float currentScore = 0f, enemyScoreValue = 5f, bossScoreValue = 100f;

    [SerializeField]
    Sprite[] lives;

    // Start is called before the first frame update
    void Awake()
    {
        SingletonCheck();
    }

    private void Start()
    {
        SpawnManager.instance.enemyDeath += EnemyDeathHandler;
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
}
