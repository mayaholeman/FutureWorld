using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titan : MonoBehaviour, Target {

	[Header("Set in Inspector")]
	public Transform target;
	public LineRenderer beam;
	public Material eyeMaterial;

    protected float health = 200f;
    protected float movementSpeed = 2f;
    protected int SHOOTCTR_LIM = 300;
    protected int AIMING_DUR = 100;
    protected int SHOOTING_DUR = 25;
    protected float FOV = 70f;

    protected bool seesPlayer = true;

	public int level = 2;

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
    	beam = Instantiate(beam);
    }

    void Aim(Vector3 targetPos) {
    	// Debug.Log("Aiming");

    	beam.enabled = false;
    	beam.startWidth = 0.1f;
    	beam.endWidth = 0.5f; // probably more like 0.5f;

    	var points = new Vector3[2];

    	points[0] = transform.position + new Vector3(0, -1f, 0);
    	points[1] = targetPos + new Vector3(0, 0.25f, 0);

    	beam.SetPositions(points);

    	// So targetPos will actually be used to calculate the angle for which to send the Raycast.
    	// Once Raycast is completed, end position of beam will actually be wherever the Raycast hits.
    }

    void Shoot() {
    	RaycastHit hit;

    	Vector3 source = beam.GetPosition(0);
    	Vector3 lastLocation = beam.GetPosition(1);

    	Vector3 direction = (source - lastLocation).normalized;

    	if (Physics.Raycast(source, -direction, out hit, 1000f)) {
    		var points = new Vector3[2];
    		points[0] = source;
    		points[1] = hit.point;
    		beam.SetPositions(points);

    		Debug.Log("Hit " + hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null && hit.transform.name == "Katya")
            {
                target.takeDamage(70);
            }
    	}

    	beam.enabled = true;
    }

    void Update() {
    	if (shootctr > 0) {
	    	Vector3 direction = target.position - transform.position;
	    	float angle = Vector3.Angle(direction, transform.forward);
	    	// Debug.Log("Angle: " + angle);
	    	if (angle <= FOV) {
	    		RaycastHit hit;
	    		if (Physics.Raycast(transform.position, direction, out hit, 1000f)) {
	    			if (hit.transform.name == "Katya") {
	    				seesPlayer = true;
	    			} else {
	    				seesPlayer = false;
	    			}
	    		}
	    	} else {
	    		seesPlayer = false;
	    	}
	    }

    	if (!seesPlayer) {
    		shootctr = SHOOTCTR_LIM;
			aiming = AIMING_DUR;
			shooting = SHOOTING_DUR;
			beam.enabled = false;
			eyeMaterial.SetColor("_Color", new Color(0.1f, 0.8f, 0.1f, 1f));
			transform.Rotate(0, Time.deltaTime * movementSpeed * 20, 0, Space.Self);
    	} else {
    		eyeMaterial.SetColor("_Color", new Color(0.7f, 0.2f, 0.2f, 1f));
	    	if (shootctr > 0) {
		    	Vector3 lookPos = target.position - transform.position;
				lookPos.y = 0;
				Quaternion rotation = Quaternion.LookRotation(lookPos);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * movementSpeed);
				shootctr--;
			}
		}

		if (shootctr <= 0) {
			// freeze position, shoot beam at last Katya location
			if (aiming == AIMING_DUR) {
				Aim(target.position);
				eyeMaterial.SetColor("_Color", new Color(0f, 0.2f, 0.2f, 1f));
			}
			aiming--;
			if (aiming > 0) {
				float red = (AIMING_DUR - aiming)/100f;
				eyeMaterial.SetColor("_Color", new Color(red * 0.7f, 0.2f, 0.2f, 1f));
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
