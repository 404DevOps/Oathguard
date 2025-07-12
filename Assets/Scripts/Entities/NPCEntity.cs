using System;
using UnityEngine.AI;

public class NPCEntity : EntityBase
{
    public bool HasAlreadyDied;

    public EnemyAI AI;
    public EntityKnockback Knockback;
    public NPCAbilities AbilityController;
    public WeightedLootTable LootTable;

    public Action<NPCEntity> OnDespawned;

    public bool CanUseAbilities => !IsDead && AI.StateMachine.CurrentStateID != AIState.Knockback;

    protected override void Initialize()
    {
        base.Initialize();
        HasAlreadyDied = false;
        Model = transform;
        var weapon = EntityStatMapping.Instance().GetBaseStats(Type).Weapon;
        WeaponInstance = weapon.CreateInstance(this, HandSlotL, HandSlotR);

        Knockback = GetComponent<EntityKnockback>();
        Knockback.Initialize(this);

        AI = GetComponent<EnemyAI>();
        AI.Initialize(this);

        AbilityController = GetComponent<NPCAbilities>();
        AbilityController.Initialize(this, weapon.WeaponAbilities);

        StartCoroutine(NotifyNextFrame());
    }

    public override void OnEntityDied(EntityBase entity)
    {
        if (entity.Id != Id) return;
        if (HasAlreadyDied) return;

        base.OnEntityDied(entity);
        EntityManager.Instance.Player.Experience.AddXP(Stats.Experience);
        HasAlreadyDied = true;
    }

    internal void OnSpawned()
    {
        Initialize();
    }

  
    public void OnDespawn() 
    {
        OnDespawned?.Invoke(this);
    }
}

