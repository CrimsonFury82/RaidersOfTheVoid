using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_ : MonoBehaviour {

	public void LoadScene(string sceneName)
	{
        //Application.LoadLevel (level); 
        //The line above is obsolete and should not be used. I replaced with the relevant line below instead. - Aston
        SceneManager.LoadScene(sceneName);
    }

	public void QuitGame()
	{
		Application.Quit ();
        print("Quitting");
	}
}