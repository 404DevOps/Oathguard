using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PlayerEntity : EntityBase
{
    [Header("Player Attributes")]
    public bool CanMove;
    public bool CanRotate;
    public bool CanUseAbilities;

    public PlayerAbilityController AbilityController;
    public EntityExperience Experience;

    void Awake()
    {
        Model = transform.Find("Model");
        GameEvents.OnWeaponSelected.AddListener(OnWeaponSelected);
        GameEvents.OnRoundStarted.AddListener(OnRoundStarted);
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

        Experience = GetComponent<EntityExperience>();
        Experience.Initialize(this);

        var movement = gameObject.GetComponent<PlayerMovement>();
        movement.Initialize();

        StartCoroutine(NotifyNextFrame());
    }

    private void OnRoundStarted()
    {
        //start
        CanMove = true;
        CanRotate = true;
        CanUseAbilities = true;
    }

    public override void OnEntityDied(EntityBase entity)
    {
        if (entity.Id != Id) return;

        CanMove = false;
        CanRotate = false;
        CanUseAbilities = false;

        base.OnEntityDied(entity);
        Debug.Log("Player died");
    }
}
