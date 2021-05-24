using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] Transform First, Second;
    [SerializeField] float scrollSpeed = 1f, verticalDifference = 21.6f, minimumHeight = -15;

    // Update is called once per frame
    void Update()
    {
        First.position = new Vector3(First.position.x, First.position.y - Time.deltaTime * scrollSpeed, First.position.z);
        Second.position = new Vector3(Second.position.x, Second.position.y - Time.deltaTime * scrollSpeed, Second.position.z);
        Repeat();
    }

    void Repeat()
    {
        if (First.localPosition.y < minimumHeight)
        {
            First.localPosition = new Vector3(Second.localPosition.x, Second.localPosition.y + verticalDifference, Second.localPosition.z);
        }

        if (Second.localPosition.y < minimumHeight)
        {
            Second.localPosition = new Vector3(First.localPosition.x, First.localPosition.y + verticalDifference, First.localPosition.z);
        }
    }
}
