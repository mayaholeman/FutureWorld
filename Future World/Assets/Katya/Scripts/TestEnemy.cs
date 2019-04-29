using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour, Target
{
	private float health = 50;

	private int level = 1;
	public float Health
	{
		get{ return this.health;}
		set{ this.health = value;}
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
			Die();
		}

	}

	public int Level
	{
		get { return this.level; }
		set { this.level = value; }
	}


}
