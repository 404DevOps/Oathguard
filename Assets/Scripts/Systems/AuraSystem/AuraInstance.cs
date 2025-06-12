using UnityEngine;

public class AuraInstance
{
    public AuraInstance(AuraBase template, EntityBase origin, EntityBase target)
    {
        Template = template;
        Origin = origin;
        Target = target;
        StartTime = Time.time;
    }

    public bool IsExpired => Time.time >= StartTime + Template.Duration;

    public EntityBase Origin;
    public EntityBase Target;
    public AuraBase Template;
    public float StartTime;
    public GameObject VisualInstance;

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
}