using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AuraInstance : IDisposable
{
    public AuraInstance(AuraBase template, EntityBase origin, EntityBase target)
    {
        Template = template;
        Origin = origin;
        Target = target;
        StartTime = Time.time;
        VisualInstances = new();
    }
    public Action<DamageContext> DamageListener;
    public bool IsExpired => Template.Duration > 0 && Time.time >= StartTime + Template.Duration;


    public EntityBase Origin;
    public EntityBase Target;
    public AuraBase Template;
    public float StartTime;
    public List<GameObject> VisualInstances;
    public float LastTick;

    public void Expire()
    {
        Template.OnExpire(this);
        GameEvents.OnAuraExpired.Invoke(new AuraExpiredEventArgs(Target.Id, this.Template.Id));
    }

    public void Refresh()
    {
        StartTime = Time.time;
        Template.OnRefresh(this);
        GameEvents.OnAuraRefreshed.Invoke(new AuraRefreshedEventArgs(Target.Id, this));
    }

    internal void Apply()
    {
        Template.OnApply(this);
    }

    internal void DoTick()
    {
        Template.OnTick(this);
    }

    public void Dispose()
    {
        DamageListener = null;
        foreach (var visual in VisualInstances)
            CoroutineUtility.Destroy(visual);

        Origin = null;
        Target = null;
        Template = null;
    }
}