using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleLaserColours : MonoBehaviour
{
    [SerializeField] Colour colour;

    public GameObject laser;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newLaser = Instantiate(laser, transform.position, laser.transform.rotation);
            Material mat = newLaser.GetComponent<TrailRenderer>().material;
            ExampleMaterialManipulation(mat);
        }
    }

    void ExampleMaterialManipulation(Material mat)
    {
        Color someColour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        mat.EnableKeyword("_EmissionColor");
        mat.SetColor("_EmissionColor", someColour);
        mat.EnableKeyword("_Color");
        mat.SetColor("_Color", someColour);
    }

    enum Colour
    {
        Red,
        Blue,
        Purple,
        Green
    };
}
