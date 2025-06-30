using System;
using UnityEngine;


public static class GameEvents
{
    //entities
    public static readonly Event<EntityBase> OnEntityInitialized = new();
    public static readonly Event<string> OnEntityDied = new();
    public static readonly Event<string> OnEntityDestroyed = new();

    //combat
    public static readonly Event<DamageContext> OnEntityDamageReceived = new();
    public static readonly Event<HealingContext> OnEntityHealed = new();
    public static readonly Event<ShieldAbsorbedEventArgs> OnEntityShieldAbsorbed = new();
    public static readonly Event<HealthChangedEventArgs> OnEntityHealthChanged = new();
    public static readonly Event<ResourceChangedEventArgs> OnEntityResourceChanged = new();
    public static readonly Event<XPChangedEventArgs> OnEntityXPChanged = new();
    public static readonly Event<ShieldChangedEventArgs> OnEntityShieldChanged = new();

    //abilities
    public static readonly Event<GCDStartedEventArgs> OnGCDStart = new();
    public static readonly Event<CooldownStartedEventArgs> OnCooldownStart = new();
    public static readonly Event<CooldownEndedEventArgs> OnCooldownEnded = new();

    public static readonly Event<EntityBase> OnAbilitiesChanged = new();

    //auras
    public static readonly Event<AuraExpiredEventArgs> OnAuraExpired = new();
    public static readonly Event<AuraAppliedEventArgs> OnAuraApplied = new();
    public static readonly Event<AuraRefreshedEventArgs> OnAuraRefreshed = new();

    //gamecycle
    public static readonly Event OnRoundStarted = new();
    public static readonly Event<PlayerEntity> OnEntityLeveledUp = new();
    public static readonly Event<WeaponSet> OnWeaponSelected = new();
    public static readonly Event<OathAura> OnOathSelected = new();
    public static readonly Event OnPerkSelected = new();
}
