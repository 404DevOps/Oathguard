using UnityEngine;


public static class GameEvents
{
    //entities
    public static readonly Event<EntityBase> OnEntityInitialized = new();
    public static readonly Event<string> OnEntityDied = new();
    public static readonly Event<string> OnEntityDestroyed = new();

    //combat
    public static readonly Event<DamageEventArgs> OnEntityDamaged = new();
    public static readonly Event<HealthChangedEventArgs> OnEntityHealthChanged = new();
    public static readonly Event<string> OnEntityHurt = new();
    public static readonly Event<HealEventArgs> OnEntityHealed = new();

    public static readonly Event<EntityBase> OnHitMissed = new();

    //abilities
    public static readonly Event<GCDStartedEventArgs> OnGCDStart = new();
    public static readonly Event<CooldownStartedEventArgs> OnCooldownStart = new();
    public static readonly Event<CooldownEndedEventArgs> OnCooldownEnded = new();

    //auras
    public static readonly Event<AuraExpiredEventArgs> OnAuraExpired = new();
    public static readonly Event<AuraAppliedEventArgs> OnAuraApplied = new();
    public static readonly Event<AuraRefreshedEventArgs> OnAuraRefreshed = new();
}
