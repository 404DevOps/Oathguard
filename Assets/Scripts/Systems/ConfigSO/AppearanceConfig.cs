using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "AppearanceConfig", fileName = "Config/AppearanceConfig")]
internal class AppearanceConfig : ScriptableObject
{
    [SerializeField] private List<ResourceInfo> _resourceData;

    private static AppearanceConfig _instance;

    public static AppearanceConfig Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(AppearanceConfig).Name) as AppearanceConfig;
        }
        return _instance;
    }

    public ResourceInfo GetResourceData(ResourceType type)
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
}