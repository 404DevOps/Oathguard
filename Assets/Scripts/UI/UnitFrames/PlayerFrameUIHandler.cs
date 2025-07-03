using System;
using UnityEngine;

public class PlayerFrameUIHandler : MonoBehaviour
{
    public PlayerEntity Player;

    [SerializeField] private HealthBarUIHandler _healthbar;
    [SerializeField] private ResourceBarUIHandler _resourcebar;
    [SerializeField] private ExperienceBarUIHandler _xpBar;

    [SerializeField] private bool _isPlayerFrame;
    private EntityHealth _entityHealth;
    private EntityResource _entityResource;

    void OnEnable()
    {
        GameEvents.OnEntityInitialized.AddListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.AddListener(OnHealthChanged);
        GameEvents.OnEntityResourceChanged.AddListener(OnResourceChanged);
        GameEvents.OnEntityShieldChanged.AddListener(OnEntityShieldChanged);
    }


    private void OnDisable()
    {
        GameEvents.OnEntityInitialized.RemoveListener(OnEntityInitialized);
        GameEvents.OnEntityHealthChanged.RemoveListener(OnHealthChanged);
        GameEvents.OnEntityResourceChanged.RemoveListener(OnResourceChanged);
        GameEvents.OnEntityShieldChanged.RemoveListener(OnEntityShieldChanged);
    }

    private void OnEntityShieldChanged(ShieldChangedEventArgs args)
    {
        if (Player == null || Player.Id != args.Entity.Id)
            return;

        _healthbar.SetNewShield(args.CurrentShield, false);
    }

    private void OnEntityInitialized(EntityBase entity)
    {
        if (entity is not PlayerEntity) return;
        InitializeFrame((PlayerEntity)entity);
    }

    private void InitializeFrame(PlayerEntity entity)
    {
        Player = entity;

        _entityHealth = Player.Health;
        _healthbar.InitializeBar(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);

        _entityResource = Player.Resource;
        var resourceData = AppearanceConfig.Instance().GetResourceData(_entityResource.ResourceType);
        _resourcebar.InitializeBar(_entityResource.CurrentResource, _entityResource.MaxResource, resourceData.ResourceBarColor);
        GameEvents.OnEntityResourceChanged.AddListener(OnResourceChanged);
    }

    private void OnHealthChanged(HealthChangedEventArgs data)
    {
        if (Player == null || Player.Id != data.Entity.Id)
            return;

        _healthbar.SetNewHealth(_entityHealth.CurrentHealth, _entityHealth.MaxHealth);
    }

    private void OnResourceChanged(ResourceChangedEventArgs data)
    {
        if (Player == null || Player.Id != data.Entity.Id)
            return;

        _resourcebar.SetNewValue(data.CurrentResource, data.MaxResource, false);
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