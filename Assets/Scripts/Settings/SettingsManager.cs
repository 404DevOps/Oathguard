using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SettingsManager : Singleton<SettingsManager>
{
    private string _settingsDirectory => Path.Combine(Application.persistentDataPath, "Settings");
    public List<SettingsTypeFileNameMapping> _settingFileNames;

    public AudioSettings Audio { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        LoadAllSettings();
    }

    private void LoadAllSettings()
    {
        Audio = LoadSettings<AudioSettings>() ?? new AudioSettings(1f, 0.5f, 0.5f);
    }

    public void SaveAllSettings()
    {
        SaveSettings(Audio);
    }

    public T LoadSettings<T>() where T : class
    {
        var fileName = _settingFileNames.First(p => p.Type == typeof(T)).FileName;
        string filePath = Path.Combine(_settingsDirectory, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        return null;
    }

    public void SaveSettings<T>(T settings)
    {
        if (!Directory.Exists(_settingsDirectory))
        {
            Directory.CreateDirectory(_settingsDirectory);
        }

        var fileName = _settingFileNames.First(p => p.Type == typeof(T)).FileName;

        string filePath = Path.Combine(_settingsDirectory, fileName);
        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(filePath, json);
    }

    internal string LoadRebindingSettings()
    {
        if (!Directory.Exists(_settingsDirectory))
        {
            Directory.CreateDirectory(_settingsDirectory);
            return "";
        }
        //load keybind settings
        var filePath = Path.Combine(Application.persistentDataPath, "Settings/rebinds.json");
        if (!File.Exists(filePath))
        {
            return "";
        }
        var rebinds = File.ReadAllText(filePath);
        return rebinds;
    }
}