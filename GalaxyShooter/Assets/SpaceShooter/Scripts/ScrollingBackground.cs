using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] Transform Top, Bottom;
    [SerializeField] float scrollSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        Top.position = new Vector3(Top.position.x, Top.position.y - Time.deltaTime * scrollSpeed, Top.position.z);
        Bottom.position = new Vector3(Bottom.position.x, Bottom.position.y - Time.deltaTime * scrollSpeed, Bottom.position.z);
        Repeat();
    }

    void Repeat()
    {
        if (Top.localPosition.y < -15)
        {
            Top.position = new Vector3(Bottom.position.x, Bottom.position.y + 21.6f, Bottom.position.z);
        }

        if (Bottom.localPosition.y < -15)
        {
            Bottom.position = new Vector3(Top.position.x, Top.position.y + 21.6f, Top.position.z);
        }
    }
}
