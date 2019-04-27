using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[CreateAssetMenu(fileName = "New Ultimate", menuName = "Card/Ultimate")]
public class UltimateData : BaseData
{
    public int dmg, heal, cooldown, apBonus;
}