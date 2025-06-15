using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatSystem : Singleton<CombatSystem>
{
    private Dictionary<string, EntityStats> EnemyStats;
    private EntityStats PlayerStats;
    private OathAura ActiveOathAura;

    private void OnEnable()
    {
        EnemyStats = new Dictionary<string, EntityStats>();
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnAuraApplied.AddListener(OnAuraApplied);
        GameEvents.OnAuraExpired.AddListener(OnAuraExpire);
    }

    private void OnAuraExpire(AuraExpiredEventArgs args)
    {
        if (args.EntityId != EntityManager.Instance.Player.Id) return;

        if (args.AuraId == ActiveOathAura.Id)
        {
            ActiveOathAura = null;
        }
    }

    private void OnAuraApplied(AuraAppliedEventArgs args)
    {
        if (args.EntityId != EntityManager.Instance.Player.Id) return;

        if (args.AuraInstance.Template is OathAura)
        {
            ActiveOathAura = args.AuraInstance.Template as OathAura;
        }
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity is PlayerEntity)
        {
            CachePlayer(entity as PlayerEntity);
        }
        else
        {
            EnemyStats.TryAdd(entity.Id, entity.Stats);
        }
    }

    private void CachePlayer(PlayerEntity entity)
    {
        PlayerStats = entity.Stats;
    }

    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
    }
    public DamageContext CalculateDamage(EntityBase origin, EntityBase target, DamageEffect effect)
    {
        var context = new DamageContext
        {
            Origin = origin,
            Target = target,
            Type = effect.Type,
            IsTrueDamage = effect.IsTrueDamage,
            SourceEffect = effect
        };

        context.BaseDamage = GetBaseDamage(origin, effect.MinDamage, effect.MaxDamage);
        context.FinalDamage = context.BaseDamage;

        // Phase 1: Check Immunity
        if (HasImmunity(target, effect.Type))
        {
            context.IsImmune = true;
            context.FinalDamage = 0;
            return context;
        }

        // Phase 2: Apply Mitigation (if not true damage)
        if (!context.IsTrueDamage)
            ApplyMitigation(context);

        // Phase 3: Crits
        if (effect.CanCrit && GetCriticalRoll(origin.Stats.CritChance))
        {
            context.IsCritical = true;
            context.FinalDamage *= 2f;
        }

        // Phase 4: Apply Auras, Elemental, On-hit Modifiers
        ApplyOathModifiers(context);
        // Add future hooks here

        return context;
    }
    public HealingContext CalculateHealing(EntityBase origin, EntityBase target, HealEffect effect)
    {
        var context = new HealingContext
        {
            Origin = origin,
            Target = target,
            IsTrueHealing = effect.IsTrueHealing,
            SourceEffect = effect
        };

        context.FinalAmount = GetBaseDamage(origin, effect.MinAmount, effect.MaxAmount);

        if (!context.IsTrueHealing)
        {
            var healReduction = GetHealingReduction(context.Target);
            context.FinalAmount *= (1f - healReduction);
        }

        if (context.IsCritical)
        {
            context.FinalAmount *= 1.5f; // Or whatever crit heal multiplier
        }

        context.FinalAmount = Mathf.Clamp(context.FinalAmount, 0, float.MaxValue);

        return context;
    }

    private float GetHealingReduction(EntityBase target)
    {
        return 0;
    }

    private float GetBaseDamage(EntityBase source, float min, float max)
    {
        float roll = GetDamageRoll(min, max);
        return source.Stats.Attack / 100f * roll;
    }

    private void ApplyMitigation(DamageContext context)
    {
        var defense = context.Target.Stats.Defense;
        context.FinalDamage -= defense;
        if (context.FinalDamage < 0)
            context.FinalDamage = 0;
    }

    private bool HasImmunity(EntityBase target, DamageType type)
    {
        var auras = AuraManager.Instance.GetEntityAuras(target.Id);
        return auras.Any(a => a.Template is ImmunityAura);
    }

    private void ApplyOathModifiers(DamageContext context)
    {
        if (ActiveOathAura == null) return;

        var oathMod = context.Target.Stats.OathModifier(ActiveOathAura.OathType);
        context.FinalDamage *= oathMod;
    }

    public static float GetDamageRoll(float min, float max)
    {
        return Mathf.RoundToInt(Random.Range(min, max));
    }

    private static bool GetCriticalRoll(float critChance)
    {
        float clampedCritChance = Mathf.Clamp(critChance, 0, 100);
        float roll = Random.Range(0f, 100f);
        return roll < clampedCritChance;
    }
}