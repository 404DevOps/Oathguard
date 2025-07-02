using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Config/DamageColorConfig", fileName = "DamageColorConfig")]
internal class TextColorConfig : ScriptableObject
{
    [SerializeField] private List<DamageColorPair> _damageColorPairs;

    private static TextColorConfig _instance;
    public static TextColorConfig Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(TextColorConfig).Name) as TextColorConfig;
        }
        return _instance;
    }

    public Color GetColor(TextColorType type)
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
    public TextColorType DamageType;
    public Color Color;
}