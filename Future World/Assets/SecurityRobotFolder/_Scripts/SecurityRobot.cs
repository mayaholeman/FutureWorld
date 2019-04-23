using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityRobot : MonoBehaviour, Target
{

	[Header("Set in Inspector")]
	//public GameObject me;
    
    //Target variables
	private float health = 65f;

    //Movement variables
    Transform player;                   // Reference to the player's position.
    UnityEngine.AI.NavMeshAgent nav;    // Reference to the nav mesh agent.
    Animator anim;                      // Reference to the animator component.
    float visionRange = 200f;
    bool seenPlayer = false;

    //Shooting variables
    public float shootingRange = 100f;
    GameObject robotGun;

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }

    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Katya").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.speed = 0.8f;
        anim = GetComponent<Animator>();
        robotGun = GetChildWithName(this.gameObject, "AssualtRifle");
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (!seenPlayer)
        {
            RaycastHit vision;
            //Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out vision, visionRange);
            Physics.SphereCast(gameObject.transform.position, 2f, gameObject.transform.forward, out vision, visionRange);
            GameObject visionTarget = vision.collider.gameObject;
            if (visionTarget.tag == "Katya") { seenPlayer = true; }
            //if(dist < 2) { seenPlayer = true; }
        }
        else
        {
            if (dist > 2)
            {
                // Tell the animator whether or not the robot is moving.
                nav.enabled = true;
                anim.SetBool("IsMoving", true);

                // Set the destination of the nav mesh agent to the player.
                nav.SetDestination(player.position);
            }
            else if (dist < 2)
            {
                nav.enabled = false;
                this.gameObject.transform.LookAt(player.transform);
                // Tell the animator whether or not the robot is moving.
                anim.SetBool("IsMoving", false);
            }
        }
    }

    private void Shoot(GameObject gun, float damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gameObject.transform.forward, out hit, shootingRange))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.takeDamage(damage);
            }
        }
    }

    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public void Die()
    {
        Destroy(this.gameObject);
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
