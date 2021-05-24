using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float lifeTime = 5f;
    Material mat;

    AudioSource aSource;
    // Start is called before the first frame update
    public void Start()
    {
        aSource = this.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);
        SetTrailColor(Color.green);

        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.ToLower().Contains("enemy"))
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    public void SetTrailColor(Color newColor)
    {
        mat = this.GetComponent<TrailRenderer>().material;
        mat.EnableKeyword("_EmissionColor");
        mat.SetColor("_EmissionColor", newColor);
        mat.EnableKeyword("_Color");
        mat.SetColor("_Color", newColor);
    }
}
