using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour, Target
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;
    
    public Speed[] speeds;

    public Vector3 jump;

	public LayerMask interactionMask;	// Everything we can interact with

    public delegate void OnFocusChanged(Interactable newFocus);
	public OnFocusChanged onFocusChangedCallback;

	public Interactable focus;	// Our current focus: Item, Enemy etc.

    public float distanceSquared = 5f;
	public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public float impactForce = 100f;
    public float fireRate = 15f;

    public SimpleHealthBar healthBar;

    private float nextTimeToFire = 0f;
	public float health = 100f;
    public float maxHealth = 100f;
	public float range = 100f;
	private float yaw = 0.0f;
	private int arsenalIndex = 0;
    private bool stealthMode = false;
    
    private IDictionary<string, float> speedsDict;
    private string movementSpeedKey = "walk";

    private Actions actions;
    private Animator animator;
    private Vector3 movement;
    private Rigidbody playerRigidbody;


    private int level = 0;

    public GameObject enemies;
#region Singleton

	public static PlayerController instance;


	#endregion
    void Awake()
    {
        instance = this;
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        level = 1;
		if (arsenal.Length > 0)
			SetArsenal(arsenal[0].name);
        speedsDict = new Dictionary<string, float>();
        foreach (Speed item in speeds)
        {
            speedsDict.Add(item.name, item.speed);
        }
        jump = new Vector3(0.0f, 02.0f, 0.0f); 
        distanceSquared = (transform.position - Camera.main.transform.position).sqrMagnitude;

        healthBar.UpdateBar(this.health, this.maxHealth);
    }
 

	private void OnEnable()
    {
        playerRigidbody.isKinematic = false;
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
        // if (EventSystem.current.IsPointerOverGameObject()){
        //     Debug.Log("Pointer over UI Object");
        //     return;
        // }
			

        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            actions.GetUp();
        }

        // Movement Actions
        bool isMoving = IsMoving();
        bool isJumping = IsJumping();
        bool isCrouching = IsCrouching();
        bool isRunning = IsRunning();

        if (isCrouching && gameObject.tag.Equals("Katya"))
        {
            gameObject.tag = "KatyaStealthMode";
            Debug.Log(gameObject.tag);
        }
        else if (!isCrouching && !gameObject.tag.Equals("Katya"))
        {
            gameObject.tag = "Katya";
            Debug.Log(gameObject.tag);
        }

        if (isMoving && isJumping)
		{
            actions.Stay();
            actions.Jump();
            Jump();
        }
		else if (isJumping)
		{
            actions.Jump();
            Jump();
        }
        else if (isMoving && isRunning && isCrouching)
        {
            movementSpeedKey = "crouch-run";
            actions.Sitting();
            actions.Run();
        }
        else if (isMoving && isRunning)
        {
            movementSpeedKey = "run";
            actions.Run();
        }
        else if (isMoving && isCrouching)
        {
            movementSpeedKey = "crouch-walk";
            actions.Sitting();
            actions.Walk();
        }
        else if (isMoving)
		{
            movementSpeedKey = "walk";
            actions.Walk();
		}
        else if (isCrouching)
        {
            actions.Stay();
            actions.Sitting();
        }
		else
		{
			actions.Stay();
		}
        

		if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
		{
			actions.Attack();
			if (arsenalIndex != 0)
			{
                nextTimeToFire = Time.time + 1f/fireRate;
				Shoot();
			}
            
			
		} 
		if (Input.GetMouseButton(1))
		{
			actions.Aiming();
			
		}
        else if (Input.GetMouseButtonUp(1))
        {
            actions.Stay();
        }

        if (Input.GetKeyDown(KeyCode.F))
		{
			SwitchWeapon();
		} 
        

		

	}

    private void OnCollisionStay(Collision collision){
        if(collision.collider.tag == "Item" && Input.GetKeyDown(KeyCode.T)) {
            SetFocus(collision.collider.GetComponent<Interactable>());
        }
    }

    private bool IsMoving()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    }

    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private bool IsJumping()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool IsCrouching()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void LateUpdate() {
        if(enemies.GetComponentsInChildren<Target>().Length == 0) {
            level++;
            Debug.Log("This is the level:" + level);
             SceneManager.LoadScene(level);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        gameObject.transform.position += movement * speedsDict[movementSpeedKey] * Time.deltaTime;
    }

    private void Turn()
    {
		yaw += speedsDict["turn"] * Input.GetAxis("Mouse X");
		gameObject.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
       
    }

    private void Jump()
    {
        GetComponent<Rigidbody>().AddForce(jump * speedsDict["jump-force"], ForceMode.Impulse);
    }

	private void SwitchWeapon()
	{
		arsenalIndex++;
		if (arsenalIndex == arsenal.Length) {
            arsenalIndex = 0;
        }
		SetArsenal(arsenal[arsenalIndex].name);
		
	}

	void Shoot()
	{
        foreach (Arsenal hand in arsenal)
        {
            if (hand.name == arsenal[arsenalIndex].name)
            {
                if (hand.rightGun != null)
                {
                    GameObject rightGun = rightGunBone.GetChild(0).gameObject;
                    Shoot(rightGun, hand.damage);
                }
                if (hand.leftGun != null)
                {
                    GameObject leftGun = leftGunBone.GetChild(0).gameObject;
                    Shoot(leftGun, hand.damage);
                }
                return;
            }
        }		
	}

    private void Shoot(GameObject gun, float damage)
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gameObject.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.takeDamage(damage);
            }
            if(hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

        }
    }

    void SetFocus (Interactable newFocus)
	{
		if (onFocusChangedCallback != null)
			onFocusChangedCallback.Invoke(newFocus);

		// If our focus has changed
		if (focus != newFocus && focus != null)
		{
			// Let our previous focus know that it's no longer being focused
			focus.OnDefocused();
		}

		// Set our focus to what we hit
		// If it's not an interactable, simply set it to null
		focus = newFocus;

		if (focus != null)
		{
			// Let our focus know that it's being focused
			focus.OnFocused(transform);
		}

	}

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void takeDamage(float amount)
	{
		this.health -= amount;
        healthBar.UpdateBar(this.health, this.maxHealth);
        if (this.health <= 0)
		{
			actions.Death();
            Die();
		}
		else
		{
			actions.Damage();
		}

	}

    public void Heal(float healthGain) {
        this.health += healthGain;
        if (this.health >= this.maxHealth)
		{
            this.health = this.maxHealth;
		}
		healthBar.UpdateBar(this.health, this.maxHealth);
    }

    public void SavePlayer() {
        SaveSystem.SavePlayer(this,gameObject.transform.position);
    }
     public float Dist()
     {
         return distanceSquared; 
     }

	private void OnCollisionEnter(Collision collision)
    {
        // TODO: Add collision handling depending on tags of objects
        //actions.Damage();
    }

    [System.Serializable]
    public struct Arsenal
    {
        public string name;
        public GameObject rightGun;
        public GameObject leftGun;
        public RuntimeAnimatorController controller;
        public float damage;
    }

    [System.Serializable]
    public struct Speed
    {
        public string name;
        public float speed;
    }
}
