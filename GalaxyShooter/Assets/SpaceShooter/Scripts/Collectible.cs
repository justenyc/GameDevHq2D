using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    Type type = new Type();

    [SerializeField]
    float descentSpeed = 1, TripleShot_Duration = 5, Speed_Duration = 10;

    [SerializeField]
    GameObject Shield_Bubble;

    private void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + -descentSpeed * Time.deltaTime, transform.position.z);

        if (Camera.main.WorldToViewportPoint(transform.position).y < 0)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            Player p = other.GetComponent<Player>();
            switch (type)
            {
                case Type.TripleShot:
                    p.gameObject.AddComponent<TripleShot>().SetDuration(TripleShot_Duration);
                    OnCollect();
                    break;

                case Type.Shield:
                    try
                    {
                        if (p.GetComponentsInChildren<Shield_Bubble>().Length < 1)
                            Instantiate(Shield_Bubble, p.transform.position, Shield_Bubble.transform.rotation, p.gameObject.transform);

                        OnCollect();
                    }
                    catch
                    {
                        Debug.LogError("Bubble Prefab not found");
                    }
                    break;

                case Type.Speed:
                    p.gameObject.AddComponent<Speed>().SetDuration(Speed_Duration);
                    OnCollect();
                    break;

                default:
                    Debug.LogError("type not defined or player not found");
                    break;
            }
        }
        catch
        {
            Debug.LogError("Player not found");
        }
    }

    void OnCollect()
    {
        AudioSource aSource = this.GetComponent<AudioSource>();
        aSource.Stop();
        aSource.Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider>().enabled = false;
        Destroy(this.gameObject, 1);
    }

    enum Type
    {
        TripleShot,
        Shield,
        Speed
    };
}
