using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/WeaponAbility", fileName = "NewWeaponAbility")]
public class WeaponAbility : AbilityBase
{
    public HitDetectionBase HitDetection;
    public LayerMask Layer;
    public float WeaponActivationDelay;
    public float WeaponActiveDuration;
    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        var weapon = origin.WeaponInstance;
        weapon.OnHit = null; //reset previous event listeners
       

        yield return WaitManager.Wait(WeaponActivationDelay);

        yield return HitDetection.Execute(origin, Layer, OnWeaponHit);

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    private void OnWeaponHit(EntityBase origin, EntityBase target)
    {
        ApplyEffects(origin, target);
        PlayOnHitEffect(origin, target);
    }

    private void PlayOnHitEffect(EntityBase origin, EntityBase target)
    {
        HitEffectManager.Instance.PlayHitVFX(origin, target);
    }

    public override IEnumerator WaitForAnimation(EntityBase origin)
    {
        var remainingTime =  AnimationData.AnimationDuration - (WeaponActivationDelay + HitDetection.Duration);
        if (remainingTime > 0)
            yield return WaitManager.Wait(remainingTime);

        InvokeAbilityFinished();
    }
}

