using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    [SerializeField] GameObject uiDisplay;

    Player player;
    // Start is called before the first frame update
    public void Start()
    {
        player = this.GetComponent<Player>();
        uiDisplay.transform.parent = UiManager.instance.GetPowerupDisplay();
        //uiDisplay.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public Player GetPlayer()
    {
        return player;
    }

    public float GetDuration()
    {
        return duration;
    }

    public void UpdateDisplay(float value)
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

    public void SetDuration(float dur)
    {
        duration = dur;
    }

    public void SetUiDisplay(GameObject go)
    {
        uiDisplay = go;
    }
}
