using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CircleSensor : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 5;
    float rotateTime;
    SpriteRenderer sr;
    Transform player, follow;
    SphereCollider sc;

    public delegate void SensorDetection(Transform t);
    public event SensorDetection detectedPlayer;
    public event SensorDetection playerLost;

    // Start is called before the first frame update
    void Start()
    {
        InitializePrivateVariables();
    }

    // Update is called once per frame
    void Update()
    {
        CircleAlphaByDistance();
        Follow();
    }

    void CircleAlphaByDistance()
    {
        if (player != null)
        {
            float distanceAlphaMod = (sc.radius * transform.localScale.x * 1.25f) / Vector3.Distance(this.transform.position, player.position);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 1 * rotateTime));
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, distanceAlphaMod);
        }
    }

    void Follow()
    {
        if (follow != null)
        {
            transform.position = follow.position;
        }
    }

    void FollowDeathHandler()
    {
        Destroy(this.gameObject);
    }

    void InitializePrivateVariables()
    {
        rotateTime = rotateSpeed * Time.deltaTime;
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>().transform;
        sc = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower() == "player")
        {
            sr.color = Color.red;
            
            if (detectedPlayer != null)
            {
                detectedPlayer(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.ToLower() == "player")
        {
            sr.color = Color.white;
            
            if (playerLost != null)
            {
                playerLost(other.transform);
            }
        }
    }

    public void SetFollow(Rammer r)
    {
        follow = r.transform;
        r.myDeath += FollowDeathHandler;
    }
}
