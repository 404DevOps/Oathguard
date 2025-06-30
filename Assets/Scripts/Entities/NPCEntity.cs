using System;
using UnityEngine.AI;

public class NPCEntity : EntityBase
{
    public EnemyAI AI;
    public EntityKnockback Knockback;
    public NPCAbilities AbilityController;
    public WeightedLootTable LootTable;

    public Action<NPCEntity> OnDespawned;

    public bool CanUseAbilities => !IsDead && AI.StateMachine.CurrentStateID != AIState.Knockback;

    protected override void Initialize()
    {
        base.Initialize();

        var weapon = EntityStatMapping.Instance().GetBaseStats(Type).Weapon;
        WeaponInstance = weapon.CreateInstance(this, HandSlotL, HandSlotR);

        Knockback = GetComponent<EntityKnockback>();
        Knockback.Initialize(this);

        AI = GetComponent<EnemyAI>();
        AI.Initialize(this);

        AbilityController = GetComponent<NPCAbilities>();
        AbilityController.Initialize(this, weapon.WeaponAbilities);

        DropTable

        StartCoroutine(NotifyNextFrame());
    }

    public override void OnEntityDied(string entityId)
    {
        if (entityId != Id) return;

        base.OnEntityDied(entityId);
        EntityManager.Instance.Player.Experience.AddXP(Stats.Experience);
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

