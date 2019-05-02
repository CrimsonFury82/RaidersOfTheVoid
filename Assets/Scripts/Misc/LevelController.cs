using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int levelCounter = 0;

    public GameController gameController;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameController = (GameController)FindObjectOfType(typeof(GameController)); //finds the gamecontroller
    }

    public void LoadNextLevel()
    {
        levelCounter++;
        print("Level: " + levelCounter);
        SceneManager.LoadScene("GameScene");
    }
}