using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Relic", menuName = "Card/Relic")]
public class RelicCardData : BaseCardData
{
    public int attack, cooldown;
}