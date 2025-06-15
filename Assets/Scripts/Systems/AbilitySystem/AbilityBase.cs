using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class AbilityBase : UniqueScriptableObject
{
    public Action OnHitDetected;
    public virtual AbilityData AbilityInfo => _abilityInfo;
    public virtual AnimationData AnimationInfo => _animationInfo;
    public virtual SoundEffectInfo SoundEffectInfo => _soundEffectInfo;

    public virtual event Action OnAbilitiyFinished;// get; set; }

    public AbilityVFXBase VFX_Execute;
    public AbilityVFXBase VFX_Pre;

    [SerializeField] protected AbilityData _abilityInfo;
    [SerializeField] protected AnimationData _animationInfo;
    [SerializeField] protected SoundEffectInfo _soundEffectInfo;
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
            if (AbilityInfo.UseGCD)
            {
                StartGlobalCooldown(origin, _abilityInfo.GCDDuration);
            }

            StartCooldown(origin, _abilityInfo.Cooldown);
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
        if (AnimationInfo.AnimationDuration > 0)
            yield return WaitManager.Wait(AnimationInfo.AnimationDuration);

        InvokeAbilityFinished();
    }
    internal virtual void PlayAttackAnimation(EntityBase origin)
    {
        if (origin == null) //assume origin dead.
            return;

        origin.Animator.SetTrigger(AnimationInfo.AnimationTriggerName);
        origin.Animator.SetInteger("animationIndex", _animationIndex);

        //Debug.Log("AnimationTrigger: " + AnimationTriggerName + " AnimationIndex: " + _animationIndex);
        _animationIndex++;

        if (_animationIndex > AnimationInfo.AnimationVariationCount - 1)
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
        if (SoundEffectInfo.SFX_Execution != null)
            AudioManager.Instance.PlaySFX(SoundEffectInfo.SFX_Execution);
    }
    internal virtual void PlayMissSound()
    {
        //AudioManager.Instance.PlayMissSFX();   
        if (SoundEffectInfo.SFX_Execution != null)
            AudioManager.Instance.PlaySFX(SoundEffectInfo.SFX_Miss);
    }

    public virtual void AbortAbility()
    {
        CoroutineUtility.Instance.AbortAllAbilityCoroutines(Id);
        InvokeAbilityFinished();
    }
}


[Serializable]
public class SoundEffectInfo
{
    public bool PlaySoundOneShot;
    public bool PlaySoundOnMiss;
    public AudioClip SFX_Execution;
    public AudioClip SFX_Miss;
}