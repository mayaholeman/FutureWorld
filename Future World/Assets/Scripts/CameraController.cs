using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject katya;

    public float distance = 10.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;


    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - katya.transform.position;
    }

    private void LateUpdate()
    {
        float wantedRotationAngle = katya.transform.eulerAngles.y;
        float wantedHeight = katya.transform.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = katya.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        //Transform tempTransform = katya.transform;
        //katya.transform.y += 1;
        transform.LookAt(katya.transform);

        //transform.position = katya.transform.position + offset;
    }
}
