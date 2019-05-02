using System.Collections;
using System.Collections.Generic;
using System.Timers;
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
    float visionRange = 20f;
    bool seenPlayer = false;

    //Shooting variables
    public float shootingRange = 200f;
    GameObject robotGun;
    int shootDelayCounter;
    float gunDamage = 10f;

    private System.Timers.Timer aTimer;

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            //childTrans.gameObject.SetActive(false);
            return childTrans.gameObject;
        }
        else
        {
            print("Cannot find child named " + name);
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
        robotGun = GetChildWithName(this.gameObject, "AssaultRifle");
        shootDelayCounter = Random.Range(0, 60);
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        shootDelayCounter = ((shootDelayCounter + 1) % 60);
        if (!seenPlayer)
        {
            RaycastHit vision;
            Vector3 direction = gameObject.transform.forward;
            direction.z = direction.z - 15 + (shootDelayCounter * 0.5f);
            Vector3 pos = gameObject.transform.position;
            pos.y = pos.y + 0.6f;
            Physics.Raycast(pos, direction, out vision, visionRange);
            Debug.DrawRay(pos, direction * 10f, Color.red);
            //Physics.SphereCast(gameObject.transform.position, .3f, gameObject.transform.forward, out vision, 100f);
            GameObject visionTarget = vision.collider.gameObject;
            if (visionTarget.tag == "Katya") { seenPlayer = true; }
            //if(dist < 2) { seenPlayer = true; }
        }
        else
        {
            if (dist > 3)
            {
                // Tell the animator whether or not the robot is moving.
                nav.enabled = true;
                anim.SetBool("IsMoving", true);
                anim.SetBool("IsShooting", false);

                // Set the destination of the nav mesh agent to the player.
                nav.SetDestination(player.position);
            }
            else if (dist < 3)
            {
                nav.enabled = false;
                this.gameObject.transform.LookAt(player.transform);
                // Tell the animator whether or not the robot is moving.
                //anim.SetBool("IsMoving", false);

                if(shootDelayCounter == 0)
                {
                    anim.SetBool("IsShooting", true);
                    Shoot(robotGun, gunDamage);
                } else if (shootDelayCounter == 10)
                {
                    anim.SetBool("IsShooting", false);
                }
                else
                {
                    anim.SetBool("IsMoving", false);
                }
            }
        }
    }

    private void Shoot(GameObject gun, float damage)
    {
        RaycastHit hit;
        Vector3 gunPos = gun.transform.position;
        gunPos.y = gunPos.y + 0.6f;
        if (Physics.Raycast(gunPos, gun.transform.forward, out hit, shootingRange))
        {
            Debug.Log(hit.transform.name);
            Debug.DrawRay(gunPos, gun.transform.forward * shootingRange, Color.green);
            Target target = hit.transform.GetComponent<Target>();
            if(hit.transform.name == "Katya")
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
            print("Die Robot");
            anim.SetBool("IsDead", true);
            StartCoroutine(Death());
            
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(0.8f);
        Die();
    }
}
