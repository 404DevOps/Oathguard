using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class AbilityBase : UniqueScriptableObject
{
    public Action OnHitDetected;
    public virtual AbilityData AbilityData => _abilityData;
    public virtual AnimationData AnimationData => _animationData;
    public virtual SFXData SFXData => _sfxData;

    public virtual event Action OnAbilitiyFinished;

    public AbilityVFXBase VFX_Execute;
    public AbilityVFXBase VFX_Pre;

    [SerializeField] protected AbilityData _abilityData;
    [SerializeField] protected AnimationData _animationData;
    [SerializeField] protected SFXData _sfxData;
    [SerializeReference] public List<AbilityEffectBase> Effects;

    internal bool _wasInterrupted = false;
    internal string _ownerId;

    #region Animation
    protected int _animationIndex = 0;
    #endregion

    public virtual void Initialize()
    {
        _animationIndex = 0;
        _wasInterrupted = false;
    }
    public virtual bool TryUseAbility(EntityBase origin, EntityBase target = null)
    {
        if (CanBeUsed(origin, true) && !HasAnyCooldown(origin, true))
        {
            _ownerId = origin.Id;
            if (AbilityData.UseGCD)
            {
                StartGlobalCooldown(origin, _abilityData.GCDDuration);
            }

            StartCooldown(origin, _abilityData.Cooldown);
            CoroutineUtility.Instance.RunAbilityCoroutine(Use(origin, target), this.Id);

            if (VFX_Pre != null)
                VFX_Pre.PlayVFX(origin, this, target);

            return true;
        }
        return false;
    }

    internal virtual void StartCooldown(EntityBase origin, float duration)
    {
        if (origin == null)
        {
            Debug.LogWarning("Origin is null, Entity maybe dead?");
            return;
        }
        origin.Cooldowns.Add(Id, duration);
    }
    internal virtual void StartGlobalCooldown(EntityBase origin, float duration)
    {
        if (origin == null)
        {
            Debug.LogWarning("Origin is null, Entity maybe dead?");
            return;
        }

        origin.GCD.SetGCD(duration);
    }
    
    internal abstract IEnumerator Use(EntityBase origin, EntityBase target = null);

    public IEnumerator WaitForAnimation(EntityBase origin)
    {
        if (AnimationData.AnimationDuration > 0)
            yield return WaitManager.Wait(AnimationData.AnimationDuration);

        InvokeAbilityFinished();
    }
    internal virtual void PlayAttackAnimation(EntityBase origin)
    {
        if (origin == null) //assume origin dead.
            return;

        origin.Animator.SetTrigger(AnimationData.AnimationTriggerName);
        origin.Animator.SetInteger("animationIndex", _animationIndex);

        //Debug.Log("AnimationTrigger: " + AnimationTriggerName + " AnimationIndex: " + _animationIndex);
        _animationIndex++;

        if (_animationIndex > AnimationData.AnimationVariationCount - 1)
        {
            _animationIndex = 0;
        }
    }

    internal void InvokeAbilityFinished()
    {
        OnAbilitiyFinished?.Invoke();
    }

    internal virtual void ApplyEffects(EntityBase origin, EntityBase target)
    {
        foreach (var effect in Effects)
        {
            effect.Apply(origin, target);
        }
    }
    internal virtual bool CanBeUsed(EntityBase origin, bool notifyPlayer, bool checkCasting = true)
    {
        return true;
    }
    internal virtual bool HasAnyCooldown(EntityBase origin, bool notifyPlayer)
    {
        if (HasCooldown(origin))
        {
            if (notifyPlayer) { Debug.Log("Can't be used: Ability is on Cooldown"); }
            return true;
        }
        if (HasGlobalCooldown(origin))
        {
            if (notifyPlayer) { Debug.Log("Can't be used: Global Cooldown"); }
            return true;
        }
        return false;
    }
    protected bool HasCooldown(EntityBase origin)
    {
        return origin.Cooldowns.IsOnCooldown(Id);
    }
    protected bool HasGlobalCooldown(EntityBase origin)
    {
        return origin.GCD.HasCooldown();
    }
    internal virtual void PlayAbilitySound()
    {
        if (SFXData.SFX_Execution != null)
            AudioManager.Instance.PlaySFX(SFXData.SFX_Execution);
    }
    internal virtual void PlayMissSound()
    {
        //AudioManager.Instance.PlayMissSFX();   
        if (SFXData.SFX_Execution != null)
            AudioManager.Instance.PlaySFX(SFXData.SFX_Miss);
    }

    public virtual void AbortAbility()
    {
        if(VFX_Execute != null)
            VFX_Execute.EndVFX();
        if (VFX_Pre != null)
            VFX_Pre.EndVFX();

        CoroutineUtility.Instance.AbortAllAbilityCoroutines(Id);
        InvokeAbilityFinished();
    }
}


[Serializable]
public class SFXData
{
    public bool PlaySoundOneShot;
    public bool PlaySoundOnMiss;
    public AudioClip SFX_Execution;
    public AudioClip SFX_Miss;
}