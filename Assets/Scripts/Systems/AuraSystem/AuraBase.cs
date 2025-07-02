using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/BaseAura", fileName = "BaseAura")]
public class AuraBase : UniqueScriptableObject
{
    [Header("Data")]
    public string Name;
    public string Description;
    public Sprite Icon;

    public TargetType TargetType;
    public AuraType Type;

    public float TickRate;
    public float Duration;
    public bool ShowAuraDisplay;

    public List<StatModifier> Modifiers;
    [SerializeReference]
    public List<AbilityEffectBase> TickEffects;

    [Header("Visuals")]
    public GameObject AuraParticles;
    public Vector3 ParticleOffset;

    [Header("Sword Particles")]
    public SwordAuraData SwordAura;

    public virtual void OnApply(AuraInstance instance)
    {
        foreach (var modifier in Modifiers)
        {
            modifier.SourceId = Id;
            instance.Target.Stats.StatMediator.AddModifier(modifier);
        }
        AddVFX(instance);
    }
    public virtual void OnExpire(AuraInstance instance)
    {
        instance.Target.Stats.StatMediator.RemoveModifiersBySourceId(Id);
        ClearVFX(instance);
    }
 

    public virtual void OnTick(AuraInstance instance)
    {
        instance.LastTick = Time.time;

        foreach (var effec in TickEffects)
        {
            effec.Apply(instance.Origin, instance.Target);
        }
    }


    internal virtual void OnRefresh(AuraInstance auraInstance)
    {
        
    }

    #region VFX
    private void AddVFX(AuraInstance instance)
    {
        if (AuraParticles != null)
        {
            var holder = instance.Target.AuraVisualsContainer.transform;
            var particles = Instantiate(AuraParticles, holder.position + ParticleOffset, AuraParticles.transform.rotation, holder);
            instance.VisualInstances.Add(particles);
        }
        if (SwordAura != null)
        {
            var weaponInstance = instance.Target.WeaponInstance;
            foreach (var holder in weaponInstance.Weapons)
            {
                var swordParticles = Instantiate(SwordAura.AuraPrefab, holder.transform);
                swordParticles.transform.localPosition = SwordAura.GetOffset(weaponInstance.Data.Type);
                swordParticles.transform.localRotation = Quaternion.Euler(SwordAura.GetRotation(weaponInstance.Data.Type));
                swordParticles.transform.localScale = SwordAura.GetScale(weaponInstance.Data.Type);
                instance.VisualInstances.Add(swordParticles);
            }
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