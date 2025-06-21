using System;
using System.Collections.Generic;

public class WeaponSetInstance
{
    public Action<EntityBase, EntityBase> OnHit;
    public WeaponSet Data;

    public List<WeaponHitbox> Weapons;

    public WeaponHitbox MainHand;
    public WeaponHitbox OffHand;

    public WeaponSetInstance(EntityBase entity, WeaponSet data, WeaponHitbox mainhand, WeaponHitbox offhand)
    {
        Data = data;
        Weapons = new List<WeaponHitbox> { mainhand };

        MainHand = mainhand;
        MainHand.Initialize(entity, data);

        if (offhand != null)
        {
            Weapons.Add(offhand);

            OffHand = offhand;
            OffHand.Initialize(entity, data);
        }
    }

    public void EnableHitboxes(bool mainHand = true, bool offHand = false)
    {
        if (mainHand)
        {
            MainHand.EnableHitbox();
            MainHand.OnHit += OnHit;
        }
        if (offHand && OffHand != null)
        {
            OffHand.EnableHitbox();
            OffHand.OnHit += OnHit;
        }
    }

    public void DisableHitboxes(bool mainHand = true, bool offHand = false)
    {
        if (mainHand)
        {
            MainHand.DisableHitbox();
            MainHand.OnHit -= OnHit;
        }
        if (offHand && OffHand != null)
        {
            OffHand.DisableHitbox();
            OffHand.OnHit -= OnHit;
        }
    }
}

