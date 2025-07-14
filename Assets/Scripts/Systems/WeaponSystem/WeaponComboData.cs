using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/ComboData")]
public class WeaponComboData : ScriptableObject
{
    public List<AbilityBase> PrimaryCombo;
    public List<AbilityBase> SecondaryCombo;
}

