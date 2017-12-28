using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	public void PlayAgain() {
		SceneManager.LoadScene (1);
	}

	public void TryAgain() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

    public void QuitGame()
    {
        Debug.Log("QUIT!"); //to check in unity
        Application.Quit();
    }
}
