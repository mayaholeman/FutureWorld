using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComputerInteraction : Interactable
{

    public Dialogue dialogue = new Dialogue();

    public ComputerInteraction()
    {
        dialogue.name = "Percy";
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "Our original window of opportunity is still open but it’s closing quickly.You’re going to infiltrate the machine’s Central Hub.";
    }

    public override void Interact()
	{
        StartCoroutine(LoadLevel());
	}

    IEnumerator LoadLevel()
    {
        Debug.Log("Hacking the computer");
        DialogueManager.instance.StartDialogue(dialogue);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(4);
    }
}
