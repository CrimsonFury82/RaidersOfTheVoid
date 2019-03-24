using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script written by Aston Olsen

[CreateAssetMenu(fileName = "New Relic", menuName = "Card/Relic")]
public class RelicCardData : BaseCardData
{
    public int dmg, heal, cooldown;
}