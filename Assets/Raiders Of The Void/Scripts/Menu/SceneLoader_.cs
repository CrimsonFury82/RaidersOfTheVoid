using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_ : MonoBehaviour {

	public void LoadScene(int level)
	{
		Application.LoadLevel (level);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
