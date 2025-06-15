using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "DamageColorConfig", fileName = "DamageColorConfig")]
internal class DamageColorConfig : ScriptableObject
{
    [SerializeField] private List<DamageColorPair> _damageColorPairs;

    private static DamageColorConfig _instance;
    public static DamageColorConfig Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(DamageColorConfig).Name) as DamageColorConfig;
        }
        return _instance;
    }

    public Color GetColor(DamageColorType type)
    {
        try
        {
            return _damageColorPairs.FirstOrDefault(pair => pair.DamageType == type).Color;
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not retreive Color for DamageType {type}:" + e.Message);
            return Color.white;
        }
    }
}

[Serializable]
public class DamageColorPair
{
    public DamageColorType DamageType;
    public Color Color;
}