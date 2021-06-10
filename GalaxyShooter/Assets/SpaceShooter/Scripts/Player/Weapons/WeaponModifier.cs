using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponModifier : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    [SerializeField] Dictionary<string, GameObject> weaponsDictionary = new Dictionary<string, GameObject>();
    [SerializeField] float weaponChangeDuration = 5f, lifeTime = 0;
    [SerializeField] Player player;
    [SerializeField] GameObject uiDisplay;

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

    public void AddUiDisplayToUi()
    {
        uiDisplay.transform.parent = UiManager.instance.GetPowerupDisplay();
        //uiDisplay.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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

        if (uiDisplay != null)
        {
            UpdateUiDisplay(lifeTime);
        }
    }

    public void SetNewProjectile(string newProjectile)
    {
        BoostLifetime(weaponChangeDuration);
        player.SetProjectile(weaponsDictionary[newProjectile]);
    }

    public void SetUiDisplay(GameObject go)
    {
        if (uiDisplay == null)
        {
            uiDisplay = go;
            AddUiDisplayToUi();
        }
    }

    void UpdateUiDisplay(float value)
    {
        value = Mathf.RoundToInt(value);
        if (value > 0)
        {
            TextMeshProUGUI text = uiDisplay.GetComponentInChildren<TextMeshProUGUI>();
            text.text = value.ToString();
        }
        else
            Destroy(uiDisplay);
    }
}
