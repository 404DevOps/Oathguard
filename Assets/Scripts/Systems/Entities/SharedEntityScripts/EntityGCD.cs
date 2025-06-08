using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityGCD : MonoBehaviour
{
    private GlobalCooldownInfo _cooldownInfo = new GlobalCooldownInfo(0, 0);
    private EntityBase _entity;

    private void OnEnable()
    {
        _entity = GetComponent<EntityBase>();
    }

    public void SetGCD(float fullDuration)
    {
        _cooldownInfo = new GlobalCooldownInfo(Time.time, fullDuration);
        GameEvents.OnGCDStart.Invoke(new GCDStartedEventArgs(_entity.Id, _cooldownInfo.StartTime, fullDuration));
    }

    public bool HasCooldown()
    {
        float cooldownEndTime = _cooldownInfo.StartTime + _cooldownInfo.Duration;
        return Time.time < cooldownEndTime;
    }
}

[Serializable]
public class GlobalCooldownInfo
{
    public GlobalCooldownInfo(float startTime, float fullDuration)
    {
        Duration = fullDuration;
        StartTime = startTime;
    }

    public float Duration;
    public float StartTime;
}