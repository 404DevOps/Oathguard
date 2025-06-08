using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class AbilityBase : UniqueScriptableObject
{
    public Action OnHitDetected;
    public virtual AbilityInfo AbilityInfo => _abilityInfo;
    public virtual AnimationInfo AnimationInfo => _animationInfo;
    public virtual SoundEffectInfo SoundEffectInfo => _soundEffectInfo;

    public virtual event Action OnAbilitiyFinished;// get; set; }

    public HitDetectionInfo HitDetectionInfo;
    public AbilityVFXBase VFX_Execute;
    public AbilityVFXBase VFX_Pre;

    [SerializeField] protected AbilityInfo _abilityInfo;
    [SerializeField] protected AnimationInfo _animationInfo;
    [SerializeField] protected SoundEffectInfo _soundEffectInfo;
    [SerializeReference] public List<AbilityEffectBase> Effects;

    internal bool _wasInterrupted = false;
    internal string _ownerId;

    #region Animation
    protected int _animationIndex = 0;
    #endregion

    public virtual void Initialize()
    {
        _wasInterrupted = false;
    }
    public virtual bool TryUseAbility(EntityBase origin, EntityBase target = null)
    {
        if (CanBeUsed(origin, true) && !HasAnyCooldown(origin, true))
        {
            _ownerId = origin.Id;
            if (AbilityInfo.CausesGCD)
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
    internal virtual List<EntityBase> DetectHits(EntityBase origin, bool showMiss = true)
    {
        //wait for the delay async
        //await Task.Delay(TimeSpan.FromSeconds(HitDetectionInfo.HitDetectionDelay));

        var allHits = HitDetectionInfo.HitDetector.CheckHit(origin, HitDetectionInfo.OriginOffset, HitDetectionInfo.LayerMask);

        List<EntityBase> filteredHits = FilterHits(allHits, origin);

        if (filteredHits.Count > 0)
        {
            OnHitDetected?.Invoke();
            if (!SoundEffectInfo.PlaySoundOneShot)
                PlayAbilitySound();

            if (AnimationInfo.OnHitVFX != null)
            {
                foreach (var target in filteredHits)
                {
                    var hitPos = HitDetectionInfo.HitDetector.GetOriginPositionWithOffset(origin, HitDetectionInfo.OriginOffset);
                    AnimationInfo.OnHitVFX.PlayVFX(target);
                }
            }
        }
        else
        {
            GameEvents.OnHitMissed.Invoke(origin);
            if (SoundEffectInfo.PlaySoundOnMiss)
                PlayMissSound();
        }

        return filteredHits;
    }
    protected virtual List<EntityBase> FilterHits(List<EntityBase> allTargetsHit, EntityBase origin)
    {
        var filteredTargets = new List<EntityBase>();

        foreach (var target in allTargetsHit)
        {
            if (!HitDetectionInfo.CanHitSelf && target.Id == origin.Id)
                continue;

            filteredTargets.Add(target);
        }

        return filteredTargets;
    }
    internal virtual IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        if (HitDetectionInfo.NeedsHit)
        {
            yield return WaitManager.Wait(HitDetectionInfo.HitDetectionDelay);
            var hits = DetectHits(origin);
            foreach (var hit in hits)
            {
                ApplyEffects(origin, hit);
            }
        }
        else
        {
            PlayAbilitySound();
            ApplyEffects(origin, null);
        }

        CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    public IEnumerator WaitForAnimation(EntityBase origin, bool fireFinishEvent = true)
    {

        if (AnimationInfo.AnimationDuration > 0)
            yield return WaitManager.Wait(AnimationInfo.AnimationDuration);

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(PlayRecoveryAnimationAnEndAttack(origin, fireFinishEvent), this.Id);
    }
    internal virtual void PlayAttackAnimation(EntityBase origin)
    {
        if (origin == null) //assume origin dead.
            return;

        origin.Animator.SetTrigger(AnimationInfo.AnimationTriggerName);
        origin.Animator.SetInteger("animationIndex", _animationIndex);

        //Debug.Log("AnimationTrigger: " + AnimationTriggerName + " AnimationIndex: " + _animationIndex);
        _animationIndex++;

        if (_animationIndex > AnimationInfo.AnimationVariations - 1)
        {
            _animationIndex = 0;
        }
    }

    internal void PlayWindUpAnimation(EntityBase origin)
    {
        if (!string.IsNullOrEmpty(AnimationInfo.AnimationWindUpTriggerName))
            origin.Animator.SetTrigger(AnimationInfo.AnimationWindUpTriggerName);
    }
    internal virtual IEnumerator PlayRecoveryAnimationAnEndAttack(EntityBase origin, bool fireFinishEvent = true)
    {
        if (!string.IsNullOrEmpty(AnimationInfo.AnimationRecoveryTriggerName))
            origin.Animator.SetTrigger(AnimationInfo.AnimationRecoveryTriggerName);

        yield return WaitManager.Wait(AnimationInfo.RecevoryAnimationDuration);

        if (fireFinishEvent)
            InvokeAbilityFinished();
    }

    internal void InvokeAbilityFinished()
    {
        OnAbilitiyFinished?.Invoke();
    }


    internal virtual void ApplyEffects(EntityBase origin, EntityBase target)
    {
        if (HitDetectionInfo.NeedsHit && target == null)
        {
            Debug.LogError("No Target but needs one, check HitDetectionInfo");
        }
        else
        {
            foreach (var effect in Effects)
            {
                effect.Apply(origin, target);
            }
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
    internal virtual void PlayWindupSound()
    {
        if (SoundEffectInfo.SFX_Windup != null)
            AudioManager.Instance.PlaySFX(SoundEffectInfo.SFX_Windup);
    }
    internal virtual void PlayAbilitySound()
    {
        if (SoundEffectInfo.SFX_Execution != null)
            AudioManager.Instance.PlaySFX(SoundEffectInfo.SFX_Execution, SoundEffectInfo.SFX_Execution_Duration);
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
public class AbilityInfo
{
    public bool LockInPlace;
    public string Name;
    public float Cooldown;
    public string Description;
    public int ResourceCost;
    public bool CausesGCD;
    public float GCDDuration;
    public float CastTime;
    public float AttackRange;

    public Sprite Icon;
}

[Serializable]
public class AnimationInfo
{
    public float AnimationDuration;
    public float RecevoryAnimationDuration;
    public string AnimationTriggerName;
    public string AnimationWindUpTriggerName;
    public string AnimationRecoveryTriggerName;
    public int AnimationVariations;
    public float ScreenShakeIntensity;
    public OnHitVFX OnHitVFX;
    public bool ShowDangerIcon;
}

[Serializable]
public class SoundEffectInfo
{
    public bool PlaySoundOneShot;
    public bool PlaySoundOnMiss;
    public bool LoopExecution;
    public AudioClip SFX_Windup;
    public AudioClip SFX_Execution;
    public AudioClip SFX_Miss;
    public float SFX_Execution_Duration;
}

[Serializable]
public class HitDetectionInfo
{
    public float HitDetectionDelay;
    public Vector3 OriginOffset;
    public int TargetCount;
    public bool NeedsHit;
    public bool CanHitSelf;
    public LayerMask LayerMask;
    [SerializeReference]
    public HitDetectionBase HitDetector;
}