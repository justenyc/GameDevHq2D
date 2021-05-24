using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("Text Objects")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI GameOverText; 
    [SerializeField] TextMeshProUGUI ammoDisplay;
    [SerializeField] TextMeshProUGUI waveAnnouncer;

    [Header("Image Objects")]
    [SerializeField] Image lives_display; 
    [SerializeField] Image shields_display;
    [SerializeField] Image GameOverScreen;
    [SerializeField] Image fuelBar;


    [Header ("Game Manager Properties")]
    [SerializeField] float currentScore = 0f;
    [SerializeField] float enemyScoreValue = 5f;
    [SerializeField] float bossScoreValue = 100f;

    [SerializeField] Sprite[] lives;

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

    public void UpdateAmmoDisplay(int ammo)
    {
        ammoDisplay.text = ammo.ToString() + "/15";
    }

    public void UpdateFuelDisplay(float amount)
    {
        RectTransform rt = fuelBar.GetComponent<RectTransform>();
        rt.localScale = new Vector3(amount, rt.localScale.y, rt.localScale.z);
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
    
    public void UpdateWaveAnnouncer(int wave)
    {
        Animator anim = waveAnnouncer.GetComponent<Animator>();
        waveAnnouncer.text = "Wave " + wave;
        anim.Play("WaveDisplay", -1, 0f);
    }
}
