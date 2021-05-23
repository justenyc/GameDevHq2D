using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Rico : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float lifeTime = 5f;
    Material mat;

    AudioSource aSource;

    // Start is called before the first frame update
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
        aSource.pitch = Random.Range(0.5f, 1.5f);
        mat = this.GetComponent<TrailRenderer>().material;
        mat.EnableKeyword("_EmissionColor");
        mat.SetColor("_EmissionColor", new Color(0, 0.5f, 1));
        mat.EnableKeyword("_Color");
        mat.SetColor("_Color", new Color(0, 0.5f, 1));

        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    float CalculateRotationAngle(Transform target)
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;
        float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;
        return angle - 90;
    }

    Collider FindNearestEnemy(Collider[] objects, Collider initialEnemy)
    {
        Collider nearest = null;

        foreach (Collider col in objects)
        {
            if (col.gameObject.tag.ToLower() == "enemy" && col != initialEnemy)
            {
                if (nearest == null)
                    nearest = col;
                else if (Vector3.Distance(col.transform.position, transform.position) < Vector3.Distance(nearest.transform.position, transform.position))
                    nearest = col;
            }
        }
        return nearest;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Collider[] nearestObjects = Physics.OverlapSphere(transform.position, 100f);
            Collider nearest = FindNearestEnemy(nearestObjects, other);

            if (nearest == other || nearest == null)
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<CapsuleCollider>().enabled = false;
                this.GetComponent<TrailRenderer>().enabled = false;
            }
            else if (nearest != null)
            {
                float angle = CalculateRotationAngle(nearest.transform);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }
}