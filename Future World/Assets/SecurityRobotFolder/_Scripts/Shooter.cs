using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    int internalTimer = 0;

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        internalTimer++;

        if(internalTimer == 20)
        {
            FireProjectile();
            internalTimer = 0;
        }
    }

    void FireProjectile()
    {
        GameObject proj = Instantiate<GameObject>(projectilePrefab);
        proj.transform.position = transform.position;
        Rigidbody rigidB = proj.GetComponent<Rigidbody>();
        Vector3 direction = new Vector3(0, 0, -1);
        rigidB.velocity = direction * projectileSpeed;
    }
}
