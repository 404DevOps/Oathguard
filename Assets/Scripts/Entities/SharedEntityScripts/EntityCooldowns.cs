using System.Collections.Generic;
using UnityEngine;

public class EntityCooldowns : MonoBehaviour
{
    private Dictionary<string, CooldownData> _cooldowns = new Dictionary<string, CooldownData>();
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
            _cooldowns[abilityId].Duration = duration;
        }
        else
        {
            _cooldowns.Add(abilityId, new CooldownData(currentTime, duration));
        }
        GameEvents.OnCooldownStart.Invoke(new CooldownStartedEventArgs(_entity.Id, abilityId, currentTime, duration));
    }

    public bool IsOnCooldown(string abilityId)
    {
        return _cooldowns.ContainsKey(abilityId);
    }

    public bool GetCooldownData(string abilityId, out CooldownData cdData)
    {
        if (_cooldowns.TryGetValue(abilityId, out cdData))
        {
            return true;
        }
        return false;
    }

    public void Update()
    {
        UpdateCooldowns();
    }

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

        foreach (var abilityId in expiredCooldowns)
        {
            _cooldowns.Remove(abilityId);
            GameEvents.OnCooldownEnded.Invoke(new CooldownEndedEventArgs(_entity.Id, abilityId));
        }
    }
}

