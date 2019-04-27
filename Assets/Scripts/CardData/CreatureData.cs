using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[ExecuteInEditMode]
[CreateAssetMenu(fileName = "New Creature", menuName = "Card/Creature")]
public class CreatureData : BaseData
{
    public int dmg;
}