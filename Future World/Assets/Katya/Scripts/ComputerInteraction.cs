using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerInteraction : Interactable
{
    public override void Interact()
	{
		Debug.Log("Hacking the computer");
		SceneManager.LoadScene(4);
	}
}
