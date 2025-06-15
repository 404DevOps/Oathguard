using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathUpgrade", fileName = "NewOathUpgrade")]
public class OathUpgrade : ScriptableObject
{
    public OathType Type;
    public string UpgradeName;
    public string Description;

    [SerializeReference]
    public List<AbilityEffectBase> Effects;

    public virtual void Apply(EntityBase origin, EntityBase target)
    {
        foreach (var effect in Effects)
        {
            effect.Apply(origin, target);
        }
    }

    public virtual void OnOathApplied(AuraInstance instance) { }
    public virtual void OnOathExpired(AuraInstance instance) { }
}