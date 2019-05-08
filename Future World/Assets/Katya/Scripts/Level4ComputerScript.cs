﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4ComputerScript : Interactable
{
    public Dialogue dialogue;
    public override void Interact()
	{
        PlayerController.instance.computersHacked++;
		Debug.Log("Hacking the computer");
        DialogueManager.instance.StartDialogue(dialogue);
        gameObject.tag = "Untagged";
        if(PlayerController.instance.computersHacked == 2) {
            SceneManager.LoadScene(5);
        }
        
		
	}
}