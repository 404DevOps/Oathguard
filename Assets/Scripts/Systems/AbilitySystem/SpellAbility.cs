using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/SpellAbility", fileName = "NewSpellAbility")]
public class SpellAbility : AbilityBase
{
    internal override IEnumerator Use(EntityBase origin, EntityBase target = null)
    {
        PlayAttackAnimation(origin);
        PlayAbilitySound();
        if (VFX_Execute != null)
            VFX_Execute.PlayVFX(origin, this, target);

        ApplyEffects(origin, target);

        yield return CoroutineUtility.Instance.RunAbilityCoroutine(WaitForAnimation(origin), this.Id);
    }
}

