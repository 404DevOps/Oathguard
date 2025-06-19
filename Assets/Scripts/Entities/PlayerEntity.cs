using System;
using UnityEngine;


public class PlayerEntity : EntityBase
{
    [Header("Player Attributes")]
    public bool CanMove;
    public bool CanRotate;
    public bool CanUseAbilities;

    public PlayerAbilityController AbilityController;

    public Transform HandSlotL;
    public Transform HandSlotR;

    void Awake()
    {
        GameEvents.OnWeaponSelected.AddListener(OnWeaponSelected);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    private void OnWeaponSelected(WeaponSet weaponSet)
    {
        Initialize();

        Animator.SetFloat("weaponType", (float)weaponSet.Type);

        var mainHand = Instantiate(weaponSet.MainHandPrefab, HandSlotR);
        if (weaponSet.OffhandPrefab != null)
            Instantiate(weaponSet.OffhandPrefab, HandSlotL);

        Weapon = mainHand.GetComponent<WeaponHitbox>();
        Weapon.Initialize(this, weaponSet);

        AbilityController = GetComponent<PlayerAbilityController>();
        AbilityController.Initialize(this, weaponSet.WeaponAbilities);

        var movement = gameObject.GetComponent<PlayerMovement>();
        movement.Initialize();

        CanMove = true;
        CanRotate = true;
        CanUseAbilities = true;

        StartCoroutine(NotifyNextFrame());
    }

    public override void OnEntityDied(string entityId)
    {
        if (entityId != Id) return;

        CanMove = false;
        CanRotate = false;
        CanUseAbilities = false;

        base.OnEntityDied(entityId);
        Debug.Log("Player died");
    }
}
