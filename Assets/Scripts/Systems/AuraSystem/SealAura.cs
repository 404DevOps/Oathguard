
using UnityEngine;

public class SealAura : AuraBase
{
    public SealType SealType;
    public new bool Unique = true;
    public new AuraType Type = AuraType.Seal;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        GameEvents.OnEntityDamaged.AddListener(OnEntityDamaged);
        base.Apply(origin, target);
    }

    public override void Expire()
    {
        GameEvents.OnEntityDamaged.RemoveListener(OnEntityDamaged);
        base.Expire();
    }

    private void OnEntityDamaged(DamageEventArgs args)
    {
        Debug.Log("EntityDamaged registered in SealAura.");
    }
}

