using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SealAbility", fileName = "NewSealAbility")]
public class SealAbility : AbilityBase
{
    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        ApplyEffects(origin, null);

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }
}

