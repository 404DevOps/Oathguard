using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class DamageCalculator : Singleton<DamageCalculator>
{
    private Dictionary<string, EntityStats> EnemyStats;
    private EntityStats PlayerStats;
    private SealAura ActiveSealAura;

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

        if (args.AuraId == ActiveSealAura.Id)
        {
            ActiveSealAura = null;
        }
    }

    private void OnAuraApplied(AuraAppliedEventArgs args)
    {
        if (args.EntityId != EntityManager.Instance.Player.Id) return;

        if (args.AuraInstance.Template is SealAura)
        {
            ActiveSealAura = args.AuraInstance.Template as SealAura;
        }
    }

    private void OnLoadoutChanged()
    {
        //reload setup to get new gems and stats
        OnEntityInitialized(EntityManager.Instance.Player);
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
    public DamageEventArgs GetCalculatedDamage(EntityBase source, EntityBase target, DamageEntityEffect effect)
    {
        if (source.GetType() == typeof(PlayerEntity))
        {
            return PlayerDamagesNPC((PlayerEntity)source, target, effect);
        }
        else
        {
            return NPCDamagesPlayer(source, target, effect);
        }
    }
    private DamageEventArgs NPCDamagesPlayer(EntityBase source, EntityBase target, DamageEntityEffect effect)
    {
        var damagePercentageRoll = Random.Range(effect.MinDamage, effect.MaxDamage);
        var baseDamage = EnemyStats[source.Id].Attack / 100 * damagePercentageRoll;

        float finalDmg = baseDamage;
        if (!effect.IsTrueDamage) //ignore mitigation if marked as true Damage
        {
            finalDmg = baseDamage - PlayerStats.Defense;
        }

        //reset to 0 so player cannot get healed when having way more defense than attackdmg was
        if (finalDmg < 0) finalDmg = 0;

        return new DamageEventArgs(target, target, finalDmg, AttackEffectivityType.Neutral, false);
    }
    private DamageEventArgs PlayerDamagesNPC(PlayerEntity origin, EntityBase target, DamageEntityEffect effect)
    {
        float damagePercentageRoll = Random.Range(effect.MinDamage, effect.MaxDamage);
        float totalRolledDamage = PlayerStats.Attack / 100f * damagePercentageRoll;

        bool isCrit = effect.CanCrit && RollCrit(PlayerStats.CritChance);
        if (isCrit)
            totalRolledDamage *= 2f;

        float baseDamage = totalRolledDamage;
        AttackEffectivityType effectivityType = AttackEffectivityType.Neutral;

        var sealType = SealType.None;
        if (ActiveSealAura != null)
        {
            sealType = ActiveSealAura.SealType;
            var sealModifier = EnemyStats[target.Id].SealModifier(ActiveSealAura.SealType);
            baseDamage *= sealModifier;
            effectivityType = GetDamageEffectivityType(sealModifier);
        }

        return new DamageEventArgs(
            origin,
            target,
            baseDamage,
            effectivityType,
            isCrit,
            false,
            sealType
        );
    }

    private static AttackEffectivityType GetDamageEffectivityType(float damageModifier)
    {
        return damageModifier > 1 ? AttackEffectivityType.Effective :
               damageModifier < 1 ? AttackEffectivityType.NotEffective :
                                    AttackEffectivityType.Neutral;
    }

    private static bool RollCrit(float critChance)
    {
        float clampedCritChance = Mathf.Clamp(critChance, 0, 100);
        float roll = Random.Range(0f, 100f);
        return roll < clampedCritChance;
    }
}