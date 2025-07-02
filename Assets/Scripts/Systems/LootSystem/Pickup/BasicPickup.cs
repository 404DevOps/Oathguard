using System.Collections.Generic;
using UnityEngine;

public class BasicPickup : PickupBase
{
    [SerializeReference]
    public List<AbilityEffectBase> Effects;

    public override void OnCollected(PlayerEntity collector)
    {
        foreach (var fx in Effects)
        {
            fx.Apply(collector, collector);
        }
        Destroy(gameObject);
    }
}

