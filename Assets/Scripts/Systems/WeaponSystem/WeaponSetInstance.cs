using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetInstance
{
    public Action<EntityBase, EntityBase> OnHit;
    public WeaponSet Data;

    public List<GameObject> Weapons;

    public GameObject MainHand;
    public GameObject OffHand;

    public WeaponSetInstance(EntityBase entity, WeaponSet data, GameObject mainhand, GameObject offhand)
    {
        Data = data;
        Weapons = new List<GameObject> { mainhand };

        MainHand = mainhand;

        if (offhand != null)
        {
            Weapons.Add(offhand);

            OffHand = offhand;
        }
    }
}

