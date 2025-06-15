using System;

[Serializable]
public class CooldownData
{
    public CooldownData(float startTime, float duration)
    {
        StartTime = startTime;
        Duration = duration;
    }

    public float StartTime;
    public float Duration;
}