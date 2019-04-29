using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";	// Name of the item
	public Sprite icon = null;				// Item icon
	public bool showInInventory = true;

    public GameObject itemObject;

	// Called when the item is pressed in the inventory
	public virtual void Use ()
	{
		// Use the item
		// Something may happen
        Debug.Log("Using " + name);
	}

	// Call this method to remove the item from inventory
	public void RemoveFromInventory ()
	{
        // this.
		Inventory.instance.Remove(this);
	}

    public void setGameObject(GameObject itemGO){
        itemObject = itemGO;
    }

}
