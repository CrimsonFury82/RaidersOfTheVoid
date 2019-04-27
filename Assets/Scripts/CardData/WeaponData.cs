using UnityEngine;

//Script written by Aston Olsen

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Card/Weapon")]
public class WeaponData : BaseData
{
    [SerializeField]
    public int dmg, ap, range;
}