using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityCooldowns : MonoBehaviour
{
    // Use a dictionary for faster lookup
    private Dictionary<string, CooldownInfo> _cooldowns = new Dictionary<string, CooldownInfo>();
    private EntityBase _entity;

    void OnEnable()
    {
        _entity = GetComponent<EntityBase>();
    }

    public void Add(string abilityId, float duration)
    {
        var currentTime = Time.time;
        if (_cooldowns.ContainsKey(abilityId))
        {
            _cooldowns[abilityId].StartTime = currentTime;
            _cooldowns[abilityId].Duration = duration; // Reset the cooldown if the ability is already in the dictionary
        }
        else
        {
            _cooldowns.Add(abilityId, new CooldownInfo(currentTime, duration));
        }
        GameEvents.OnCooldownStart.Invoke(new CooldownStartedEventArgs(_entity.Id, abilityId, currentTime, duration));
    }

    // Optimized cooldown check
    public bool IsOnCooldown(string abilityId)
    {
        return _cooldowns.ContainsKey(abilityId);
    }
    public bool GetCooldownInfo(string abilityId, out CooldownInfo cdInfo)
    {
        if (_cooldowns.TryGetValue(abilityId, out cdInfo))
        {
            return true;
        }
        return false;
    }

    public void Update()
    {
        UpdateCooldowns();
    }

    // Update cooldowns only when needed (not every frame)
    public void UpdateCooldowns()
    {
        var expiredCooldowns = new List<string>();

        foreach (var cd in _cooldowns)
        {
            var currentTime = Time.time;

            if (currentTime > cd.Value.StartTime + cd.Value.Duration)
            {
                expiredCooldowns.Add(cd.Key);
            }
        }

        // Remove expired cooldowns in bulk to avoid modifying the dictionary during iteration
        foreach (var abilityId in expiredCooldowns)
        {
            _cooldowns.Remove(abilityId);
            GameEvents.OnCooldownEnded.Invoke(new CooldownEndedEventArgs(_entity.Id, abilityId));
        }
    }
}

[Serializable]
public class CooldownInfo
{
    public CooldownInfo(float startTime, float duration)
    {
        StartTime = startTime;
        Duration = duration;
    }

    public float StartTime;
    public float Duration;
}