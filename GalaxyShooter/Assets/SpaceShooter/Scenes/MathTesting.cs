using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTesting : MonoBehaviour
{
    public Transform target;
    public Transform self;
    public float xDistance;
    public float yDistance;
    public float hypotenuse;
    public float angle;
    public float final;

    // Start is called before the first frame update
    void Start()
    {
        xDistance = target.position.x - self.position.x;
        yDistance = target.position.y - self.position.y;
        angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;
        Debug.Log("xDistance is: " + xDistance + " | " +
                  "yDistance is: " + yDistance + " | " +
                  "angle is: " + angle);
        final = 360 + angle - 90;
        Debug.Log(final);
    }
}
