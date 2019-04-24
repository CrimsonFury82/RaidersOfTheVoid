using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Card/Weapon")]
public class WeaponCardData : BaseCardData
{
    public int dmg, ap, range;
}