using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponSet", fileName = "NewWeaponSet")]
public class WeaponSet : ScriptableObject
{
    public Sprite Image;
    public string Name;
    [TextArea]
    public string Description;
    public WeaponType Type;
    public float BaseDamage;
    public float CritChance;
    public float Defense;

    public List<AbilityBase> WeaponAbilities;

    public GameObject MainHandPrefab;
    public GameObject OffhandPrefab;
    public Vector3 OffHandOffset;

    public WeaponSetInstance CreateInstance(EntityBase entity, Transform handSlotL, Transform handSlotR)
    {
        List<WeaponHitbox> weapons = new List<WeaponHitbox>();

        var mainHand = Instantiate(this.MainHandPrefab, handSlotR);
        weapons.Add(mainHand.GetComponent<WeaponHitbox>());
        if (OffhandPrefab != null)
        {
            var offhand = Instantiate(OffhandPrefab, handSlotL);
            offhand.transform.localPosition += OffHandOffset;

            if (Type != WeaponType.SNS) //dont add shield
                weapons.Add(offhand.GetComponent<WeaponHitbox>());
        }

        var instance = new WeaponSetInstance(entity, this, weapons);
        return instance;
    }
}