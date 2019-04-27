using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script Written by Kyle Runeckles

public class SceneLoader : MonoBehaviour {

	public void LoadScene(string sceneName)
	{
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("MenuMusic");
        if (controller != null)
        {
            controller.GetComponent<MenuMusicController>().StartMusic();
        }
    }

    public void QuitGame()
	{
		Application.Quit ();
        print("Quitting");
	}
}