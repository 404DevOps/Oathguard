using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Ability/WeaponAbility", fileName = "NewWeaponAbility")]
public class WeaponAbility : AbilityBase
{
    public float WeaponActivationDelay;
    public float WeaponActiveDuration;
    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        yield return WaitManager.Wait(WeaponActivationDelay);
        //enable weapon
        var weapon = origin.WeaponInstance;
        weapon.EnableHitboxes();
        weapon.OnHit = null; //reset previous event listeners
        weapon.OnHit += ApplyEffects;
        weapon.OnHit += PlayOnHitEffect;

        yield return WaitManager.Wait(WeaponActiveDuration);


        //disable weapon
        weapon.OnHit -= ApplyEffects;
        weapon.OnHit -= PlayOnHitEffect;
        weapon.DisableHitboxes();

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }

    private void PlayOnHitEffect(EntityBase origin, EntityBase target)
    {
        HitEffectManager.Instance.PlayHitVFX(origin, target);
    }

    public override IEnumerator WaitForAnimation(EntityBase origin)
    {
        var remainingTime = AnimationData.AnimationDuration - (WeaponActivationDelay + WeaponActiveDuration);
        if (remainingTime > 0)
            yield return WaitManager.Wait(remainingTime);

        InvokeAbilityFinished();
    }
}

