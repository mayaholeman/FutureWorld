using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : MonoBehaviour { 

    private float movementSpeed = 25f;
    private int health = 15;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update() {
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rigidbody.position += Vector3.back * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rigidbody.position += Vector3.left * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rigidbody.position += Vector3.right * Time.deltaTime * movementSpeed;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, Time.deltaTime * 500.0f, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, Time.deltaTime * -500.0f, 0);
            }
        }
    }

    public Vector3 pos {
        get {
            return (this.transform.position);
        }
        set {
            this.transform.position = value;
        }
    }
}
