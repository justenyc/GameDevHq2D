using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Bubble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
