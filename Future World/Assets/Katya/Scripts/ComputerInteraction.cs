using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerInteraction : Interactable
{
    public override void Interact()
	{
		SceneManager.LoadScene(4);
	}
}
