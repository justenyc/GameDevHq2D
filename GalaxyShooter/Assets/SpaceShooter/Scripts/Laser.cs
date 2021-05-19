using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lifeTime = 5f;

    AudioSource aSource;
    // Start is called before the first frame update
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);

        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CapsuleCollider>().enabled = false;
            this.GetComponent<TrailRenderer>().enabled = false;
        }
    }
}
