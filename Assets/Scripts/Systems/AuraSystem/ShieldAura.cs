using UnityEngine;

[CreateAssetMenu(menuName = "Aura/ShieldAura", fileName = "ShieldAura")]
public class ShieldAura : AuraBase
{
    public float ShieldAmount; // Total shield health

    public override void OnApply(AuraInstance instance)
    {
        base.OnApply(instance);
        instance.Target.Shield.AddShield(ShieldAmount);
    }

    public override void OnExpire(AuraInstance instance)
    {
        base.OnExpire(instance);
        instance.Target.Shield.ReduceShield(ShieldAmount);
    }

    internal override void OnRefresh(AuraInstance instance)
    {
        base.OnRefresh(instance);

        instance.Target.Shield.ReduceShield(ShieldAmount);
        instance.Target.Shield.AddShield(ShieldAmount);
    }
}
