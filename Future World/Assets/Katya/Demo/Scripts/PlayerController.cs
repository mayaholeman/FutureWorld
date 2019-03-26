using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{

    public Transform rightGunBone;
    public Transform leftGunBone;
    public Arsenal[] arsenal;

    public float speed = 12f;
    public float turnSpeed = 180f;
    
    private float movementInputValue;
    private float turnInputValue;

    private Actions actions;
    private Animator animator;
    private Vector3 movement;
    private Rigidbody playerRigidbody;

    void Awake()
    {
        actions = GetComponent<Actions>();
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        //if (arsenal.Length > 0)
        //    SetArsenal(arsenal[0].name);
    }

    private void OnEnable()
    {
        playerRigidbody.isKinematic = false;

        movementInputValue = 0f;
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
                //animator.runtimeAnimatorController = hand.controller;
                return;
            }
        }
    }

    private void Update()
    {
        movementInputValue = Input.GetAxis("Vertical");
        turnInputValue = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            actions.Walk();
        }
        else
        {
            actions.Stay();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            actions.Sitting();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            actions.GetUp();
        }

    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;

        playerRigidbody.MovePosition(playerRigidbody.position + movement);
    }

    private void Turn()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        
        gameObject.transform.Rotate(0, turnInputValue * turnSpeed * Time.deltaTime, 0);
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
