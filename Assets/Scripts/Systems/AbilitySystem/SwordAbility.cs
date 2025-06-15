using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SwordAbility", fileName = "NewSwordAbility")]
public class SwordAbility : AbilityBase
{
    public float WeaponActivationDelay;
    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        yield return WaitManager.Wait(WeaponActivationDelay);
        //enable sword
        var weapon = origin.Weapon;
        weapon.EnableHitbox();
        weapon.OnHit += ApplyEffects;

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);

        //disable sword
        weapon.DisableHitbox();
        weapon.OnHit -= ApplyEffects;
    }
}

