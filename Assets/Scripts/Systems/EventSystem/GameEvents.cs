using System;
using UnityEngine;


public static class GameEvents
{
    //entities
    public static readonly Event<EntityBase> OnEntityInitialized = new();
    public static readonly Event<string> OnEntityDied = new();
    public static readonly Event<string> OnEntityDestroyed = new();

    //combat

    public static readonly Event<DamageContext> OnPreDamageApplied = new();
    public static readonly Event<DamageContext> OnPostDamageApplied =new();
    public static readonly Event<DamageContext> OnEntityDamageReceived = new();

    public static readonly Event<HealingContext> OnEntityHealed = new();
    public static readonly Event<HealthChangedEventArgs> OnEntityHealthChanged = new();
    public static readonly Event<ResourceChangedEventArgs> OnEntityResourceChanged = new();

    //abilities
    public static readonly Event<GCDStartedEventArgs> OnGCDStart = new();
    public static readonly Event<CooldownStartedEventArgs> OnCooldownStart = new();
    public static readonly Event<CooldownEndedEventArgs> OnCooldownEnded = new();

    public static readonly Event<EntityBase> OnAbilitiesChanged = new();

    //auras
    public static readonly Event<AuraExpiredEventArgs> OnAuraExpired = new();
    public static readonly Event<AuraAppliedEventArgs> OnAuraApplied = new();
    public static readonly Event<AuraRefreshedEventArgs> OnAuraRefreshed = new();

    public static readonly Event<WeaponSet> OnWeaponSelected = new();
}
