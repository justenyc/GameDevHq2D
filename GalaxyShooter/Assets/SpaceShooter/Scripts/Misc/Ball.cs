using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        mat.EnableKeyword("_BaseColor");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        mat.SetColor("_BaseColor", Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        mat.SetColor("_BaseColor", Color.green);
    }
}
