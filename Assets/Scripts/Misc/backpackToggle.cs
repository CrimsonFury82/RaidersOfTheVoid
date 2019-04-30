using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backpackToggle : MonoBehaviour
{
    public bool backpackSavedToInventory;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        backpackSavedToInventory = false;
    }
}
