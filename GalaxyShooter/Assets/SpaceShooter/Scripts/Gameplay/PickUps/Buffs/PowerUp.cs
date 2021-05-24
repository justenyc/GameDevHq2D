using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    float duration = 5f;

    Player player;
    // Start is called before the first frame update
    public void Start()
    {
        try
        {
            player = this.GetComponent<Player>();
        }
        catch
        {
            Debug.LogError("Player component not found");
            Destroy(this);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public Player GetPlayer()
    {
        return player;
    }

    public float GetDuration()
    {
        return duration;
    }

    public void SetDuration(float dur)
    {
        duration = dur;
    }
}
