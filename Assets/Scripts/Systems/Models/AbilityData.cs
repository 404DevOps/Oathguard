using System;
using UnityEngine;

[Serializable]
public class AbilityData
{
    public Sprite Icon;
    public string Name;
    [TextArea]
    public string Description;

    public float Cooldown;
    public bool UseGCD;
    public float GCDDuration;
}