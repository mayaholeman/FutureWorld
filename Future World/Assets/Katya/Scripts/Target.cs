using UnityEngine;

public interface Target
{
	float Health { get; set; }

	void takeDamage(float amount);

	void Die();
}
