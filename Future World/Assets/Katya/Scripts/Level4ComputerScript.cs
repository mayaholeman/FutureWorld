using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4ComputerScript : Interactable
{
    public int computerHacked = 0;
    public override void Interact()
	{
        computerHacked++;
		Debug.Log("Hacking the computer");
        if(computerHacked == 2) {
            SceneManager.LoadScene(5);
        }
		
	}
}
