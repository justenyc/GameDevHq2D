using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialListener : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(this.gameObject);
        }
    }
}
