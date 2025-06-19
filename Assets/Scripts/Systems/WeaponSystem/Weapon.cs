using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponSet", fileName = "NewWeaponSet")]
public class WeaponSet : ScriptableObject
{
    public Sprite Image;
    public string Name;
    [TextArea]
    public string Description;
    public WeaponType Type;
    public float BaseDamage;
    public float CritChance;
    public float Defense;

    public List<AbilityBase> WeaponAbilities;

    public GameObject MainHandPrefab;
    public GameObject OffhandPrefab;
}