using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : MonoBehaviour, Target {

    [Header("Set in Inspector")]
    public GameObject me;

    protected float health = 15f;
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

    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void takeDamage(float amount)
    {
        Debug.Log("Standard Bot took " + amount + " damage");
        this.health -= amount;
        if (this.health <= 0)
        {
            Die();
        }
    }
}
