
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathAura", fileName = "NewOathAura")]
public class OathAura : AuraBase
{
    public List<DamageType> ReactToDamageTypes;
    public OathType OathType;
    public List<OathUpgrade> OathUpgrades;




    public override void OnApply(AuraInstance instance)
    {
        base.OnApply(instance);

        instance.DamageListener = null;
        instance.DamageListener += (args) => OnEntityDamaged(instance, args);
        GameEvents.OnEntityDamageReceived.AddListener(instance.DamageListener);
    }

    public override void OnExpire(AuraInstance instance)
    {
        if (instance.DamageListener != null)
        {
            GameEvents.OnEntityDamageReceived.RemoveListener(instance.DamageListener);
            instance.DamageListener = null;
        }
        base.OnExpire(instance);
    }

    private void OnEntityDamaged(AuraInstance instance, DamageContext args)
    {
        if (instance.Target != args.Origin) return; //only trigger if holder was attacker
        if (instance.Target == args.Target) return; //only trigger on enemy struck
        if (!ReactToDamageTypes.Contains(args.Type)) return;

        Debug.Log("OnEntityDamaged fired in OathAura.");

        foreach (var upgrade in OathUpgrades)
        {
            if (args.SourceOathUpgrade == upgrade) return; //dont trigger self
            
            if (UnityEngine.Random.value <= upgrade.ProccChance / 100f) //procc chance
                upgrade.Apply(args);
        }
    }

  
}

