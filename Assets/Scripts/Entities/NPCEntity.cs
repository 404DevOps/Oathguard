using static UnityEngine.EventSystems.EventTrigger;

public class NPCEntity : EntityBase
{
    void Awake()
    {
        Initialize();

        var weapon = EntityStatMapping.Instance().GetBaseStats(Type).Weapon;
        WeaponInstance = weapon.CreateInstance(this, HandSlotL, HandSlotR);
        StartCoroutine(NotifyNextFrame());
    }
}

