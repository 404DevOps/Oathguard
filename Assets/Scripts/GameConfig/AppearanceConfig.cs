using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Config/AppearanceConfig", fileName = "AppearanceConfig")]
internal class AppearanceConfig : ScriptableObject
{
    [SerializeField] private List<ResourceData> _resourceData;
    [SerializeField] private List<OathColorData> _oathColorData;

    private static AppearanceConfig _instance;

    public static AppearanceConfig Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(AppearanceConfig).Name) as AppearanceConfig;
        }
        return _instance;
    }

    public ResourceData GetResourceData(ResourceType type)
    {
        try
        {
            return _resourceData.FirstOrDefault(res => res.ResourceType == type);
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not retreive ResourceInfo for {type}:" + e.Message);
            return null;
        }
    }
    public Color GetOathColor(OathType type)
    {
        try
        {
            return _oathColorData.FirstOrDefault(oath => oath.Type == type).PrimaryColor;
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not retreive OathColor for {type}:" + e.Message);
            return Color.white;
        }
    }
}

[Serializable]
public class OathColorData
{
    public OathType Type;
    [ColorUsage(true, true)]
    public Color PrimaryColor;
}