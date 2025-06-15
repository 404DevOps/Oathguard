
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/OathAura", fileName = "NewOathAura")]
public class OathAura : AuraBase
{
    public OathType OathType;

    [Header("Player Particles")]
    public GameObject AuraParticles;
    public Vector3 ParticleOffset;

    [Header("Sword Particles")]
    public GameObject SwordAuraParticles;
    public Vector3 SwordParticleOffset;
    public Vector3 SwordParticleRotation;

    public List<OathUpgrade> OathUpgrades;

    public override void OnApply(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.AddListener(OnEntityDamaged);

        AddVFX(instance);

        //var glowController = instance.Target.Weapon.GetComponent<SwordGlowController>();
        //glowController.EnableGlow(SwordGlowColor);
    }

    public override void OnExpire(AuraInstance instance)
    {
        GameEvents.OnEntityDamaged.RemoveListener(OnEntityDamaged);

        ClearVFX(instance);
        
        base.OnExpire(instance);
    }

    private void OnEntityDamaged(DamageEventArgs args)
    {
        //todo filter dmg to only take basic & spin hits where target != player
        Debug.Log("EntityDamaged registered in OathAura.");

        foreach (var upgrade in OathUpgrades)
            upgrade.Apply(args.Origin, args.Target);
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

