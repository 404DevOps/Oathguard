using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/ImmunityAura", fileName = "NewImmunityAura")]
public class ImmunityAura : AuraBase
{
    public List<DamageType> ImmuneTypes;

    public override void OnApply(AuraInstance instance)
    {
        base.OnApply(instance); // If you still want modifiers, otherwise remove

        var immunityHandler = instance.Target.GetComponent<EntityImmunity>();
        if (immunityHandler != null)
        {
            foreach (var type in ImmuneTypes)
            {
                immunityHandler.AddImmunity(type, this.Id);
            }
        }
    }

    public override void OnExpire(AuraInstance instance)
    {
        base.OnExpire(instance);

        var immunityHandler = instance.Target.GetComponent<EntityImmunity>();
        if (immunityHandler != null)
        {
            foreach (var type in ImmuneTypes)
            {
                immunityHandler.RemoveImmunity(type, this.Id);
            }
        }
    }
}