using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Ability/DualWeaponAbility", fileName = "NewDualWeaponAbility")]
public class DualWeaponAbility : WeaponAbility
{
    public HitDetectionBase HitDetectionOffhand;
    public AbilityTimingData TimingOffhand;
    public AbilityVFXBase OffhandVFX;

    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();

        // assume mainhand attack first
        yield return WaitManager.Wait(Timing.AnticipationDelay);

        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target, Timing.HitDuration);

        OnSwingStart?.Invoke();
        yield return HitDetection.Execute(origin, Layer, Timing.HitDuration, OnWeaponHit);

        //offhand second
        yield return WaitManager.Wait(TimingOffhand.AnticipationDelay);

        if (OffhandVFX != null)
            OffhandVFX.PlayVFX(origin, this, target, TimingOffhand.HitDuration);

        yield return HitDetectionOffhand.Execute(origin, Layer, TimingOffhand.HitDuration, OnWeaponHit);

        OnSwingEnd?.Invoke();

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    public override IEnumerator WaitForAnimation(EntityBase origin)
    {
        var remainingTime = TimingOffhand.RecoveryTime;
        if (remainingTime > 0)
            yield return WaitManager.Wait(remainingTime);

        InvokeAbilityFinished();
    }
}

