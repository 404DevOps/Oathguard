using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEntity : EntityBase
{
    [Header("Player Attributes")]
    public bool CanMove;
    public bool CanRotate;
    public bool CanUseAbilities;

    public PlayerAbilityController AbilityController;

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
        WeaponInstance = weaponSet.CreateInstance(this, HandSlotL, HandSlotR);

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
