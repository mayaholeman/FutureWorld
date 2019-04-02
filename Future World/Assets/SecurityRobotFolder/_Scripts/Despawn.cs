using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float despawnTime = 120;

    int internalTimer = 0;

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        internalTimer++;
        if(internalTimer == despawnTime)
        {
            Destroy(this.gameObject);
        }
    }
}
