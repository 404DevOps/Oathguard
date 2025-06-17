using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathUpgrade", fileName = "NewOathUpgrade")]
public class OathUpgrade : ScriptableObject
{
    public OathType Type;
    public string UpgradeName;
    public string Description;

    public float ProccChance;

    [SerializeReference]
    public List<AbilityEffectBase> Effects;

    public virtual void Apply(DamageContext ctx)
    {
        foreach (var effect in Effects)
        {
            effect.Apply(ctx.Origin, ctx.Target, this);
        }
    }

    public virtual void OnOathApplied(AuraInstance instance) { }
    public virtual void OnOathExpired(AuraInstance instance) { }
}