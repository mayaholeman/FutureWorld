using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityRobotMovement : MonoBehaviour, Target
{

	[Header("Set in Inspector")]
	public GameObject me;

	private float health = 65f;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public void Die()
    {
        Destroy(me);
    }

    public void takeDamage(float amount)
    {
        Debug.Log("Security Bot took " + amount + " damage");
        this.health -= amount;
        if (this.health <= 0)
        {
            Die();
        }
    }
}
