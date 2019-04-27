using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public int levelCounter = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void levelIncrement()
    {
        levelCounter++;
        print("Level: " + levelCounter);
    }
}