using static UnityEngine.EventSystems.EventTrigger;

public class NPCEntity : EntityBase
{
    void Awake()
    {
        Initialize();


        var weapon = EntityStatMapping.Instance().GetBaseStats(Type).Weapon;

        Weapon = GetComponentInChildren<WeaponHitbox>();
        Weapon.Initialize(this, weapon);

        StartCoroutine(NotifyNextFrame());
    }
}

