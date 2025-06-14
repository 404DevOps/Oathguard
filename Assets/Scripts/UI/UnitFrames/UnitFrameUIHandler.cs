using UnityEngine;

public class UnitFrameUIHandler : MonoBehaviour
{
    public EntityBase Entity;

    private HealthBarUIHandler _healthbar;
    private ResourceBarUIHandler _resourcebar;

    [SerializeField] private bool _isPlayerFrame;
    private EntityHealth _entityHealth;
    private EntityResource _entityResource;

    void OnEnable()
    {
        _healthbar = GetComponentInChildren<HealthBarUIHandler>(true);
        _resourcebar = GetComponentInChildren<ResourceBarUIHandler>(true);

        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.AddListener(OnHealthChanged);
        GameEvents.OnEntityResourceChanged.AddListener(OnResourceChanged);

    }
    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.RemoveListener(OnHealthChanged);
        GameEvents.OnEntityResourceChanged.RemoveListener(OnResourceChanged);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity is not PlayerEntity) return;
        InitializeFrame(entity);
    }

    private void InitializeFrame(EntityBase entity)
    {
        Entity = entity;

        _entityHealth = Entity.Health;
        _healthbar.InitializeBar(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);

        _entityResource = Entity.Resource;
        var resourceData = AppearanceConfig.Instance().GetResourceData(_entityResource.ResourceType);
        _resourcebar.InitializeBar(_entityResource.CurrentResource, _entityResource.MaxResource, resourceData.ResourceBarColor);
        GameEvents.OnEntityResourceChanged.AddListener(OnResourceChanged);
    }

    private void OnHealthChanged(HealthChangedEventArgs data)
    {
        if (Entity == null || Entity.Id != data.Entity.Id)
            return;

        _healthbar.SetNewHealth(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }

    private void OnResourceChanged(ResourceChangedEventArgs data)
    {
        if (Entity == null || Entity.Id != data.Entity.Id)
            return;

        _resourcebar.SetNewValue(data.CurrentResource, data.MaxResource, true);
    }

    //private void ActivateAuraGrid()
    //{
    //    AuraContainer.InitializeGrid(Entity.Id, AuraManager.Instance.GetAuraInfosForPlayer(Entity.Id));
    //    AuraContainer.gameObject.SetActive(true);
    //}
    //private void SetUnitFrameIcon()
    //{
    //    IconImage.sprite = AppearanceData.Instance().GetClassIcon(Entity.ClassType);
    //}
}