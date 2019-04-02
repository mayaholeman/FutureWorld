using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, Target
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    public float speed = 12f;
    public float turnSpeed = 180f;

	private float turnInputValue;

    private Actions actions;
    private Animator animator;
    private Vector3 movement;
    private Rigidbody playerRigidbody;

	public ParticleSystem muzzleFlash;

	public float damage = 10f;
	public float health = 100f;
	public float range = 100f;

	private float yaw = 0.0f;

	private int arsenalIndex = 0;

    void Awake()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
		if (arsenal.Length > 0)
			SetArsenal(arsenal[0].name);
	}

	private void OnEnable()
    {
        playerRigidbody.isKinematic = false;
        turnInputValue = 0f;
    }

    private void OnDisable()
    {
        playerRigidbody.isKinematic = true;
    }

    public void SetArsenal(string name)
    {
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == name)
            {
                if (rightGunBone.childCount > 0)
                    Destroy(rightGunBone.GetChild(0).gameObject);
                if (leftGunBone.childCount > 0)
                    Destroy(leftGunBone.GetChild(0).gameObject);
                if (hand.rightGun != null)
                {
                    GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                    newRightGun.transform.parent = rightGunBone;
                    newRightGun.transform.localPosition = Vector3.zero;
                    newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
                if (hand.leftGun != null)
                {
                    GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                    newLeftGun.transform.parent = leftGunBone;
                    newLeftGun.transform.localPosition = Vector3.zero;
                    newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                }
				animator.runtimeAnimatorController = hand.controller;
				return;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            actions.Sitting();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            actions.GetUp();
        }

		if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && Input.GetKeyDown(KeyCode.Space))
		{
			print("walking and jump hit");
			actions.Jump();
		}
		else if (Input.GetKeyDown(KeyCode.Space))
		{
			print("jump hit");
			actions.Jump();
		}
		else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			print("walking");
			actions.Walk();
		}
		else
		{
			actions.Stay();
		}

		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Left button down");
			actions.Attack();
			if (arsenalIndex != 0)
			{
				Shoot();
			}
			
		}
		if (Input.GetMouseButton(1))
		{
			Debug.Log("Right button down");
			actions.Aiming();
			
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			SwitchWeapon();
		}

	}

	private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
		gameObject.transform.position += movement * speed * Time.deltaTime;
    }

    private void Turn()
    {
		yaw += turnSpeed * Input.GetAxis("Mouse X");
		gameObject.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }

	private void SwitchWeapon()
	{
		arsenalIndex++;
		if (arsenalIndex == arsenal.Length) arsenalIndex = 0;
		SetArsenal(arsenal[arsenalIndex].name);
		
	}

	void Shoot()
	{
		muzzleFlash.Play();
		RaycastHit hit;
		if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, range))
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
		Destroy(gameObject);
	}

	public void takeDamage(float amount)
	{
		this.health -= amount;
		if (this.health <= 0)
		{
			actions.Death();
		}
		else
		{
			actions.Damage();
		}

	}

	private void OnCollisionEnter(Collision collision)
    {
        // TODO: Add collision handling depending on tags of objects
        actions.Damage();
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
    }
}
