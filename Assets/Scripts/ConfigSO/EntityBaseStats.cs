using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/EntityStats", fileName = "EntityBaseStats")]
public class EntityBaseStats : ScriptableObject
{
    public float MaxHealth;
    public float MaxResource;
    public float Attack;
    public float MoveSpeed;
    public float CritChance;
    public float Defense;
    public ResourceType ResourceType;
    public WeaponSet Weapon;

    public float OathTwistWindow;
}

[Serializable]
public class OathModifier
{
    public OathType Oath;
    public float Value;
}