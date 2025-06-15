using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/VFX/SwordSpinVFX")]
internal class SpinAbilityVFX : AbilityVFXBase
{
    public GameObject SpinVFXPrefab;
    public TargetType TargetType;
    public float DelayBeforePlay;
    public float Duration;

    public override void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null)
    {
        CoroutineUtility.Instance.RunAbilityCoroutine(PlayVFXRoutine(origin, target), ability.Id);
    }

    private IEnumerator PlayVFXRoutine(EntityBase origin, EntityBase target)
    {
        if (DelayBeforePlay > 0)
            yield return WaitManager.Wait(DelayBeforePlay);

        var tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null)
            Debug.LogError("VFX Target null.");

        var instance = Instantiate(SpinVFXPrefab, tar.transform);

        yield return WaitManager.Wait(Duration);

        Destroy(instance);
    }
}

