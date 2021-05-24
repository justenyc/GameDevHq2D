using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithRandomColour : MonoBehaviour
{
    public Color randomColour;
    public SpriteRenderer sr;
    public float intervalSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        randomColour = new Color(Random.Range(0, 2f), Random.Range(0, 2f), Random.Range(0, 2f));
        StartCoroutine(NewRandomColour());
    }

    IEnumerator NewRandomColour()
    {
        yield return new WaitForSeconds(1f);
        randomColour = Color.white;
        sr.color = randomColour;
        yield return new WaitForSeconds(1f);
        randomColour = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        sr.color = randomColour;
        StartCoroutine(NewRandomColour());
    }

    Vector3 ColorToVector3(Color c)
    {
        return new Vector3(c.r, c.g, c.b);
    }

    Color Vector3ToColor(Vector3 v3)
    {
        return new Color(v3.x, v3.y, v3.z);
    }
}
