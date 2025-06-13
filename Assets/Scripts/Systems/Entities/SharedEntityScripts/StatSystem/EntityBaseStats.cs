using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/EntityStats", fileName = "EntityBaseStats")]
public class EntityBaseStats : ScriptableObject
{
    public float MaxHealth;
    public float Attack;
    public float MoveSpeed;
    public float CritChance;
    public float Defense;
    public List<SealModifier> SealModifier;
}

[Serializable]
public class SealModifier
{
    public OathType Seal;
    public float Value;
}