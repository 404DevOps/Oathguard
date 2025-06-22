using UnityEngine;

[CreateAssetMenu(menuName = "Aura/ShieldAura", fileName = "ShieldAura")]
public class ShieldAura : AuraBase
{
    public float ShieldAmount;

    public override void OnApply(AuraInstance instance)
    {
        base.OnApply(instance);
        instance.Target.Shield.SetShield(Id, SourceType.Aura, ShieldAmount);
    }

    public override void OnExpire(AuraInstance instance)
    {
        base.OnExpire(instance);
        instance.Target.Shield.RemoveSource(Id);
    }

    internal override void OnRefresh(AuraInstance instance)
    {
        base.OnRefresh(instance);
        instance.Target.Shield.SetShield(Id, SourceType.Aura, ShieldAmount);
    }
}
