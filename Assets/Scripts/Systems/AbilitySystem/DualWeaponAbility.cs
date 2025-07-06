using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Ability/DualWeaponAbility", fileName = "NewDualWeaponAbility")]
public class DualWeaponAbility : AbilityBase
{
    public bool SimultaneousStrike;

    public float MainHandActivationDelay;
    public float MainHandActiveDuration;

    public float OffHandActivationDelay;
    public float OffHandActiveDuration;

    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        VFX_Execute?.PlayVFX(origin, this, target);

        var weapon = origin.WeaponInstance;

        // Reset hit events
        weapon.OnHit = null;
        weapon.OnHit += ApplyEffects;
        weapon.OnHit += PlayOnHitEffect;

        if (SimultaneousStrike)
        {
            // Simultaneous: wait for mainhand delay, then enable both hitboxes
            yield return WaitManager.Wait(MainHandActivationDelay);

            weapon.EnableHitboxes(true, true);

            // Wait for the longer active duration of both hands
            float activeTime = Mathf.Max(MainHandActiveDuration, OffHandActiveDuration);
            yield return WaitManager.Wait(activeTime);

            weapon.DisableHitboxes(true, true);
        }
        else
        {
            // Sequential: mainhand first
            yield return WaitManager.Wait(MainHandActivationDelay);

            weapon.EnableHitboxes(true, false);
            yield return WaitManager.Wait(MainHandActiveDuration);
            weapon.DisableHitboxes(true, false);

            // Then offhand
            yield return WaitManager.Wait(OffHandActivationDelay);

            weapon.EnableHitboxes(false, true);
            yield return WaitManager.Wait(OffHandActiveDuration);
            weapon.DisableHitboxes(false, true);
        }

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    private void PlayOnHitEffect(EntityBase origin, EntityBase target)
    {
        HitEffectManager.Instance.PlayHitVFX(origin, target);
    }

    public override IEnumerator WaitForAnimation(EntityBase origin)
    {

        var remainingTime = AnimationData.AnimationDuration - (MainHandActivationDelay + MainHandActiveDuration);
        if (!SimultaneousStrike)
            remainingTime = remainingTime - OffHandActivationDelay - OffHandActiveDuration;

        if (remainingTime > 0)
            yield return WaitManager.Wait(remainingTime);

        InvokeAbilityFinished();
    }
}

