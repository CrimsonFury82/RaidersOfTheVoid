using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[CreateAssetMenu(fileName = "New Hero", menuName = "Card/Hero")]
public class HeroData : BaseData
{
    public int armour;
}