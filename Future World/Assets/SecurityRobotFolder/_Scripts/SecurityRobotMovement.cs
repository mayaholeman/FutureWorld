using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityRobotMovement : MonoBehaviour
{
    Transform player;                   // Reference to the player's position.
    UnityEngine.AI.NavMeshAgent nav;    // Reference to the nav mesh agent.
    Animator anim;                      // Reference to the animator component.


    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        if (dist > 2)
        {
            // Tell the animator whether or not the robot is moving.
            anim.SetBool("IsMoving", true);

            // Set the destination of the nav mesh agent to the player.
            nav.SetDestination(player.position);
        }
        else
        {
            nav.enabled = false;

            // Tell the animator whether or not the robot is moving.
            anim.SetBool("IsMoving", false);
        }
        
    }
}
