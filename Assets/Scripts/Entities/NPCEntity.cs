using static UnityEngine.EventSystems.EventTrigger;

public class NPCEntity : EntityBase
{
    public EntityKnockback Knockback;
    void Awake()
    {
        Initialize();

        var weapon = EntityStatMapping.Instance().GetBaseStats(Type).Weapon;
        WeaponInstance = weapon.CreateInstance(this, HandSlotL, HandSlotR);

        Knockback = GetComponent<EntityKnockback>();
        Knockback.Initialize(this);

        var enemyContext = GetComponent<EnemyAI>();
        enemyContext.Initialize(this);

        StartCoroutine(NotifyNextFrame());
    }

    public override void OnEntityDied(string entityId)
    {
        base.OnEntityDied(entityId);
        EntityManager.Instance.Player.Experience.AddXP(Stats.Experience);
    }
}

