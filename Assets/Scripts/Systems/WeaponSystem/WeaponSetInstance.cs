using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class WeaponSetInstance
{
    public Action<EntityBase, EntityBase> OnHit;

    public List<WeaponHitbox> Weapons;   
    public WeaponSet Data;

    public WeaponSetInstance(EntityBase entity, WeaponSet data, List<WeaponHitbox> hitboxes) 
    {
        Data = data;
        Weapons = hitboxes;
        foreach (var hb in Weapons)
        {
            hb.Initialize(entity, data);
        }
    }

    public void EnableHitboxes()
    {
        foreach (var weaponHitbox in Weapons)
        {
            weaponHitbox.EnableHitbox();
            weaponHitbox.OnHit += OnHit;
        }
    }

    public void DisableHitboxes()
    {
        foreach (var weaponHitbox in Weapons)
        {
            weaponHitbox.OnHit -= OnHit;
            weaponHitbox.DisableHitbox();
        }
    }
}

