using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
	public GameObject finshedGame;
	public GameObject credits;

	void Start()
    {
		StartCoroutine(EndGameUI());
    }

	IEnumerator EndGameUI()
	{
		yield return new WaitForSeconds(3f);
		finshedGame.SetActive(false);
		credits.SetActive(true);
		yield return new WaitForSeconds(4f);
		SceneManager.LoadScene("Main Menu");
	}

}
