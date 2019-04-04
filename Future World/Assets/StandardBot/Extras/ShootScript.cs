using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject shootie;

    private int shootctr = 50;
    private int speed = 10;

    private void Update()
    {
        if (shootctr <= 0)
        {
            shoot();
        } else
        {
            shootctr--;
        }
    }

    void shoot()
    {
        GameObject go = Instantiate<GameObject>(shootie);
        go.transform.position = transform.position;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = Vector3.left * speed;

        shootctr = 50;
    }
}
