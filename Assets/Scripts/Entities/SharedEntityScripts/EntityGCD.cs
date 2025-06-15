using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityGCD : MonoBehaviour
{
    private CooldownData _cdData = new CooldownData(0, 0);
    private EntityBase _entity;

    private void OnEnable()
    {
        _entity = GetComponent<EntityBase>();
    }

    public void SetGCD(float fullDuration)
    {
        _cdData = new CooldownData(Time.time, fullDuration);
        GameEvents.OnGCDStart.Invoke(new GCDStartedEventArgs(_entity.Id, _cdData.StartTime, fullDuration));
    }

    public bool HasCooldown()
    {
        float cooldownEndTime = _cdData.StartTime + _cdData.Duration;
        return Time.time < cooldownEndTime;
    }
}