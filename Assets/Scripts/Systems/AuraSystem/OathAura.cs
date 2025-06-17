
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


    [Header("Visuals")]
    public GameObject AuraParticles;
    public Vector3 ParticleOffset;

    [Header("Sword Particles")]
    public GameObject SwordAuraParticles;
    public Vector3 SwordParticleOffset;
    public Vector3 SwordParticleRotation;

    public override void OnApply(AuraInstance instance)
    {
        base.OnApply(instance);

        instance.DamageListener = null;
        instance.DamageListener += (args) => OnEntityDamaged(instance, args);
        GameEvents.OnEntityDamageReceived.AddListener(instance.DamageListener);

        AddVFX(instance);
    }

    public override void OnExpire(AuraInstance instance)
    {
        if (instance.DamageListener != null)
        {
            GameEvents.OnEntityDamageReceived.RemoveListener(instance.DamageListener);
            instance.DamageListener = null;
        }

        ClearVFX(instance);
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

    #region VFX
    private void AddVFX(AuraInstance instance)
    {
        if (AuraParticles != null)
        {
            var holder = instance.Target.AuraVisualsContainer.transform;
            var particles = Instantiate(AuraParticles, holder.position + ParticleOffset, Quaternion.identity, holder);
            instance.VisualInstances.Add(particles);
        }
        if (SwordAuraParticles != null)
        {
            var swordHolder = instance.Target.Weapon.transform;
            var swordParticles = Instantiate(SwordAuraParticles, swordHolder);
            swordParticles.transform.localPosition = SwordParticleOffset;
            swordParticles.transform.localRotation = Quaternion.Euler(SwordParticleRotation); 
            instance.VisualInstances.Add(swordParticles);
        }
    }
    private void ClearVFX(AuraInstance instance)
    {
        if (instance.VisualInstances != null)
        {
            foreach (var visualGO in instance.VisualInstances)
                Destroy(visualGO);
        }
        instance.VisualInstances.Clear();
    }

    #endregion
}

