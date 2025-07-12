using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
        GameEvents.OnEntityDied.AddListener(OnEntityDied);
        GameEvents.OnEntityHealthChanged.AddListener(OnHealthChanged);

    }
    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityDied.RemoveListener(OnEntityDied);
        GameEvents.OnEntityHealthChanged.RemoveListener(OnHealthChanged);
    }

    private void Update()
    {
        if(Entity != null)

        transform.LookAt(transform.position + Utility.Camera.transform.rotation * Vector3.forward,
                         Utility.Camera.transform.rotation * Vector3.up);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity.Id != Entity.Id) return;
        ToggleHealthbar(true);
        InitializeFrame(entity);
    }

    private void ToggleHealthbar(bool v)
    {
        if (_healthbar.gameObject.activeSelf == v) return;

        _healthbar.gameObject.SetActive(v);
    }

    private void OnEntityDied(EntityBase entity)
    {
        if (entity.Id != Entity.Id) return;
        ToggleHealthbar(false);
    }

    private void InitializeFrame(EntityBase entity)
    {
        Entity = entity;

        _entityHealth = Entity.Health;
        _healthbar.InitializeBar(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }

    private void OnHealthChanged(HealthChangedEventArgs data)
    {
        if (Entity == null || Entity.Id != data.Entity.Id || data.Entity.IsDead)
            return;

        _healthbar.SetNewHealth(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }
}