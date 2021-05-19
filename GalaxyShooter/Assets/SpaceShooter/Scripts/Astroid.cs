using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 1f;

    [SerializeField]
    GameObject explosion, spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        Quaternion someQuaternion = transform.rotation;
        Vector3 position = someQuaternion.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, rotation.z + rotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "laser")
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            spawnManager.GetComponent<SpawnManager>().enabled = true;
            Destroy(this.gameObject);
        }
    }
}
