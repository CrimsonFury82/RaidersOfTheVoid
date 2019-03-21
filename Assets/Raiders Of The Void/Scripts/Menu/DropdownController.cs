using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownController : MonoBehaviour
{

    public GameObject menu;
    public bool isVisable;

    // Update is called once per frame
    public void Dropdown()
    {
        isVisable = !isVisable;
        menu.SetActive(isVisable);
    }
}
