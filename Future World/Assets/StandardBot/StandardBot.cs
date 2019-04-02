using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : MonoBehaviour {

    [Header("Set in Inspector")]
    public GameObject me;

    protected int health = 15;
    protected float movementSpeed = 2f;

    public void DealDamage(int damage)
    {
        health -= damage;
        if (health < 0) {
            health = 0;
        }
        print(damage + " damage dealt. (" + health + " remaining)");
        
        if (health == 0)
        {
            print("Robot destroyed");
            Destroy(me);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;

        if (go.tag == "Shooties")
        {
            Destroy(go);
            DealDamage(10);
        }
    }
}
