using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Script written by Aston Olsen

[System.Serializable]
[ExecuteInEditMode]

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck/New Deck")]

public class DeckData : ScriptableObject
{
    [SerializeField]
    public List<BaseData> deckList;
}