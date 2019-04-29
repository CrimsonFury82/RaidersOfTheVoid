using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backpackToggle : MonoBehaviour
{
    public bool backpackLoaded;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleBackpack()
    {
        backpackLoaded = !backpackLoaded;
    }
}
