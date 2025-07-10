using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Ability/VFX/NewSlashVFX")]
internal class SlashVFX : AbilityVFXBase
{
    public GameObject SlashVFXPrefab;
    public float Duration;
    public bool LeftToRight;

    public Vector3 EntityOffset;
    [ColorUsage(true, true)]
    public Color ColorBorder;
    [ColorUsage(true, true)]
    public Color ColorMain;

    private VisualEffect _vfx;

    public override void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null, float duration = 0)
    {
        Duration = Duration >= duration ? Duration : duration;
        if (ability != null)
            CoroutineUtility.Instance.RunAbilityCoroutine(PlayVFXRoutine(origin, target), ability.Id);
        else
            CoroutineUtility.Instance.RunCoroutineTracked(PlayVFXRoutine(origin, target));
    }

    private IEnumerator PlayVFXRoutine(EntityBase origin, EntityBase target)
    {
        var euler = new Vector3(0, origin.Model.eulerAngles.y + 90, 0);
        if (LeftToRight)
            euler.x = 180;

        var instance = Instantiate(SlashVFXPrefab, origin.transform.position, Quaternion.Euler(euler));
        _vfx = instance.GetComponentInChildren<VisualEffect>();
        while (!_vfx.HasVector4("ColorMain"))
            yield return null;
        _vfx.SetVector4("ColorBorder", AppearanceConfig.Instance().GetOathColor(GameManager.Instance.SelectedOath.OathType));
        _vfx.SetVector4("ColorMain", ColorMain);

        yield return WaitManager.Wait(Duration);

        Destroy(instance);
    }
}

