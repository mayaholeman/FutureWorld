using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : MonoBehaviour, Target {

	[Header("Set in Inspector")]
	public Transform target;
	public LineRenderer beam;

    protected float health = 200f;
    protected float movementSpeed = 2f;
    protected int SHOOTCTR_LIM = 300;
    protected int AIMING_DUR = 100;
    protected int SHOOTING_DUR = 25;
	public int level = 2;

    protected bool seesPlayer = true;

    private int shootctr;
    private int aiming;
    private int shooting;

    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

	public int Level
	{
		get { return this.level; }
		set { this.level = value; }
	}

	public void Die()
    {
        Destroy(gameObject);
    }

    public void takeDamage(float amount)
    {
        Debug.Log("Titan took " + amount + " damage");
        this.health -= amount;
        if (this.health <= 0)
        {
            Die();
        }
    }

    void Start() {
    	aiming = AIMING_DUR;
    	shootctr = SHOOTCTR_LIM;
    	shooting = SHOOTING_DUR;
    	beam.enabled = false;
    }

    void Aim(Vector3 targetPos) {
    	// Debug.Log("Aiming");

    	beam.enabled = false;
    	beam.startWidth = 0.1f;
    	beam.endWidth = 0.75f;

    	var points = new Vector3[2];

    	points[0] = transform.position + new Vector3(0, -1f, 0);
    	points[1] = targetPos;

    	beam.SetPositions(points);

    	// So targetPos will actually be used to calculate the angle for which to send the Raycast.
    	// Once Raycast is completed, end position of beam will actually be wherever the Raycast hits.
    }

    void Shoot() {
    	RaycastHit hit;

    	// Vector3 source = beam.GetPosition(0);
    	// Vector3 lastLocation = beam.GetPosition(1);

    	// if (Physics.Raycast(source, lastLocation, out hit, 1000f)) {
    	// 	var points = new Vector3[2];
    	// 	points[0] = source;
    	// 	points[1] = hit.transform.position;
    	// 	beam.SetPositions(points);

    	// 	Debug.Log("Hit " + hit.transform.name);
     //        Target target = hit.transform.GetComponent<Target>();
     //        if (target != null)
     //        {
     //            target.takeDamage(70);
     //        }
    	// }

    	// beam.enabled = true;
    }

    void Update() {
    	if (!seesPlayer) {
    		shootctr = SHOOTCTR_LIM;
			aiming = AIMING_DUR;
			shooting = SHOOTING_DUR;
			beam.enabled = false;
    	}

    	if (seesPlayer && shootctr > 0) {
	    	Vector3 lookPos = target.position - transform.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
			shootctr--;
		}

		if (shootctr <= 0) {
			// freeze position, shoot beam at last Katya location
			if (aiming == AIMING_DUR) {
				Aim(target.position);
			}
			aiming--;
			if (aiming > 0) {
				
			}
			if (aiming <= 0) {
				if (shooting == SHOOTING_DUR) {
					Shoot();
				}
				shooting--;

				if (shooting <= 0) {
					shootctr = SHOOTCTR_LIM;
					aiming = AIMING_DUR;
					shooting = SHOOTING_DUR;
					beam.enabled = false;
					// Debug.Log("Resuming");
				}
			}
		}
    }
}
