using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] Transform First, Second;
    [SerializeField] float scrollSpeed = 1f, verticalDifference = 21.6f, minimumHeight = -15, minimumWidth, horizontalDifference;
    [SerializeField] int directionX = 0, directionY = -1; 

    // Update is called once per frame
    void Update()
    {
        First.position = new Vector3(First.position.x + (directionX * Time.deltaTime * scrollSpeed), First.position.y + (directionY * Time.deltaTime * scrollSpeed), First.position.z);
        Second.position = new Vector3(Second.position.x + (directionX * Time.deltaTime * scrollSpeed), Second.position.y + (directionY * Time.deltaTime * scrollSpeed), Second.position.z);
        Repeat();
    }

    void Repeat()
    {
        if (CheckExtremes(First.localPosition.x, directionX, minimumWidth)  || CheckExtremes(First.localPosition.y, directionY, minimumHeight))
        {
            First.localPosition = new Vector3(Second.localPosition.x + horizontalDifference * -Mathf.Sign(directionX), Second.localPosition.y + verticalDifference * -Mathf.Sign(directionY), Second.localPosition.z);
        }

        if (CheckExtremes(Second.localPosition.x, directionX, minimumWidth) || CheckExtremes(Second.localPosition.y, directionY, minimumHeight))
        {
            Second.localPosition = new Vector3(First.localPosition.x + horizontalDifference * -Mathf.Sign(directionX), First.localPosition.y + verticalDifference * -Mathf.Sign(directionY), First.localPosition.z);
        }
    }

    bool CheckExtremes(float localPositionAxis, int direction, float extreme)
    {
        if (Mathf.Sign(direction) < 0)
        {
            return (localPositionAxis < Mathf.Sign(direction) * extreme);
        }
        else if (Mathf.Sign(direction) > 0)
        {
            return (localPositionAxis > Mathf.Sign(direction) * extreme);
        }
        return false;
    }
}
