using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot : MonoBehaviour { 

    private float movementSpeed = 25f;
    private float rotationSpeed = 500f;
    private int health = 15;

    // Start is called before the first frame update
    void Start() {

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
                transform.position += -transform.forward * Time.deltaTime * movementSpeed;
            }

            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, Time.deltaTime * -rotationSpeed, 0);
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
