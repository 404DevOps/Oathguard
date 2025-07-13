
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathAura", fileName = "NewOathAura")]
public class OathAura : AuraBase
{
    [Tooltip("React to all by Default, if thats unintended set None or specific Types instead.")]
    public List<DamageType> ReactToDamageTypes;
    public OathType OathType;
    public List<OathUpgrade> OathUpgrades;

    public string FlavorText;

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
        if (args.Target.IsDead) return;
        if (!args.SourceEffect.AllowTriggerReactiveEvents) return; //this will typically be on bleeds/poison etc
        if (instance.Target != args.Origin) return; //only trigger if holder was attacker
        if (instance.Target == args.Target) return; //only trigger on enemy struck

        
        if (ReactToDamageTypes != null) //react to all by default, when none cancel list, when some are set != none, apply
        {
            if (args.Type == DamageType.None) return;
            if (!ReactToDamageTypes.Contains(args.Type)) return;
        }
        
        

        Debug.Log("OnEntityDamaged fired in OathAura.");

        foreach (var upgrade in OathUpgrades)
        {
            if (args.SourceOathUpgrade == upgrade) return; //dont trigger self


            var levelAddition = 0;
            if (instance.Target is PlayerEntity)
            { 
                var player = (PlayerEntity)instance.Target;
                levelAddition = player.Experience.CurrentLevel;
            }

            if (UnityEngine.Random.value <= (upgrade.ProccChance + levelAddition) / 100f) //procc chance
                upgrade.Apply(args);
        }
    }

  
}

