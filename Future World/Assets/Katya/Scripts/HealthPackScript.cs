using UnityEngine;

/* An Item that can be consumed. So far that just means regaining health */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Consumable")]
public class HealthPackScript : Item {

	public int healthGain;		// How much health?
    public PlayerController player;

	// This is called when pressed in the inventory
	public override void Use()
	{
		// Heal the player
        player.Heal(healthGain);

		Debug.Log(name + " consumed!");

		RemoveFromInventory();	// Remove the item after use
	}

}
