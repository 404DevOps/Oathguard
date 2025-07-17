using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/WeaponAbility", fileName = "NewWeaponAbility")]
public class WeaponAbility : AbilityBase
{
    public HitDetectionBase HitDetection;
    public LayerMask Layer;


    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();


        var weapon = origin.WeaponInstance;
        weapon.OnHit = null; //reset previous event listeners
       

        yield return WaitManager.Wait(Timing.AnticipationDelay);
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target, Timing.VFXDuration);

        OnSwingStart?.Invoke();

        yield return HitDetection.Execute(origin, Layer, Timing.HitDuration, OnWeaponHit);

        OnSwingEnd?.Invoke();
        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    protected virtual void OnWeaponHit(EntityBase origin, EntityBase target)
    {
        ApplyEffects(origin, target);
        PlayOnHitEffect(origin, target);
    }

    protected virtual void PlayOnHitEffect(EntityBase origin, EntityBase target)
    {
        HitEffectManager.Instance.PlayHitVFX(origin, target);
        HitEffectManager.Instance.PlaySplashVFX(origin, target);
    }

    public override IEnumerator WaitForAnimation(EntityBase origin)
    {
        var remainingTime = Timing.RecoveryTime;
        yield return WaitManager.Wait(remainingTime);

        InvokeAbilityFinished();
    }
}


