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

        Debug.Log("Shoot");

		RaycastHit hit;
		if (Physics.Raycast(transform.position, Vector3.left, out hit, 100f))
		{
			Debug.Log(hit.transform.name);
			Target target = hit.transform.GetComponent<Target>();
			if (target != null)
			{
				target.takeDamage(10);
			}
		}
    }
}
