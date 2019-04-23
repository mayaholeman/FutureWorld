using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBot_WL1 : StandardBot { 

    private bool moving = false;
    private int moveCount = 0;
    private int rotateCount = 0;

    private int ROTATE_LIM = 45;
    private int MOVE_LIM = 50;

    // Update is called once per frame
    void Update() {

        if (moving) {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
            moveCount++;
            if (moveCount >= MOVE_LIM) {
                moving = false;
            }
        } else {
            if (rotateCount < ROTATE_LIM) {
                transform.Rotate(0, 90/ROTATE_LIM, 0);
                rotateCount++;
            } else {
                moveCount = 0;
                rotateCount = 0;
                moving = true;
            }
        }

        /**if (Input.GetKey(KeyCode.W))
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
        }*/
        
    }
}
