using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/SwordAuraData")]
public class SwordAuraData : ScriptableObject
{
    public GameObject AuraPrefab;

    [Header("1H Config")]
    [SerializeField] private Vector3 _1H_Offset = new Vector3(0,0.9f,0);
    [SerializeField] private Vector3 _1H_Scale = new Vector3(1, 1, 0.7f);
    [SerializeField] private Vector3 _1H_Rotation = new Vector3(90, 90, 0);


    [Header("2H Config")]
    [SerializeField] private Vector3 _2H_Offset = new Vector3(0, 1.2f, 0);
    [SerializeField] private Vector3 _2H_Scale = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 _2H_Rotation = new Vector3(90, 90, 0);


    public Vector3 GetScale(WeaponType type)
    {
        return type switch
        {
            WeaponType.DualSword or WeaponType.SNS => _1H_Scale,
            WeaponType.Claymore => _2H_Scale,
            _ => Vector3.one
        };
    }

    public Vector3 GetOffset(WeaponType type)
    {
        return type switch
        {
            WeaponType.DualSword or WeaponType.SNS => _1H_Offset,
            WeaponType.Claymore => _2H_Offset,
            _ => Vector3.one
        };
    }

    public Vector3 GetRotation(WeaponType type)
    {
        return type switch
        {
            WeaponType.DualSword or WeaponType.SNS => _1H_Rotation,
            WeaponType.Claymore => _2H_Rotation,
            _ => Vector3.one
        };
    }
}
