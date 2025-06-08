using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct GCDStartedEventArgs
{
    public GCDStartedEventArgs(string entityId, float cooldownStartTime, float duration)
    {
        EntityId = entityId;
        StartTime = cooldownStartTime;
        Duration = duration;
    }

    public string EntityId;
    public float StartTime;
    public float Duration;
}

