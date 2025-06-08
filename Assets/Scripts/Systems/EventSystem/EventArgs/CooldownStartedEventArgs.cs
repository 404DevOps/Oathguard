using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct CooldownStartedEventArgs
{
    public CooldownStartedEventArgs(string entityId, string abilityId, float cooldownStartTime, float duration)
    {
        EntityId = entityId;
        AbilityId = abilityId;
        StartTime = cooldownStartTime;
        Duration = duration;
    }

    public string EntityId;
    public string AbilityId;
    public float StartTime;
    public float Duration;
}

