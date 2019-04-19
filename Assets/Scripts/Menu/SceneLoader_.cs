using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script Written by Kyle Runeckles

public class SceneLoader_ : MonoBehaviour {

	public void LoadScene(string sceneName)
	{
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        GameObject.FindGameObjectWithTag("MenuMusic").GetComponent<MenuMusicController>().StartMusic();
    }

    public void QuitGame()
	{
		Application.Quit ();
        print("Quitting");
	}
}