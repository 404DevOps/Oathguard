using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Element Color Mapping", fileName = "ElementColorMapping")]
internal class DamageColorMapping : ScriptableObject
{
    [SerializeField] private List<DamageColorPair> _damageColorPairs;

    private static DamageColorMapping _instance;
    public static DamageColorMapping Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(DamageColorMapping).Name) as DamageColorMapping;
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