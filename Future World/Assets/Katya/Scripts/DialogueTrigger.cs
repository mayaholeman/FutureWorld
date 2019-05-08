using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

	public void TriggerDialogue ()
	{
		// FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        DialogueManager.instance.StartDialogue(dialogue);
	}

	public void Start(){
		this.TriggerDialogue();
	}

}
