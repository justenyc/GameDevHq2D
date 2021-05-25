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
        if (other.tag.ToLower().Contains("enemy"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int amount)
    {
        AudioSource aSource = this.GetComponent<AudioSource>();
        UiManager.instance.UpdateShieldDisplay(-strength);

        strength -= amount;
        if (strength < 1)
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;

            Destroy(this.gameObject, 2f);
        }

        aSource.pitch = Random.Range(0.5f, 1.5f);
        aSource.Stop();
        aSource.Play();
    }

    public void AddStrength(int amount)
    {
        strength += amount;
        strength = Mathf.Clamp(strength, 0, 3);
        UiManager.instance.UpdateShieldDisplay(strength);
    }
}
