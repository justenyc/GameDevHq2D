using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Bubble : MonoBehaviour
{
    [SerializeField] int strength = 1;
    private void Start()
    {
        UiManager.instance.UpdateShieldDisplay(strength);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "enemy")
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int amount)
    {
        UiManager.instance.UpdateShieldDisplay(-strength);

        strength -= amount;
        if (strength < 1)
            Destroy(this.gameObject);
    }

    public void AddStrength(int amount)
    {
        strength += amount;
        strength = Mathf.Clamp(strength, 0, 3);
        UiManager.instance.UpdateShieldDisplay(strength);
    }
}
