using UnityEngine;

public class WorldUnitFrameUIHandler : MonoBehaviour
{
    public EntityBase Entity;

    private HealthBarUIHandler _healthbar;
     private EntityHealth _entityHealth;

    void OnEnable()
    {
        Entity = GetComponentInParent<EntityBase>();
        _healthbar = GetComponentInChildren<HealthBarUIHandler>(true);

        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.AddListener(OnHealthChanged);

    }
    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity.Id != Entity.Id) return;
        InitializeFrame(entity);
    }

    private void InitializeFrame(EntityBase entity)
    {
        Entity = entity;

        _entityHealth = Entity.Health;
        _healthbar.InitializeBar(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }

    private void OnHealthChanged(HealthChangedEventArgs data)
    {
        if (Entity == null || Entity.Id != data.Entity.Id)
            return;

        _healthbar.SetNewHealth(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }
}